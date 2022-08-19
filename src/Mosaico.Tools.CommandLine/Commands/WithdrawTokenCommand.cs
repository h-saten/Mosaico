using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;

namespace Mosaico.Tools.CommandLine.Commands
{
    // withdraw-token -token=MIC -userId=c6566c22-70d5-46a8-95fb-bc6828207c58 -amount=565 -recipient=0x59186DeA2CC06C04676A14f6a0C77914dd9594fE
    [Command("withdraw-token")]
    public class WithdrawTokenCommand : CommandBase
    {
        private readonly IWalletDbContext _context;
        private readonly IUserWalletService _userWalletService;
        private string _tokenTicker;
        private string _userId;
        private string _recipient;
        private decimal _amount;

        public WithdrawTokenCommand(IWalletDbContext context, IUserWalletService userWalletService)
        {
            _context = context;
            _userWalletService = userWalletService;
            SetOption("-token", "token ticker", s => _tokenTicker = s);
            SetOption("-userId", "user id", s => _userId = s);
            SetOption("-amount", "amount of tokens", s => _amount = decimal.Parse(s));
            SetOption("-recipient", "recipient", s => _recipient = s);
        }

        public override async Task Execute()
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Symbol == _tokenTicker);
            if(token == null) throw new TokenNotFoundException(_tokenTicker);
            var userWalelt = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == _userId && w.Network == token.Network);
            await _userWalletService.TransferTokenAsync(userWalelt, token.Address, _recipient, _amount);
        }
    }
}