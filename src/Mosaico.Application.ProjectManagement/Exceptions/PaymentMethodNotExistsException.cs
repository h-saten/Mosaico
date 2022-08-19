using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class PaymentMethodNotExistsException : ExceptionBase
    {
        public List<string> Keys { get; set; } = new();
        
        public PaymentMethodNotExistsException(string key) : base($"Payment method '{key}' not exists.")
        {
            Keys.Add(key);
        }
        
        public PaymentMethodNotExistsException(List<string> keys) : base($"Payment methods '{string.Join(",", keys.ToArray())}' not exists.")
        {
            Keys.AddRange(keys);
        }

        public override string Code => Constants.ErrorCodes.VestingWalletWasNotDeployed;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}