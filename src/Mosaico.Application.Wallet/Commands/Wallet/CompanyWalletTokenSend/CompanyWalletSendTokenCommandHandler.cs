using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Domain.Wallet.ValueObjects;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Commands.Wallet.CompanyWalletTokenSend
{
    public class CompanyWalletSendTokenCommandHandler : IRequestHandler<CompanyWalletSendTokenCommand>
    {
        private readonly IUserManagementClient _managementClient; 
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly IAccountRepository _accountRepository;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IOperationService _operationService;
        
        public CompanyWalletSendTokenCommandHandler(IUserManagementClient managementClient, IWalletDbContext walletDbContext, ICurrentUserContext currentUser, IAccountRepository accountRepository, IEventFactory eventFactory, IEventPublisher eventPublisher, IDateTimeProvider timeProvider, IOperationService operationService)
        {
            _managementClient = managementClient;
            _walletDbContext = walletDbContext;
            _currentUser = currentUser;
            _accountRepository = accountRepository;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _timeProvider = timeProvider;
            _operationService = operationService;
        }

        public async Task<Unit> Handle(CompanyWalletSendTokenCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletDbContext.CompanyWallets.Include(t => t.Tokens).FirstOrDefaultAsync(c =>
                c.CompanyId == request.CompanyId, cancellationToken: cancellationToken);
            if (wallet == null)
            {
                throw new CompanyWalletNotFoundException(request.CompanyId.ToString());
            }

            var permissions = await _managementClient.GetUserPermissionsAsync(_currentUser.UserId, wallet.CompanyId, cancellationToken);
            if (!permissions.Any(p => p.Key == Authorization.Base.Constants.Permissions.Company.CanEditDetails))
            {
                throw new ForbiddenException();
            }
            
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken: cancellationToken);
            if (token == null)
            {
                throw new InsufficientFundsException(wallet.AccountAddress);
            }

            var contractAddress = token.Address;
            
            if (string.IsNullOrWhiteSpace(contractAddress))
            {
                throw new TokenNotFoundException($"Token not found");
            }

            var balanceResponse = await _accountRepository.Erc20BalanceAsync(wallet.AccountAddress, contractAddress, 
                wallet.Network, token.GetERCType());
            var balance = new Wei(balanceResponse);
            if (request.Amount > balance.ToDecimal())
            {
                throw new InsufficientFundsException(wallet.AccountAddress);
            }

            var transactionStatus =
                await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionStatuses.Pending, cancellationToken);

            var transactionType =
                await _walletDbContext.TransactionType.FirstOrDefaultAsync(
                    t => t.Key == Domain.Wallet.Constants.TransactionType.Transfer, cancellationToken);
            
            var operation = await _operationService.CreateDaoTransferOperationAsync(wallet.CompanyId, _currentUser.UserId, token.Network, token.Address, token.Symbol);

            var correlationId = Guid.NewGuid();
            var transaction = new Transaction
            {
                From = wallet.AccountAddress,
                To = request.Address,
                Status = transactionStatus,
                StatusId = transactionStatus.Id,
                Network = wallet.Network,
                Type = transactionType,
                TypeId = transactionType.Id,
                CorrelationId = correlationId.ToString(),
                PaymentProcessor = Constants.PaymentProcessors.Mosaico,
                TokenAmount = request.Amount,
                TokenId = request.TokenId,
                WalletAddress = wallet.AccountAddress,
                InitiatedAt = _timeProvider.Now(),
                UserId = _currentUser.UserId
            };
            _walletDbContext.Transactions.Add(transaction);
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            await PublishEventAsync(transaction.Id, operation.Id);
            return Unit.Value;
        }

        private async Task PublishEventAsync(Guid transactionId, Guid operationId)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new NonCustodialTransactionInitiated(transactionId, operationId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}