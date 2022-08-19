using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.KangaWallet.Queries.KangaUserProfile;
using Mosaico.Application.KangaWallet.Queries.UserKangaWalletBalance;
using Mosaico.Authorization.Base;

namespace Mosaico.API.v1.Wallet.Controllers.Kanga
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/kanga")]
    [Route("api/v{version:apiVersion}/kanga")]
    [Route("api/KangaUser")]
    public class KangaUserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _currentUserContext;

        public KangaUserController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            _mediator = mediator;
            _currentUserContext = currentUserContext;
        }

        [HttpGet("UserProfile")]
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserProfileAsync([FromRoute] string userId = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = _currentUserContext.UserId.ToString();
            }

            var response = await _mediator.Send(new KangaUserProfileQuery
            {
                UserId = userId
            });
            
            return new SuccessResult(response);
        }

        [HttpGet("account/balance")]
        [Authorize]
        public async Task<IActionResult> UserKangaAccountBalance()
        {
            var response = await _mediator.Send(new UserKangaWalletBalanceQuery
            {
                UserId = _currentUserContext.UserId
            });
            
            return new SuccessResult(response);
        }
    }
}