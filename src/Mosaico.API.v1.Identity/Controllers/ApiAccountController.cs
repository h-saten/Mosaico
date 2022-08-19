using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Duende.IdentityServer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Identity;
using Mosaico.Application.Identity.Commands.ConfirmChangePassword;
using Mosaico.Application.Identity.Commands.ConfirmEmail;
using Mosaico.Application.Identity.Commands.CreateExternalUser;
using Mosaico.Application.Identity.Commands.CreateUser;
using Mosaico.Application.Identity.Commands.InitiatePasswordChange;
using Mosaico.Application.Identity.Commands.InitiatePasswordReset;
using Mosaico.Application.Identity.Commands.LoginUser;
using Mosaico.Application.Identity.Commands.LogoutUser;
using Mosaico.Application.Identity.Commands.ResetUserPassword;
using Mosaico.Application.Identity.Queries.GetEmailExistence;
using Mosaico.Application.Identity.Queries.GetPhoneNumberExistence;
using Mosaico.Application.Identity.Queries.GetReCaptchaSiteVerify;
using static Mosaico.API.Base.Constants.Permissions;

namespace Mosaico.API.v1.Identity.Controllers
{
    [Authorize]
    [Route("api/auth/[controller]/[action]")]
    [ApiController]
    public class ApiAccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IIndex<string, string> _urls;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiAccountController(IMediator mediator, IIndex<string, string> urls, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _urls = urls;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> AccountExist([FromQuery] string email)
        {
            var result = await _mediator.Send(new GetEmailExistenceQuery
            {
                Email = email
            });
            return new SuccessResult(result.Exist);
        }

        [HttpPost]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> RegisterConfirmed([FromBody] CreateUserCommand model)
        {
            model.IsConfirmed = true;
            var response = await _mediator.Send(model);
            return new SuccessResult(response) {StatusCode = StatusCodes.Status201Created};
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand model)
        {
            model.IsConfirmed = false;
            var response = await _mediator.Send(model);
            return new SuccessResult(response) {StatusCode = StatusCodes.Status201Created};
        }
        
        [HttpPost]
        [Authorize(AppServicesInternalActions)]
        public async Task<IActionResult> RegisterExternalUser([FromBody] CreateExternalUserCommand model)
        {
            var response = await _mediator.Send(model);
            return new SuccessResult(response)
            {
                StatusCode = StatusCodes.Status200OK,
                Data = response
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand model)
        {
            if (_urls.TryGetValue(Constants.UrlKeys.AfterLoginRedirectUrl, out var afterLoginUrl))
            {
                model.AfterLoginRedirectUrl = afterLoginUrl;
            }
            
            if (_httpContextAccessor?.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("User-Agent", out var agentInfo);
                var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                model.AgentInfo = agentInfo;
                model.IP = ip;
            }

            var response = await _mediator.Send(model);
            return new SuccessResult(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("User-Agent", out var agentInfo);
            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            
            await _mediator.Send(new ConfirmEmailCommand
            {
                Code = code,
                UserId = userId,
                AgentInfo = agentInfo,
                IP = ip
            });
            return new SuccessResult();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmailExistence([FromQuery] string email)
        {
            var result = await _mediator.Send(new GetEmailExistenceQuery
            {
                Email = email
            });

            return new SuccessResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RecaptchaVerification([FromQuery] string response)
        {
            var result = await _mediator.Send(new GetReCaptchaSiteverifyQuery
            {
                Response = response
            });

            return new SuccessResult(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CheckPhoneNumberExistence([FromQuery] string phoneNumber)
        {
            var result = await _mediator.Send(new GetPhoneNumberExistenceQuery
            {
                PhoneNumber = phoneNumber
            });

            return new SuccessResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] InitiatePasswordResetCommand model)
        {
            await _mediator.Send(model);
            return new SuccessResult();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetUserPasswordCommand model)
        {
            await _mediator.Send(model);
            return new SuccessResult();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] InitiatePasswordChangeCommand model)
        {
            await _mediator.Send(model);
            return new SuccessResult();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmChangePassword([FromBody] ConfirmPasswordChangeCommand model)
        {
            await _mediator.Send(model);
            return new SuccessResult();
        }

        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] LogoutUserCommand model)
        {
            var response = await _mediator.Send(model);
            return new SuccessResult(response);
        }
    }
}