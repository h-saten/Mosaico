using Mosaico.Base.Exceptions;

namespace Mosaico.Payments.Binance.Exceptions
{
    public class BinanceException : ExceptionBase
    {
        public BinanceException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public override string Code => "BINANCE_ERROR";
        public override int StatusCode { get; }
    }
}