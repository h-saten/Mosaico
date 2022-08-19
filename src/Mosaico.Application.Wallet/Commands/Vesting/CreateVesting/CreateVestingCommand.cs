using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Vesting.CreateVesting
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class CreateVestingCommand : IRequest<Guid>
    {
        public Guid TokenId { get; set; }
        public string Name { get; set; }
        public int NumberOfDays { get; set; }
        public decimal ImmediatePay { get; set; }
        public int AmountOfClaims { get; set; }
        public decimal TokenAmount { get; set; }
        public string WalletAddress { get; set; }
        public DateTimeOffset? StartsAt { get; set; }
    }
}