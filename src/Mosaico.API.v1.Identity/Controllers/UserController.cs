using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Filters;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Identity.Commands.CompleteEvaluation;
using Mosaico.Application.Identity.Commands.ConfirmEmailChange;
using Mosaico.Application.Identity.Commands.DeactivateUser;
using Mosaico.Application.Identity.Commands.DeleteUser;
using Mosaico.Application.Identity.Commands.GenerateSmsConfirmationCode;
using Mosaico.Application.Identity.Commands.InitiateEmailChange;
using Mosaico.Application.Identity.Commands.InitKycVerification;
using Mosaico.Application.Identity.Commands.ReportStolenAccount;
using Mosaico.Application.Identity.Commands.RestoreUser;
using Mosaico.Application.Identity.Commands.SetPhoneNumber;
using Mosaico.Application.Identity.Commands.SetUserLanguage;
using Mosaico.Application.Identity.Commands.UpdatePhoneNumber;
using Mosaico.Application.Identity.Commands.UpdateUser;
using Mosaico.Application.Identity.Commands.VerifyPhoneNumber;
using Mosaico.Application.Identity.Queries.FindUser;
using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Application.Identity.Queries.GetUserDeletionRequests;
using Mosaico.Application.Identity.Queries.GetUserKycStatus;
using Mosaico.Application.Identity.Queries.GetUserProfilePermissions;
using Mosaico.Application.Identity.Queries.GetUsers;
using Mosaico.Application.Identity.Queries.GetUsersById;
using Mosaico.Application.Identity.Queries.GetUsersByName;
using Mosaico.Authorization.Base;
using Serilog;
using Serilog.Core;

