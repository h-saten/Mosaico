using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Permissions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Models;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.ImportToken
{
    public class ImportTokenCommandHandler : IRequestHandler<ImportTokenCommand, Guid>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IMapper _mapper;
        private readonly IProjectManagementClient _projectManagement;
        private readonly IUserManagementClient _managementClient;
        private readonly ICurrentUserContext _currentUser;
        private readonly ITokenPermissionFactory _permissionFactory;
        private readonly ITokenService _tokenService;

        public ImportTokenCommandHandler(
            IWalletDbContext walletDbContext, 
            IEventPublisher eventPublisher, 
            IEventFactory eventFactory, 
            IMapper mapper, 
            IProjectManagementClient projectManagement, 
            IUserManagementClient managementClient, 
            ICurrentUserContext currentUser, 
            ITokenPermissionFactory permissionFactory, 
            ITokenService tokenService, 
            ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _mapper = mapper;
            _projectManagement = projectManagement;
            _managementClient = managementClient;
            _currentUser = currentUser;
            _permissionFactory = permissionFactory;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Guid> Handle(ImportTokenCommand request, CancellationToken cancellationToken)
        {
            if (request.ProjectId.HasValue)
            {
                await IfUserCanManageProjectGuardAsync(request, cancellationToken);
            }

            var token = await AddNewTokenToCompanyWalletAsync(request, cancellationToken);
            await PublishPermissionEventAsync(token.Id);
            await PublishEvents(token.Id, request.ProjectId);
            return token.Id;
        }

        private async Task<Token> AddNewTokenToCompanyWalletAsync(ImportTokenCommand request, CancellationToken cancellationToken)
        {
            var companyWallet = await FetchWalletAsync(request.CompanyId, request.Network, cancellationToken);

            var token = await InitializeTokenAsync(request, companyWallet, cancellationToken);

            _walletDbContext.Tokens.Add(token);
            companyWallet.Tokens.Add(new CompanyWalletToToken
            {
                Token = token,
                TokenId = token.Id
            });

            await _walletDbContext.SaveChangesAsync(cancellationToken);

            return token;
        }

        private async Task<Token> InitializeTokenAsync(ImportTokenCommand request, CompanyWallet companyWallet, CancellationToken cancellationToken)
        {
            var tokenDetails = await _tokenService.GetDetailsAsync(request.Network, request.ContractAddress);
            
            var token = new Token();
            _mapper.Map(tokenDetails, token);
            token.Status = TokenStatus.Deployed;
            token.Network = companyWallet.Network;
            token.CompanyId = companyWallet.CompanyId;
            token.OwnerAddress = request.OwnerAddress;
            token.ContractVersion = request.ContractVersion;

            if (!Integration.Blockchain.Ethereum.Constants.TokenContractVersions.All.Contains(request.ContractVersion))
            {
                throw new UnknownContractVersionException("erc20", request.ContractVersion);
            }
            
            var tokenType = await _walletDbContext
                .TokenTypes
                .FirstOrDefaultAsync(t => t.Key == request.TokenType, cancellationToken);
            if (tokenType == null)
            {
                throw new TokenTypeNotFoundException(request.TokenType);
            }
            token.SetType(tokenType);
            
            return token;
        }

        private async Task IfUserCanManageProjectGuardAsync(ImportTokenCommand request, CancellationToken cancellationToken)
        {
            var project = await FetchProjectAsync(request, cancellationToken);
            await ValidateIfUserCanManageProjectAsync(cancellationToken, project);
        }

        private async Task ValidateIfUserCanManageProjectAsync(CancellationToken cancellationToken, MosaicoProject project)
        {
            var userPermissions =
                await _managementClient.GetUserPermissionsAsync(_currentUser.UserId, project.Id, cancellationToken);
            if (!userPermissions.Any(p => p.Key == Authorization.Base.Constants.Permissions.Project.CanEditDetails))
            {
                throw new ForbiddenException("No permissions to update project");
            }
        }

        private async Task<MosaicoProject> FetchProjectAsync(ImportTokenCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectManagement.GetProjectAsync(request.ProjectId.Value, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId.Value);
            }

            return project;
        }

        private async Task<CompanyWallet> FetchWalletAsync(Guid companyId, string network, CancellationToken cancellationToken)
        {
            var companyWallet = await _walletDbContext
                .CompanyWallets
                .FirstOrDefaultAsync(c => c.CompanyId == companyId && c.Network == network, cancellationToken);

            if (companyWallet == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            return companyWallet;
        }

        private async Task PublishEvents(Guid tokenId, Guid? projectId)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets, new TokenImported(tokenId, projectId));
            await _eventPublisher.PublishAsync(e);
        }

        private async Task PublishPermissionEventAsync(Guid id)
        {
            var permissions = await _permissionFactory.GetRolePermissionsAsync();
            await _permissionFactory.AddUserPermissionsAsync(id, _currentUser.UserId, permissions);
        }
    }
}