using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Queries.GetToken
{
    public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, TokenDTO>
    {
        private readonly ILogger _logger;
        private readonly IWalletDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IProjectManagementClient _managementClient;

        public GetTokenQueryHandler(IWalletDbContext dbContext, IMapper mapper, IProjectManagementClient managementClient, ILogger logger = null)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _managementClient = managementClient;
            _logger = logger;
        }

        public async Task<TokenDTO> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            var token = await _dbContext.Tokens
                .Include(t => t.Type)
                .Include(t => t.Exchanges).ThenInclude(e => e.ExternalExchange)
                .Include(t => t.Stakings)
                .Include(t => t.Vestings).ThenInclude(v => v.Funds)
                .Include(t => t.Deflation)
                .Include(t => t.Vault)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(request.Id);
            }

            var projects = await _managementClient.GetProjectsByTokenAsync(token.Id, cancellationToken);

            var dto = _mapper.Map<TokenDTO>(token);
            dto.Projects = projects.Select(p => new TokenProjectDTO
            {
                Id = p.Id,
                Slug = p.Slug,
                Title = p.Title,
                LogoUrl = p.LogoUrl
            }).ToList();
            return dto;
        }
    }
}