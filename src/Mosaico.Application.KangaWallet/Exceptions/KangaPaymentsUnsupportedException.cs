using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.KangaWallet.Exceptions
{
    public class KangaPaymentsUnsupportedException : ExceptionBase
    {
        public KangaPaymentsUnsupportedException() : base("Kanga payment method is not supported.")
        {
        }

        public override string Code => KangaExchange.SDK.Constants.ErrorCodes.UnsupportedPaymentMethod;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}