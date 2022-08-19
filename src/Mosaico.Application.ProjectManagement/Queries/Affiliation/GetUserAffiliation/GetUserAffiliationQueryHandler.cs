using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.DTOs.Affiliation;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.Affiliation.GetUserAffiliation
{
    public class GetUserAffiliationQueryHandler : IRequestHandler<GetUserAffiliationQuery, GetUserAffiliationQueryResponse>
    {
        private readonly IWalletClient _walletClient;
        private readonly IUserAffiliationService _affiliationService;
        private readonly IMapper _mapper;
        
        public GetUserAffiliationQueryHandler(IWalletClient walletClient, IMapper mapper, IUserAffiliationService affiliationService)
        {
            _walletClient = walletClient;
            _mapper = mapper;
            _affiliationService = affiliationService;
        }

        public async Task<GetUserAffiliationQueryResponse> Handle(GetUserAffiliationQuery request, CancellationToken cancellationToken)
        {
            var userAffiliation = await _affiliationService.GetOrCreateUserAffiliation(request.UserId);
            var projects = new List<UserAffiliationPartnerDTO>();
            foreach (var partnerAssignment in userAffiliation.PartnerAssignments)
            {
                if (partnerAssignment.ProjectAffiliation?.Project?.TokenId != null)
                {
                    var token = await _walletClient.GetTokenAsync(partnerAssignment.ProjectAffiliation.Project.TokenId.Value);
                    if (token != null)
                    {
                        projects.Add(new UserAffiliationPartnerDTO
                        {
                            ProjectTitle = partnerAssignment.ProjectAffiliation.Project.Title,
                            ProjectId = partnerAssignment.ProjectAffiliation.Project.Id,
                            ProjectSlug = partnerAssignment.ProjectAffiliation.Project.Slug,
                            EstimatedReward = partnerAssignment.PartnerTransactions.Sum(p => p.EstimatedReward),
                            TransactionsCount = partnerAssignment.PartnerTransactions.Count,
                            Token = _mapper.Map<TokenDTO>(token)
                        });
                    }
                }
            }

            return new GetUserAffiliationQueryResponse
            {
                Id = userAffiliation.Id,
                AccessCode = userAffiliation.AccessCode,
                Projects = projects
            };
        }
    }
}