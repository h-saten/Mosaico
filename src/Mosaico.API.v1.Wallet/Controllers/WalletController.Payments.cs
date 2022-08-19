using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.Wallet.Queries.GetPaymentCurrencies;

namespace Mosaico.API.v1.Wallet.Controllers
{
    public partial class WalletController
    {
        [HttpGet("{network}/payment-currencies")]
        [ProducesResponseType(typeof(SuccessResult<List<PaymentCurrencyDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaymentCurrencies([FromRoute] string network)
        {
            var response = await _mediator.Send(new GetPaymentCurrenciesQuery
            {
                Network = network,
            });
            return new SuccessResult(response);
        }
    }
}