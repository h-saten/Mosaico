using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.Base;
using Mosaico.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(SendInvestorCertificateOnTransactionFinalized), "wallets:api")]
    [EventTypeFilter(typeof(PurchaseTransactionConfirmedEvent))]
    public class SendInvestorCertificateOnTransactionFinalized : EventHandlerBase
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateEngine _templateEngine;
        private readonly IResourceManager _resourceManager;
        private readonly IWalletClient _walletClient;
        private readonly ICertificateGeneratorService _certificateGeneratorService;
        private readonly IUserManagementClient _managementClient;
        private readonly ILogger _logger;

        public SendInvestorCertificateOnTransactionFinalized(
            IProjectDbContext projectDbContext, 
            IEmailSender emailSender, 
            ITemplateEngine templateEngine, 
            IResourceManager resourceManager, 
            IWalletClient walletClient, 
            ICertificateGeneratorService certificateGeneratorService, 
            IUserManagementClient managementClient, 
            ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _emailSender = emailSender;
            _templateEngine = templateEngine;
            _resourceManager = resourceManager;
            _walletClient = walletClient;
            _certificateGeneratorService = certificateGeneratorService;
            _managementClient = managementClient;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var eventData = @event?.GetData<PurchaseTransactionConfirmedEvent>();
            if (eventData is null)
            {
                return;
            }
            
            var transaction = await _walletClient.GetTransactionAsync(eventData.TransactionId);
            if (transaction is null)
            {
                _logger.Warning($"Transaction with id: '{eventData.TransactionId}' not found while try to send certificate.");
                return;
            }

            // TODO probably there should be available user language
            // var user = await _userManagementClient.GetUserAsync(transaction.UserId);
            //
            // if (user is null)
            // {
            //     _logger.Warning($"User with id: '{eventData.TransactionId}' not found while try to send certificate.");
            //     return;
            // }
            
            var project =
                await _projectDbContext
                    .Projects
                    .Include(t => t.InvestorCertificate.Backgrounds)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TokenId == transaction.TokenId);

            var sendingCertificatesEnabled =
                project?.InvestorCertificate is not null 
                && project.InvestorCertificate.IsSendingEnabled();
            
            if (project is null || project.TokenId is null || !sendingCertificatesEnabled)
            {
                return; 
            }
            
            var currentUser = await _managementClient.GetCurrentUserAsync();
            var userName = $"{currentUser.FirstName} {currentUser.LastName}";

            var userTokenInvestmentsSummary = await _walletClient.GetTokenSummaryAsync((Guid) project.TokenId, new Guid(transaction.UserId));
            
            var certificateImgUrl = await _certificateGeneratorService.AsImageUrlAsync(x =>
            {
                x.Language = Base.Constants.Languages.English;
                x.ProjectId = project.Id;
                x.Date = DateTimeOffset.UtcNow;
                x.SequenceNumber = userTokenInvestmentsSummary.InvestorSequenceNumber;
                x.FinalizedTransactionsAmount = userTokenInvestmentsSummary.InvestorFinalizedTransactions;
                x.TokensAmount = userTokenInvestmentsSummary.PaidTokensAmount;
                x.UserName = userName;
                x.TokenSymbol = userTokenInvestmentsSummary.TokenSymbol;
            });
            
            // TODO get language from user entity
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "GeneralCertificateEmail");
            var subjectTemplate = await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects, "GeneralCertificateEmail");
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            // TODO to prepare valid parameters list 
            var payload = new Dictionary<string, string>
            {
                { "ProjectLogoPath", logoFileName }, 
                {"TokenName", "MOS"},
                {"CertificateImgPath", certificateImgUrl},
                {"CertificateUrlEncoded", HttpUtility.UrlEncode(certificateImgUrl)},
                {"ShowInvestorPackageSection", null}
            };
            var email = new Email
            {
                Html = _templateEngine.Build(template, payload),
                Recipients = new List<string>() {"dev@mosaico.ai"}, // TODO to implement
                Subject = _templateEngine.Build(subjectTemplate, payload),
                Attachments = new List<EmailAttachment>
                {
                    new()
                    {
                        Content = mosaicoLogo,
                        IsInline = true,
                        FileName = logoFileName,
                        ContentType = MimeTypes.GetMimeType(logoFileName)
                    }
                }
            };
            await _emailSender.SendAsync(email);
        }
    }
}