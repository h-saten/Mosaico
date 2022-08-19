using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.ExportSubscribers;
using Mosaico.Application.ProjectManagement.Commands.SubscribeToNewsletter;
using Mosaico.Application.ProjectManagement.Commands.UnsubscribeFromNewsletter;
using Mosaico.Application.ProjectManagement.Commands.UpsertProjectArticles;
using Mosaico.Application.ProjectManagement.Queries.GetArticles;
using Mosaico.Application.ProjectManagement.Queries.GetProjectSubscribers;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        [HttpPost("{id}/newsletter")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> SubscribeToNewsletter([FromRoute] Guid id, [FromBody]SubscribeToNewsletterCommand command)
        {
            command ??= new SubscribeToNewsletterCommand();
            command.ProjectId = id;
            if (_currentUserContext.IsAuthenticated)
            {
                command.UserId = _currentUserContext.UserId;
            }
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpDelete("{id}/newsletter")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> UnsubscribeFromNewsletter([FromRoute] Guid id, [FromQuery]string email = null, [FromQuery] string code = null)
        {
            var command = new UnsubscribeFromNewsletterCommand
            {
                ProjectId = id,
                Email = email,
                Code = code
            };
            command.ProjectId = id;
            if (_currentUserContext.IsAuthenticated)
            {
                command.UserId = _currentUserContext.UserId;
            }
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpGet("{id}/subscribers")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectSubscribersQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetProjectSubscribers([FromRoute] Guid id, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            if (skip < 0) skip = 0;
            if (take <= 0 || take > 100) take = 10;
            var response = await _mediator.Send(new GetProjectSubscribersQuery
            {
                Skip = skip,
                Take = take,
                ProjectId = id
            });
            return new SuccessResult(response);
        }
        
        [HttpPost("{id}/subscribers/export")]
        [Authorize]
        public async Task<IActionResult> ExportSubscribers([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new ExportSubscribersCommand
            {
                ProjectId = id
            });
            Response.Headers.Add("filename", response.Filename);
            var file = File(response.File, response.ContentType, response.Filename);
            return file;
        }

    }
}