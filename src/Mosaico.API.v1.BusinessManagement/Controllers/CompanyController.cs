using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.BusinessManagement.Commands.AcceptInvitation;
using Mosaico.Application.BusinessManagement.Commands.ApproveVerificationRequest;
using Mosaico.Application.BusinessManagement.Commands.CreateCompany;
using Mosaico.Application.BusinessManagement.Commands.CreateCompanyTeamMember;
using Mosaico.Application.BusinessManagement.Commands.CreateVerification;
using Mosaico.Application.BusinessManagement.Commands.DeleteCompanyTeamMember;
using Mosaico.Application.BusinessManagement.Commands.LeaveCompany;
using Mosaico.Application.BusinessManagement.Commands.ResendInvitation;
using Mosaico.Application.BusinessManagement.Commands.Subscribe;
using Mosaico.Application.BusinessManagement.Commands.Unsubscribe;
using Mosaico.Application.BusinessManagement.Commands.UpdateCompany;
using Mosaico.Application.BusinessManagement.Commands.UpdateInvitation;
using Mosaico.Application.BusinessManagement.Commands.UpdateSocialMediaLinks;
using Mosaico.Application.BusinessManagement.Permissions;
using Mosaico.Application.BusinessManagement.Queries.CompanyPermissions;
using Mosaico.Application.BusinessManagement.Queries.GetAllVerifications;
using Mosaico.Application.BusinessManagement.Queries.GetCompanies;
using Mosaico.Application.BusinessManagement.Queries.GetCompaniesPublicly;
using Mosaico.Application.BusinessManagement.Queries.GetCompany;
using Mosaico.Application.BusinessManagement.Queries.GetCompanySocialLinks;
using Mosaico.Application.BusinessManagement.Queries.GetCompanyTeamMembers;
using Mosaico.Application.BusinessManagement.Queries.GetVerification;
using Mosaico.Authorization.Base;

