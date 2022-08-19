using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;

namespace Mosaico.Application.Wallet.Commands.Vesting.CreateVesting
{
    public class CreateVestingCommandHandler : IRequestHandler<CreateVestingCommand, Guid>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IOperationService _operationService;
        private readonly IWalletDbContext _walletDbContext;

        public CreateVestingCommandHandler(IWalletDbContext walletDbContext, IEventFactory eventFactory,
            IEventPublisher eventPublisher, ICurrentUserContext currentUserContext, IOperationService operationService)
        {
            _walletDbContext = walletDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _currentUserContext = currentUserContext;
            _operationService = operationService;
        }

        public async Task<Guid> Handle(CreateVestingCommand request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId,
                cancellationToken);
            if (token == null) throw new TokenNotFoundException();
            var vault = await _walletDbContext.Vaults.FirstOrDefaultAsync(v => v.TokenId == token.Id,
                cancellationToken);
            if (vault == null) throw new VaultNotFoundException(token.Id);
            request.StartsAt ??= DateTimeOffset.UtcNow;
            var vesting = new Domain.Wallet.Entities.Vesting
            {
                Name = request.Name,
                Token = token,
                TokenId = token.Id,
                TokenAmount = request.TokenAmount,
                InitialPaymentPercentage = request.ImmediatePay,
                Vault = vault,
                VaultId = vault.Id,
                WalletAddress = request.WalletAddress,
                NumberOfDays = request.NumberOfDays,
                StartsAt = request.StartsAt
            };
            await _walletDbContext.Vestings.AddAsync(vesting, cancellationToken);
            var initialPaymentTokenAmount = 0m;
            if (request.ImmediatePay > 0)
            {
                initialPaymentTokenAmount = vesting.TokenAmount * (request.ImmediatePay / 100);
                vesting.Funds.Add(new VestingFund
                {
                    Days = 0,
                    Name = $"{request.Name}_0",
                    TokenAmount = initialPaymentTokenAmount,
                    StartAt = request.StartsAt,
                    VestingId = vesting.Id,
                    Vesting = vesting,
                    Status = VestingFundStatus.Pending
                });
            }

            var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w =>
                    w.AccountAddress == request.WalletAddress && w.Network == token.Network,
                cancellationToken);
            if (wallet != null)
            {
                await _walletDbContext.WalletToVestings.AddAsync(new WalletToVesting
                {
                    WalletId = wallet.Id,
                    VestingId = vesting.Id
                }, cancellationToken);
                await _walletDbContext.SaveChangesAsync(cancellationToken);
            }

            var tokenAmountForVesting = vesting.TokenAmount - initialPaymentTokenAmount;
            var tokensPerClaim = tokenAmountForVesting / request.AmountOfClaims;
            var daysPerClaim = (int) Math.Floor((decimal) request.NumberOfDays / request.AmountOfClaims);
            var currentStartDate = vesting.StartsAt.Value.AddDays(daysPerClaim);

            for (var i = 0; i < request.AmountOfClaims; i++)
            {
                var fund = new VestingFund
                {
                    Days = daysPerClaim,
                    Status = VestingFundStatus.Pending,
                    Name = $"{request.Name}_{i + 1}",
                    StartAt = currentStartDate,
                    Vesting = vesting,
                    VestingId = vesting.Id,
                    TokenAmount = tokensPerClaim
                };
                vesting.Funds.Add(fund);
                currentStartDate = currentStartDate.AddDays(daysPerClaim);
            }

            await _walletDbContext.SaveChangesAsync(cancellationToken);

            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o =>
                o.Network == vault.Network && o.Type == BlockchainOperationType.VESTING_CREATION &&
                o.TransactionId == vesting.Id, cancellationToken);

            if (operation != null && operation.State == OperationState.IN_PROGRESS)
                throw new VestingCreationIsInProgressException(vault.Id);

            operation ??= await _operationService.CreateVestingDeploymentTransaction(vault.Network, vesting.Id,
                vault.Address, _currentUserContext.UserId);

            try
            {
                var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                    new VestingDeploymentRequested(vesting.Id, _currentUserContext.UserId));
                await _eventPublisher.PublishAsync(e);
                return vesting.Id;
            }
            catch (Exception ex)
            {
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                throw;
            }
        }
    }
}