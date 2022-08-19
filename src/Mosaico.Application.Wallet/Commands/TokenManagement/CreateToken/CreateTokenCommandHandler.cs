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
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.CreateToken
{
    public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, Guid>
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

        public CreateTokenCommandHandler(IWalletDbContext walletDbContext, IEventPublisher eventPublisher, IEventFactory eventFactory, IMapper mapper, IProjectManagementClient projectManagement, IUserManagementClient managementClient, ICurrentUserContext currentUser, ITokenPermissionFactory permissionFactory, ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _mapper = mapper;
            _projectManagement = projectManagement;
            _managementClient = managementClient;
            _currentUser = currentUser;
            _permissionFactory = permissionFactory;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId && c.Network == request.Network, cancellationToken: cancellationToken);
            if (companyWallet == null)
            {
                throw new CompanyNotFoundException(request.CompanyId);
            }

            if (request.ProjectId.HasValue)
            {
                var project = await _projectManagement.GetProjectAsync(request.ProjectId.Value, cancellationToken);
                if (project == null)
                {
                    throw new ProjectNotFoundException(request.ProjectId.Value);
                }
                var userPermissions = await _managementClient.GetUserPermissionsAsync(_currentUser.UserId, project.Id, cancellationToken);
                if (!userPermissions.Any(p => p.Key == Authorization.Base.Constants.Permissions.Project.CanEditDetails) && !_currentUser.IsGlobalAdmin)
                {
                    throw new ForbiddenException("No permissions to update project");
                }
            }

            var token = new Token();
            _mapper.Map(request, token);
            token.Status = TokenStatus.Pending;
            var tokenType = await _walletDbContext.TokenTypes.FirstOrDefaultAsync(t => t.Key == request.TokenType, cancellationToken);
            if (tokenType == null)
            {
                throw new TokenTypeNotFoundException(request.TokenType);
            }

            if (!string.IsNullOrWhiteSpace(request.ContractAddress) && !string.IsNullOrWhiteSpace(request.OwnerAddress))
            {
                if (!Integration.Blockchain.Ethereum.Constants.TokenContractVersions.All.Contains(
                    request.ContractVersion))
                {
                    throw new UnknownContractVersionException("erc20", request.ContractVersion);
                }
                //TODO: verify if contract really exists?
                token.Address = request.ContractAddress;
                token.OwnerAddress = request.OwnerAddress;
                token.ContractVersion = request.ContractVersion;
                token.Status = TokenStatus.Deployed;
            }
            
            token.SetType(tokenType);
            
            _walletDbContext.Tokens.Add(token);
            companyWallet.Tokens.Add(new CompanyWalletToToken
            {
                Token = token,
                TokenId = token.Id
            });
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            await PublishPermissionEventAsync(token.Id);
            await PublishEvents(token.Id, request.ProjectId);
            return token.Id;
        }

        private async Task PublishEvents(Guid tokenId, Guid? projectId)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets, new TokenCreated(tokenId, projectId));
            await _eventPublisher.PublishAsync(e);
        }

        private async Task PublishPermissionEventAsync(Guid id)
        {
            var permissions = await _permissionFactory.GetRolePermissionsAsync();
            await _permissionFactory.AddUserPermissionsAsync(id, _currentUser.UserId, permissions);
        }
    }
}