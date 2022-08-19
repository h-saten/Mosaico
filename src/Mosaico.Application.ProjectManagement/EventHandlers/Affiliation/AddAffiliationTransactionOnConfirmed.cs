using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers.Affiliation
{
    [EventInfo(nameof(AddAffiliationTransactionOnConfirmed), "wallets:api")]
    [EventTypeFilter(typeof(PurchaseTransactionConfirmedEvent))]
    public class AddAffiliationTransactionOnConfirmed : EventHandlerBase
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IWalletClient _walletClient;
        private readonly ILogger _logger;

        public AddAffiliationTransactionOnConfirmed(IProjectDbContext projectDbContext, ILogger logger, IWalletClient walletClient)
        {
            _projectDbContext = projectDbContext;
            _logger = logger;
            _walletClient = walletClient;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<PurchaseTransactionConfirmedEvent>();
            if (data != null && !string.IsNullOrEmpty(data.RefCode))
            {
                var affiliation = await _projectDbContext.ProjectAffiliations.FirstOrDefaultAsync(p => p.ProjectId == data.ProjectId);
                if (affiliation == null || !affiliation.IsEnabled)
                {
                    _logger.Verbose($"Affiliation is disabled for project {data.ProjectId}. Skipping transaction...");
                    return;
                }
                
                var userAffiliation = await _projectDbContext.UserAffiliations.FirstOrDefaultAsync(u => u.AccessCode == data.RefCode);
                if (userAffiliation == null)
                {
                    var normalizedUserId = data.UserId.ToLowerInvariant().Trim();
                    var now = DateTimeOffset.UtcNow;
                    
                    var reference = await _projectDbContext.AffiliationReferences.FirstOrDefaultAsync(r => r.ReferencedUserId == normalizedUserId);
                    userAffiliation = reference?.UserAffiliation;
                    
                    if (userAffiliation == null || now > reference?.CreatedAt.AddDays(7))
                    {
                        _logger?.Warning($"Affiliation user with ref. code {data.RefCode} was not found");
                        return;
                    }
                }

                if (userAffiliation.UserId.ToLowerInvariant() != data.UserId.ToLowerInvariant())
                {
                    var partner = userAffiliation.PartnerAssignments.FirstOrDefault(p => p.ProjectAffiliationId == affiliation.Id);
                    if (partner == null)
                    {
                        if (!affiliation.EverybodyCanParticipate)
                        {
                            _logger?.Verbose($"Affiliation {affiliation.Id} does not accept everybody.");
                            return;
                        }
                        partner = new Partner
                        {
                            Status = PartnerStatus.ENABLED,
                            AccessCode = userAffiliation.AccessCode,
                            PaymentStatus = PartnerPaymentStatus.PENDING,
                            ProjectAffiliation = affiliation,
                            ProjectAffiliationId = affiliation.Id,
                            UserAffiliation = userAffiliation,
                            UserAffiliationId = userAffiliation.Id,
                            RewardPercentage = affiliation.RewardPercentage
                        };
                        await _projectDbContext.Partners.AddAsync(partner);
                    }

                    if (partner.PartnerTransactions.Any(p => p.PurchasedById == data.UserId) && !affiliation.IncludeAll)
                    {
                        _logger?.Verbose($"user {data.UserId} already performed purchase for this partner. skipping");
                        return;
                    }

                    var estimatedReward = data.TokensAmount * (partner.RewardPercentage / 100);
                    var totalRewarded = affiliation.Partners.SelectMany(p => p.PartnerTransactions).Sum(t => t.EstimatedReward);
                    if (totalRewarded + estimatedReward > affiliation.RewardPool)
                    {
                        _logger?.Error($"Affiliation {affiliation.Id} exhausted");
                        return;
                    }

                    if (affiliation.PartnerShouldBeInvestor)
                    {
                        var isInvestor = await _walletClient.IsInvestorAsync(affiliation.ProjectId, userAffiliation.UserId);
                        if (!isInvestor)
                        {
                            _logger.Error($"user is not an investor of {affiliation.Id} affiliation. skipping");
                            return;
                        }
                    }

                    if (partner.Status == PartnerStatus.ENABLED)
                    {
                        _projectDbContext.PartnerTransactions.Add(new PartnerTransaction
                        {
                            PurchasedTokens = data.TokensAmount,
                            PartnerId = partner.Id,
                            Partner = partner,
                            TransactionCorrelationId = data.TransactionCorrelationId,
                            EstimatedReward = estimatedReward,
                            PurchasedById = data.UserId
                        });
                    }

                    await _projectDbContext.SaveChangesAsync();
                }
            }
        }
    }
}