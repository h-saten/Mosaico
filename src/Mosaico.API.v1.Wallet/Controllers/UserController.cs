using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Queries.Operations.GetUserOperations;
using Mosaico.Authorization.Base;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/users")]
    [Route("api/v{version:apiVersion}/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _currentUser;

        public UserController(IMediator mediator, ICurrentUserContext currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }
        
        [HttpGet("operations")]
        [ProducesResponseType(typeof(SuccessResult<GetUserOperationsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetUserOperations([FromQuery] GetUserOperationsQuery query)
        {
            query ??= new GetUserOperationsQuery();
            if (query.Skip < 0) query.Skip = 0;
            if (query.Take <= 0) query.Take = 10;
            query.UserId = _currentUser.UserId;
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }
    }
}