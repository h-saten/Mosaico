using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.GetPaymentDetails
{
    public class GetPaymentDetailsQueryHandler : IRequestHandler<GetPaymentDetailsQuery, GetPaymentDetailsQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;

        public GetPaymentDetailsQueryHandler(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<GetPaymentDetailsQueryResponse> Handle(GetPaymentDetailsQuery request, CancellationToken cancellationToken)
        {
            var paymentDetails =
                await _walletDbContext.ProjectBankPaymentDetails.FirstOrDefaultAsync(t =>
                    t.ProjectId == request.ProjectId, cancellationToken);
            if(paymentDetails == null)
            {
                return null;
            }

            return new GetPaymentDetailsQueryResponse
            {
                Account = paymentDetails.Account,
                Key = paymentDetails.Key,
                Swift = paymentDetails.Swift,
                AccountAddress = paymentDetails.AccountAddress,
                BankName = paymentDetails.BankName
            };
        }
    }
}