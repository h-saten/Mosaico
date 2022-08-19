using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Payments.RampNetwork.Abstractions;
using Mosaico.Payments.Transak.Abstractions;

namespace Mosaico.Application.Wallet.Queries.GetTransactionStatus
{
    public class GetTransactionStatusQueryHandler : IRequestHandler<GetTransactionStatusQuery, GetTransactionStatusQueryResponse>
    {
        private readonly ITransakClient _transakClient;
        private readonly IRampClient _rampClient;
        private readonly ICurrentUserContext _currentUser;
        private readonly IWalletDbContext _walletDbContext;

        public GetTransactionStatusQueryHandler(ITransakClient transakClient, IRampClient rampClient, ICurrentUserContext currentUser, IWalletDbContext walletDbContext)
        {
            _transakClient = transakClient;
            _rampClient = rampClient;
            _currentUser = currentUser;
            _walletDbContext = walletDbContext;
        }

        public async Task<GetTransactionStatusQueryResponse> Handle(GetTransactionStatusQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (transaction == null)
            {
                throw new TransactionNotFoundException(request.Id.ToString());
            }
            if(transaction.UserId != _currentUser.UserId && !_currentUser.IsGlobalAdmin)
            {
                throw new ForbiddenException();
            }

            if (transaction.PaymentProcessor == Payments.Transak.Constants.PaymentProcessorName)
            {
                var externalTransaction = await _transakClient.GetOrderDetailsAsync(transaction.CorrelationId);
                if(externalTransaction != null)
                {
                    var status = string.Empty;
                    switch (externalTransaction.Response?.Status)
                    {
                        case "PAYMENT_DONE_MARKED_BY_USER":
                        case "PAYMENT_EXECUTED":
                        case "PROCESSING":
                        case "PENDING_DELIVERY_FROM_TRANSAK":
                        case "ON_HOLD_PENDING_DELIVERY_FROM_TRANSAK":
                            status = Domain.Wallet.Constants.TransactionStatuses.InProgress;
                            break;
                        case "FAILED":
                            status = Domain.Wallet.Constants.TransactionStatuses.Failed;
                            break;
                        case "CANCELLED":
                        case "EXPIRED":
                        case "REFUNDED":
                            status = Domain.Wallet.Constants.TransactionStatuses.Canceled;
                            break;
                        case "COMPLETED":
                            status = Domain.Wallet.Constants.TransactionStatuses.Confirmed;
                            break;
                        case "AWAITING_PAYMENT_FROM_USER":
                            status = Domain.Wallet.Constants.TransactionStatuses.Pending;
                            break;
                        default:
                            status = "INVALID";
                            break;
                    }

                    return new GetTransactionStatusQueryResponse
                    {
                        Status = status
                    };
                }
            }
            else if(transaction.PaymentProcessor == Payments.RampNetwork.Constants.PaymentProcessorName)
            {
                var externalTransaction = await _rampClient.GetPurchaseAsync(transaction.CorrelationId, transaction.ExtraData);
                if(externalTransaction != null)
                {
                    var status = string.Empty;
                    switch (externalTransaction.Status)
                    {
                        case "PAYMENT_IN_PROGRESS":
                        case "PAYMENT_EXECUTED":
                        case "FIAT_SENT":
                        case "FIAT_RECEIVED":
                        case "RELEASING":
                            status = Domain.Wallet.Constants.TransactionStatuses.InProgress;
                            break;
                        case "PAYMENT_FAILED":
                            status = Domain.Wallet.Constants.TransactionStatuses.Failed;
                            break;
                        case "CANCELLED":
                        case "EXPIRED":
                            status = Domain.Wallet.Constants.TransactionStatuses.Canceled;
                            break;
                        case "INITIALIZED":
                        case "PAYMENT_STARTED":
                            status = Domain.Wallet.Constants.TransactionStatuses.Pending;
                            break;
                        default:
                            status = "INVALID";
                            break;
                    }

                    return new GetTransactionStatusQueryResponse
                    {
                        Status = status
                    };
                }
            }
            else
            {
                return new GetTransactionStatusQueryResponse
                {
                    Status = transaction.Status.Key
                };
            }
            return new GetTransactionStatusQueryResponse
            {
                Status = "INVALID"
            };
        }
    }
}