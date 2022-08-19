using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.GetUserCertificate
{
    public class GetUserCertificateQueryHandler : IRequestHandler<GetUserCertificateQuery, FileContentResult>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IUserManagementClient _managementClient;
        private readonly IWalletClient _walletClient;
        private readonly ICertificateGeneratorService _certificateGenerator;

        public GetUserCertificateQueryHandler(
            IProjectDbContext projectDbContext, 
            IUserManagementClient managementClient, 
            IWalletClient walletClient,
            ICertificateGeneratorService certificateGenerator)
        {
            _projectDbContext = projectDbContext;
            _managementClient = managementClient;
            _walletClient = walletClient;
            _certificateGenerator = certificateGenerator;
        }

        // TODO abort when user is not investor?
        public async Task<FileContentResult> Handle(GetUserCertificateQuery request, CancellationToken cancellationToken)
        {
            var project =
                await _projectDbContext
                    .Projects
                    .Include(t => t.InvestorCertificate.Backgrounds)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == request.ProjectId, cancellationToken);
            
            if (project == null || project.TokenId == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            var currentUser = await _managementClient.GetCurrentUserAsync(cancellationToken);
            var userName = $"{currentUser.FirstName} {currentUser.LastName}";

            var userTokenInvestmentsSummary = await _walletClient.GetTokenSummaryAsync((Guid) project.TokenId, new Guid(request.UserId));
            
            var file = await _certificateGenerator.AsPdfAsync(x =>
            {
                x.Language = Base.Constants.Languages.English;
                x.ProjectId = request.ProjectId;
                x.Date = DateTimeOffset.UtcNow;
                x.SequenceNumber = userTokenInvestmentsSummary.InvestorSequenceNumber;
                x.FinalizedTransactionsAmount = userTokenInvestmentsSummary.InvestorFinalizedTransactions;
                x.TokensAmount = userTokenInvestmentsSummary.PaidTokensAmount;
                x.UserName = userName;
                x.TokenSymbol = userTokenInvestmentsSummary.TokenSymbol;
            }, cancellationToken);
            
            return file;
        }
    }
}