namespace Mosaico.API.v1.BusinessManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/dao")]
    [Route("api/v{version:apiVersion}/dao")]
    public partial class CompanyController : ControllerBase
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            _mediator = mediator;
            _currentUserContext = currentUserContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetCompaniesQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetCompanies([FromQuery] GetCompaniesQuery query)
        {
            if (query == null) throw new InvalidParameterException(nameof(query));
            if (string.IsNullOrWhiteSpace(query.UserId)) query.UserId = _currentUserContext.UserId;
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }

        [HttpGet("public-list")]
        [ProducesResponseType(typeof(SuccessResult<GetCompaniesQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCompaniesListPublicly([FromQuery] GetCompaniesPubliclyQuery query)
        {
            if (query == null) throw new InvalidParameterException(nameof(query));
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResult<GetCompanyQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompany([FromRoute] string id)
        {
            var response = await _mediator.Send(new GetCompanyQuery {UniqueIdentifier = id});
            return new SuccessResult(response);
        }

        /*
         * <summary>
         *  Create company
         * </summary>
         * <param name="dto">Parameter of type CreateCompanyCommand</param>
         * <returns>ID of the new company (GUID)</returns>
         */
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));

            var id = await _mediator.Send(command);

            return new SuccessResult(id) {StatusCode = 201};
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateCompany([FromRoute] string id, [FromBody] UpdateCompanyCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var companyId))
                throw new InvalidParameterException(nameof(id));

            command.CompanyId = companyId;
            await _mediator.Send(command);

            return new SuccessResult();
        }

        [HttpGet("{id}/permissions")]
        [ProducesResponseType(typeof(SuccessResult<CompanyPermissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyPermissions([FromRoute] string id)
        {
            var permissions = await _mediator.Send(new CompanyPermissionQuery
            {
                UniqueIdentifier = id,
                UserId = _currentUserContext.IsAuthenticated ? _currentUserContext.UserId : string.Empty
            });

            return new SuccessResult(permissions);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> LeaveCompany([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var companyId))
                throw new InvalidParameterException(nameof(id));

            await _mediator.Send(new LeaveCompanyCommand
            {
                CompanyId = companyId
            });
            return new SuccessResult(true);
        }

        #region Company Verification

        [HttpGet("{id}/verification")]
        [ProducesResponseType(typeof(SuccessResult<GetVerificationQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVerification([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var companyId))
                throw new InvalidParameterException(nameof(id));
            var response = await _mediator.Send(new GetVerificationQuery {CompanyId = companyId});
            return new SuccessResult(response);
        }

        [HttpGet("verification")]
        [ProducesResponseType(typeof(SuccessResult<GetVerificationQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllVerifications([FromQuery] GetAllVerificationsQuery query)
        {
            if (query == null) throw new InvalidParameterException(nameof(query));
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }

        [HttpPost("{id}/verification")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateVerification([FromRoute] string id,
            [FromBody] CreateVerificationCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var companyId))
                throw new InvalidParameterException(nameof(id));

            command.CompanyId = companyId;
            await _mediator.Send(command);

            return new SuccessResult();
        }

        [HttpDelete("{id}/verification")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "tokenizer.globaladmin")]
        public async Task<IActionResult> ApproveVerificationRequest([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var companyId))
                throw new InvalidParameterException(nameof(id));

            await _mediator.Send(new ApproveVerificationRequestCommand
            {
                CompanyId = companyId
            });
            return new SuccessResult(true);
        }

        #endregion


        #region Company Team Member

        [HttpPut("invitation")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> AcceptCompanyInvitation([FromBody] AcceptInvitationCommand command)
        {
            var response = await _mediator.Send(command);
            return new SuccessResult(response) {StatusCode = 201};
        }

        [HttpPut("{id}/members/{invitationId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateTeamMember([FromRoute] Guid id, [FromRoute] Guid invitationId,
            [FromBody] UpdateTeamMemberCommand command)
        {
            command.Id = invitationId;
            command.CompanyId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpPost("{id}/members/{invitationId}/resend")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> ResendInvitation([FromRoute] Guid id, [FromRoute] Guid invitationId)
        {
            await _mediator.Send(new ResendInvitationCommand
            {
                Id = invitationId,
                CompanyId = id
            });
            return new SuccessResult(true);
        }

        [HttpPost("{id}/members")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateUpdateCompanyTeamMember([FromRoute] Guid id,
            [FromBody] CreateTeamMemberCommand command)
        {
            if (command == null)
                throw new InvalidParameterException(nameof(command));

            command.CompanyId = id;

            var memberId = await _mediator.Send(command);
            return new SuccessResult(memberId) {StatusCode = 201};
        }

        [HttpDelete("{id}/members/{teamMemberId}")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteCompanyTeamMember([FromRoute] Guid id, [FromRoute] Guid teamMemberId)
        {
            await _mediator.Send(new DeleteCompanyTeamMemberCommand
            {
                Id = teamMemberId,
                CompanyId = id
            });
            return new SuccessResult(true);
        }

        [HttpGet("{id}/members")]
        [ProducesResponseType(typeof(SuccessResult<GetCompanyQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetTeamMembers([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetCompanyTeamMembersQuery
            {
                CompanyId = id
            });
            return new SuccessResult(response);
        }

        #endregion

        #region subscription

        [HttpPost("{id}/newsletter")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Subscribe([FromRoute] Guid id, [FromBody] SubscribeCommand command)
        {
            command ??= new SubscribeCommand();

            command.CompanyId = id;
            if (_currentUserContext.IsAuthenticated && string.IsNullOrWhiteSpace(command.UserId))
                command.UserId = _currentUserContext.UserId;

            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }

        [HttpDelete("{id}/newsletter")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Subscribe([FromRoute] Guid id, [FromQuery] string email = null,
            [FromQuery] string code = null)
        {
            var command = new UnsubscribeCommand
            {
                Email = email,
                CompanyId = id,
                Code = code
            };

            if (_currentUserContext.IsAuthenticated && string.IsNullOrWhiteSpace(command.UserId))
                command.UserId = _currentUserContext.UserId;

            await _mediator.Send(command);
            return new SuccessResult();
        }

        #endregion

        #region Company Social Links

        [HttpPut("{id}/social-media")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateSocialMediaLinks([FromRoute] Guid id,
            [FromBody] UpdateSocialMediaLinksCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));

            command.CompanyId = id;
            await _mediator.Send(command);

            return new SuccessResult();
        }

        [HttpGet("{id}/social-media")]
        [ProducesResponseType(typeof(SuccessResult<GetCompanyQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetSocialMediaLinks([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetCompanySocialLinksQuery
            {
                CompanyId = id
            });
            return new SuccessResult(response);
        }

        #endregion
    }
}