namespace Mosaico.API.v1.Identity.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/users")]
    [Route("api/v{version:apiVersion}/users")]
    [Produces("application/json")]
    [APIExceptionFilter]
    public partial class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _currentUserContext;

        public UserController(IMediator mediator, ICurrentUserContext currentUserContext, ILogger logger = null)
        {
            _mediator = mediator;
            _currentUserContext = currentUserContext;            
        }

        [HttpGet("{id}")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUserQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidParameterException(nameof(id), id);
            var user = await _mediator.Send(new GetUserQuery {Id = id});
            return new SuccessResult(user);
        }

        [HttpGet("select")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUserQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsersById([FromQuery(Name = "Ids")] string[] ids)
        {
            if (ids.Length < 1) throw new InvalidParameterException(nameof(ids));
            var user = await _mediator.Send(new GetUsersByIdQuery {UsersId = ids.ToList()});
            return new SuccessResult(user);
        }
        
        [HttpPost("select")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUserQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsersByIdPost([FromBody] List<string> ids)
        {
            if (ids == null || ids.Count < 1) throw new InvalidParameterException(nameof(ids));
            var user = await _mediator.Send(new GetUsersByIdQuery {UsersId = ids});
            return new SuccessResult(user);
        }
        
        [HttpGet("{id}/profile/permissions")]
        [HttpGet("self/profile/permissions")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUserProfilePermissionsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserProfilePermissions([FromRoute] string id = null)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = _currentUserContext.UserId;
            }
            var user = await _mediator.Send(new GetUserProfilePermissionsQuery {UserId = id});
            return new SuccessResult(user);
        }

        [HttpGet]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUserQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery cmd)
        {
            var user = await _mediator.Send(cmd);
            return new SuccessResult(user);
        }
        
        [HttpGet("{userName}/user")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUsersByNameQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsersByName([FromQuery] GetUsersByNameQuery userName)
        {
            if (userName == null) throw new InvalidParameterException(nameof(userName));
            var user = await _mediator.Send(userName);
            return new SuccessResult(user);
        }
        
        [HttpGet("find")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUsersByNameQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string findBy, [FromQuery] string value)
        {
            if (String.IsNullOrWhiteSpace(findBy)) throw new InvalidParameterException(nameof(findBy));
            if (String.IsNullOrWhiteSpace(value)) throw new InvalidParameterException(nameof(value));
            var user = await _mediator.Send(new FindUserQuery { FindBy = findBy, Value = value });
            return new SuccessResult(user);
        }
        
        [HttpPost("deactivate")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> Deactivate([FromBody] DeactivateUserCommand model)
        {
            await _mediator.Send(model);
            return new SuccessResult();
        }

        [HttpGet("deletionrequests")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUserDeletionRequestsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDelitionRequests([FromQuery] GetUserDeletionRequestsQuery command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            var deletionRequests = await _mediator.Send(command);
            return new SuccessResult(deletionRequests);
        }

        // [HttpPost("verification")]
        // [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        // [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        // public async Task<IActionResult> InitiateUserVerification([FromBody] InitiateUserVerificationCommand command)
        // {
        //     if (command == null) throw new InvalidParameterException(nameof(command));
        //
        //     await _mediator.Send(command);
        //     return new SuccessResult(true);
        // }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.Id = id;
            await _mediator.Send(command);
            return new SuccessResult(true);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> DeleteUser([FromRoute] string id, [FromBody] DeleteUserCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.Id = id;
            await _mediator.Send(command);

            return new SuccessResult(true);
        }

        [HttpPost("{id}/restore")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]  
        public async Task<IActionResult> RestoreAccount([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var userId)) throw new InvalidParameterException(nameof(id));
            await _mediator.Send(new RestoreUserCommand() { Id = id });

            return new SuccessResult(true);
        }
        [HttpPost("{id}/report-stolen")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> ReportStolenAccount([FromRoute] string id, string code)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var userId)) throw new InvalidParameterException(nameof(id));
            await _mediator.Send(new ReportStolenAccountCommand() { Id = id, Code = code }) ;

            return new SuccessResult(true);
        }

        [HttpPost("{id}/phone-number-confirmation/generate-code")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> GeneratePhoneNumberConfirmationCode([FromRoute] string id, [FromBody] GenerateSmsConfirmationCodeCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = id;
            await _mediator.Send(command);
            return new SuccessResult(true);
        }

        [HttpPost("{id}/phone-number-confirmation/verify")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> VerifyPhoneNumber([FromRoute] string id, [FromBody] VerifyPhoneNumberCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = id;
            await _mediator.Send(command);
            return new SuccessResult(true);
        }

        [HttpPost("{id}/email")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> InitiateEmailChange([FromRoute] string id, [FromBody] InitiateEmailChangeCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = id;
            await _mediator.Send(command);
            return new SuccessResult(true);
        }


        [HttpPut("{id}/email")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeEmail([FromRoute] string id, [FromBody] ConfirmEmailChangeCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpPut("self/language")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> ChangeLanguage([FromBody] SetUserLanguageCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = _currentUserContext.UserId;
            await _mediator.Send(command);
            return new SuccessResult(true);
        }

        [HttpPut("self/phone-number")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> ChangePhoneAsync([FromBody] UpdatePhoneNumberCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = _currentUserContext.UserId;
            if (string.IsNullOrWhiteSpace(command.Code))
            {
                await _mediator.Send(new SetUnconfirmedPhoneNumberCommand
                {
                    PhoneNumber = command.PhoneNumber,
                    UserId = command.UserId
                });
            }
            else
            {
                await _mediator.Send(command);
            }

            return new SuccessResult();
        }
        
        [HttpPost("self/evaluation")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> CompleteEvaluation([FromBody] CompleteEvaluationCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = _currentUserContext.UserId;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("self/kyc")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> InitiateKyc([FromBody] InitKycVerificationCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = _currentUserContext.UserId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response) {StatusCode = StatusCodes.Status201Created};
        }
        
        [HttpGet("self/kyc")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> GetKycStatus()
        {
            var response = await _mediator.Send(new GetUserKycStatusQuery
            {
                UserId = _currentUserContext.UserId
            });
            return new SuccessResult(response);
        }
    }
}