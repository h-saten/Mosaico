using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class PaymentDetailsNotFoundException : ExceptionBase
    {
        public PaymentDetailsNotFoundException(Guid projectId) : base($"payment details for project {projectId} not found")
        {
        }

        public override string Code => "PAYMENT_DETAILS_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}