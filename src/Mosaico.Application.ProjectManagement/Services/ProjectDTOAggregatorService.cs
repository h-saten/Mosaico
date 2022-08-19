using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.ProjectManagement.Services
{
    public class ProjectDTOAggregatorService : IProjectDTOAggregatorService
    {
        private readonly IMapper _mapper;
        private readonly IWalletClient _walletClient;
        private readonly ICurrentUserContext _currentUser;
        private readonly IProjectDbContext _projectDbContext;

        public ProjectDTOAggregatorService(IMapper mapper, IWalletClient walletClient, ICurrentUserContext currentUser, IProjectDbContext projectDbContext)
        {
            _mapper = mapper;
            _walletClient = walletClient;
            _currentUser = currentUser;
            _projectDbContext = projectDbContext;
        }

        public async Task<ProjectDTO> FillInDTOAsync(Project project, ProjectDTO input = null)
        {
            var dto = input == null ? _mapper.Map<ProjectDTO>(project) : _mapper.Map(project, input);
            dto.ShortDescription = project.Page?.ShortDescription?.GetTranslationInLanguage(_currentUser.Language)?.Value;
            var currentStage = project.ActiveStage();
            if (currentStage != null)
            {
                dto.ActiveStage = _mapper.Map<StageDTO>(currentStage);
                var limits = await _projectDbContext.StagePurchaseLimits.Where(s => s.StageId == dto.ActiveStage.Id)
                    .Where(s => s.MinimumPurchase >= 0)
                    .Select(s => s.MinimumPurchase).ToListAsync();
                
                if (limits.Any() && limits.Min() < dto.ActiveStage.MinimumPurchase) 
                    dto.ActiveStage.MinimumPurchase = limits.Min();
            }
            
            if (project.TokenId.HasValue && project.TokenId.Value != Guid.Empty)
            {
                var token = await _walletClient.GetTokenAsync(dto.TokenId);
                if (token == null)
                {
                    throw new TokenNotFoundException(dto.TokenId);
                }
                dto.IsExchangeAvailable = token.Exchanges.Any();
                dto.TokenName = token.Name;
                dto.TokenSymbol = token.Symbol;
                var raisedCapital = await _walletClient.GetTokenRaisedAmountAsync(dto.TokenId, project.Id);
                dto.RaisedCapital = raisedCapital.Item1 ?? 0;
                dto.RaisedCapitalInUSD = raisedCapital.Item2 ?? 0;
                var airdrops = await _projectDbContext.AirdropCampaigns.Where(a => a.ProjectId == project.Id && a.CountAsPurchase && a.StageId.HasValue).ToListAsync();
                foreach (var airdrop in airdrops)
                {
                    var stage = project.Stages.FirstOrDefault(t => t.Id == airdrop.StageId.Value);
                    if (stage != null)
                    {
                        dto.RaisedCapital += airdrop.TotalCap;
                        dto.RaisedCapitalInUSD += airdrop.TotalCap * stage.TokenPrice;
                    }
                }
                dto.NumberOfBuyers = await _walletClient.GetNumberOfBuyersPerToken(dto.TokenId) ?? default;
                dto.HardCap = project.Crowdsale?.HardCap ?? 0;
                dto.SoftCap = project.Crowdsale?.SoftCap ?? 0;
                dto.RaisedCapitalPercentage = dto.HardCap > 0 ? dto.RaisedCapital * 100 / dto.HardCap : 0;
                dto.RaisedCapitalSoftCapPercentage = dto.SoftCap > 0 ? dto.RaisedCapital * 100 / dto.SoftCap : 0;
                dto.IsSoftCapAchieved = dto.RaisedCapitalSoftCapPercentage >= 100;
                dto.Network = token.Network;
                dto.CoverLogoUrl = project.Page.PageCovers?.FirstOrDefault()?.GetTranslationInLanguage(_currentUser.Language)
                    ?.Value;
                if (currentStage != null)
                {
                    var tokenTransactions = await _walletClient.StageTransactionsDetails(dto.TokenId, dto.ActiveStage.Id);
                    if (tokenTransactions != null)
                    {
                        dto.ActiveStage.SoldTokens = tokenTransactions.SoldTokensAmount;
                        dto.ActiveStage.ProgressPercentage = dto.ActiveStage.TokensSupply > 0 ? 
                            dto.ActiveStage.SoldTokens * 100 / dto.ActiveStage.TokensSupply : 0;
                    }
                }
            }

            dto.LikeCount = project.Likes.Distinct().Count();
            if (_currentUser.IsAuthenticated)
            {
                dto.LikedByUser = project.Likes.Distinct().Any(l => l.UserId == _currentUser.UserId);
                dto.IsUserSubscribeProject = _projectDbContext.ProjectNewsletterSubscriptions.Any(t => t.ProjectId == project.Id && t.UserId == _currentUser.UserId);
            }

            dto = CalculateCapsToUserCurrency(project, dto);

            dto.MarketplaceStatus = GetMarketplaceStatus(project);

            return dto;
        }

        private static ProjectDTO CalculateCapsToUserCurrency(Project project, ProjectDTO dto)
        {
            var stagesSupply = project.Stages.Sum(projectStage => projectStage.TokensSupply * projectStage.TokenPrice);

            var softCap = 0m;
            
            var stage = project.Stages.FirstOrDefault();
            if (stage != null && stage.TokensSupply >= dto.SoftCap)
            {
                softCap = dto.SoftCap * stage.TokenPrice;
            }
            else
            {
                var capsRatio = decimal.Zero;
                if (dto.HardCap > 0)
                {
                    capsRatio = (dto.SoftCap * 100) / dto.HardCap / 100;
                }
                softCap = stagesSupply * capsRatio;
            }
            
            
            dto.HardCapInUserCurrency = stagesSupply;
            dto.SoftCapInUserCurrency = softCap;
            return dto;
        }

        public async Task<ProjectDetailDTO> FillInDetailDTOAsync(Project project)
        {
            var projectDTO = new ProjectDetailDTO();
            await FillInDTOAsync(project, projectDTO);
            projectDTO.CompanyId = project.CompanyId;
            projectDTO.LegacyId = project.LegacyId;
            projectDTO.PaymentMethods = project.PaymentMethods.Select(x => x.Key).ToList();
            projectDTO.PaymentCurrencies = project.Crowdsale?.SupportedStableCoins;
            projectDTO.Stages = project.Stages.Select(s => _mapper.Map<StageDTO>(s)).ToList();

            return projectDTO;
        }

        private string GetMarketplaceStatus(Project project)
        {
            var upcomingStatusKey = Domain.ProjectManagement.Constants.MarketplaceStatuses.Upcoming;
            var privateStatusKey = Domain.ProjectManagement.Constants.MarketplaceStatuses.PrivateSale;
            var publicSaleStatusKey = Domain.ProjectManagement.Constants.MarketplaceStatuses.PublicSale;
            var preSaleStatusKey = Domain.ProjectManagement.Constants.MarketplaceStatuses.PreSale;
            var approvedStatusKey = Domain.ProjectManagement.Constants.ProjectStatuses.Approved;
            var inProgressStatusKey = Domain.ProjectManagement.Constants.ProjectStatuses.InProgress;
            var closedStatusKey = Domain.ProjectManagement.Constants.ProjectStatuses.Closed;
            
            var pendingStageStatus = Domain.ProjectManagement.Constants.StageStatuses.Pending;
            var activeStageStatus = Domain.ProjectManagement.Constants.StageStatuses.Active;

            if ((project.Status.Key == approvedStatusKey || project.Status.Key == inProgressStatusKey) 
                && project.Stages.Any(s => s.Status.Key == pendingStageStatus) && project.Stages.All(s => s.Status.Key != activeStageStatus))
            {
                return upcomingStatusKey;
            }
            else if (project.Status.Key == closedStatusKey)
            {
                return closedStatusKey;
            }
            else if (project.Status.Key == inProgressStatusKey && project.Stages.Any(s =>
                s.Status.Key == activeStageStatus && s.Type == StageType.Private))
            {
                return privateStatusKey;
            }
            else if (project.Status.Key == inProgressStatusKey && project.Stages.Any(s =>
                s.Status.Key == activeStageStatus && s.Type == StageType.Public))
            {
                return publicSaleStatusKey;
            }
            else if (project.Status.Key == inProgressStatusKey
                     && project.Stages.Any(s =>
                         s.Status.Key == activeStageStatus && s.Type == StageType.PreSale))
            {
                return preSaleStatusKey;
            }
            return null;
        }
    }
}