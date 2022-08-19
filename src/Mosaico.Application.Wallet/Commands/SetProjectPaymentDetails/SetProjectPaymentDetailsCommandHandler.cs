using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Commands.SetProjectPaymentDetails
{
    public class SetProjectPaymentDetailsCommandHandler : IRequestHandler<SetProjectPaymentDetailsCommand>
    {
        private readonly IWalletDbContext _context;

        public SetProjectPaymentDetailsCommandHandler(IWalletDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetProjectPaymentDetailsCommand request, CancellationToken cancellationToken)
        {
            var paymentDetails =
                await _context.ProjectBankPaymentDetails.FirstOrDefaultAsync(t => t.ProjectId == request.ProjectId, cancellationToken);
            if (string.IsNullOrWhiteSpace(request.Key))
            {
                request.Key = "DEFAULT";
            }
            
            if(paymentDetails == null)
            {
                paymentDetails = new ProjectBankPaymentDetails
                {
                    Account = request.Account,
                    Key = request.Key,
                    Swift = request.Swift,
                    AccountAddress = request.AccountAddress,
                    BankName = request.BankName,
                    ProjectId = request.ProjectId
                };
                _context.ProjectBankPaymentDetails.Add(paymentDetails);
            }
            else
            {
                paymentDetails.AccountAddress = request.AccountAddress;
                paymentDetails.Swift = request.Swift;
                paymentDetails.Key = request.Key;
                paymentDetails.Account = request.Account;
                paymentDetails.BankName = request.BankName;
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}