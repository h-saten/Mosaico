using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.KangaWallet.Commands.CreateTransaction;
using Mosaico.Application.KangaWallet.Commands.SaveTransaction;
using Mosaico.Application.KangaWallet.Queries.TokenEstimates;
using Mosaico.Authorization.Base;

namespace Mosaico.API.v1.Wallet.Controllers.Kanga
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/kanga")]
    [Route("api/v{version:apiVersion}/kanga")]
    [Route("api/KangaApiBuy")]
    public class KangaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _currentUserContext;

        public KangaController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            _mediator = mediator;
            _currentUserContext = currentUserContext;
        }
        
        [HttpPost("transaction")]
        [HttpPost("SaveTransaction")]
        [AllowAnonymous]
        // [ServiceFilter(typeof(KangaIpCheckFilter))]
        public async Task<Unit> SaveTransaction(SaveTransactionCommand request)
            => await _mediator.Send(request);

        [HttpPost("transaction/create")]
        [HttpPost("CreateTransaction")]
        public async Task<IActionResult> CreateTransactions(CreateTransactionCommand request)
        {
            request.UserId = _currentUserContext.UserId;
            var response = await _mediator.Send(request);
            
            return new SuccessResult(response);
        }

        [HttpGet("token/{token:guid}/estimates")]
        public async Task<IActionResult> TokenEstimates([FromRoute] Guid token)
        {
            
            var query = new TokenEstimatesQuery
            {
                TokenId = token,
                UserId = _currentUserContext.UserId
            };
            var response = await _mediator.Send(query);
            
            return new SuccessResult(response);
        }
    }
}