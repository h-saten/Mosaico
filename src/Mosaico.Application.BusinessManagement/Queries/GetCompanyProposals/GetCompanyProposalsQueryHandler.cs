using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Services.v1;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyProposals
{
    public class GetCompanyProposalsQueryHandler : IRequestHandler<GetCompanyProposalsQuery, PaginatedResult<ProposalDTO>>
    {
        private readonly IBusinessDbContext _businessDb;
        private readonly IWalletClient _walletClient;
        private readonly IMapper _mapper;
        private readonly IDaoService _daoService;

        public GetCompanyProposalsQueryHandler(IBusinessDbContext businessDb, IMapper mapper, IWalletClient walletClient, IDaoService daoService)
        {
            _businessDb = businessDb;
            _mapper = mapper;
            _walletClient = walletClient;
            _daoService = daoService;
        }

        public async Task<PaginatedResult<ProposalDTO>> Handle(GetCompanyProposalsQuery request, CancellationToken cancellationToken)
        {
            var proposalsQuery = _businessDb.Proposals
                .Include(p => p.Company)
                .Include(t => t.Votes).AsNoTracking()
                .Where(c => c.CompanyId == request.CompanyId)
                .OrderByDescending(c => c.CreatedAt)
                .Skip(request.Skip).Take(request.Take);
            
            var proposals = await proposalsQuery.ToListAsync(cancellationToken);
            var count = await proposalsQuery.CountAsync(cancellationToken);

            var dtos = proposals.Select(p => _mapper.Map<ProposalDTO>(p)).ToList();
            foreach (var dto in dtos)
            {
                var proposal = proposals.FirstOrDefault(p => p.Id == dto.Id);
                if (proposal != null)
                {
                    dto.VoteCount = proposal.Votes.Count;
                    dto.AbstainCount = proposal.Votes.Where(t => t.Result == VoteResult.Abstain).Sum(t => t.Tokens);
                    dto.AgainstCount = proposal.Votes.Where(t => t.Result == VoteResult.Against).Sum(t => t.Tokens);
                    dto.ForCount = proposal.Votes.Where(t => t.Result == VoteResult.For).Sum(t => t.Tokens);
                    dto.Status = proposal.GetStatus();
                    if (proposal.TokenId != Guid.Empty)
                    {
                        var token = await _walletClient.GetTokenAsync(proposal.TokenId);
                        if (token == null)
                        {
                            throw new TokenNotFoundException(proposal.TokenId);
                        }
                        if (dto.VoteCount > 0)
                        {
                            dto.ForCountPercentage = dto.ForCount * 100 / token.TotalSupply;
                            dto.AgainstCountPercentage = dto.AgainstCount * 100 / token.TotalSupply;
                            dto.AbstainCountPercentage = dto.AbstainCount * 100 / token.TotalSupply;
                        }
                        var state = await _daoService.GetStateAsync(proposal.Network, proposal.Company.ContractAddress, proposal.ProposalId);
                        dto.QuorumReached = state == ProposalState.Succeeded;
                    }
                }
            }

            return new PaginatedResult<ProposalDTO>
            {
                Entities = dtos,
                Total = count
            };
        }
    }
}