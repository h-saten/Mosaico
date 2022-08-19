using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.NFT.CreateNFT;
using Mosaico.Application.Wallet.Commands.NFT.CreateNFTCollection;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Queries.NFT.GetNFT;
using Mosaico.Authorization.Base;
using Mosaico.Base;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/nfts")]
    [Route("api/v{version:apiVersion}/nfts")]
    public class NFTController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _userContext;

        public NFTController(IMediator mediator, ICurrentUserContext userContext)
        {
            _mediator = mediator;
            _userContext = userContext;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> CreateNFTCollection([FromBody] CreateNFTCollectionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            var response = await _mediator.Send(command);
            return new SuccessResult(response){StatusCode = 201};
        }
        
        [HttpPost("{id}/tokens")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> CreateNFT([FromRoute] Guid id, [FromBody] CreateNFTCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.NFTCollectionId = id;
            var response = await _mediator.Send(command);
            return new SuccessResult(response){StatusCode = 201};
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<PaginatedResult<NFTDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNFTs([FromQuery] GetNFTQuery query)
        {
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }
    }
}