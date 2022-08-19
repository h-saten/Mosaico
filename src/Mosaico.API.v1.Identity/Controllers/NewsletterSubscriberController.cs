using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Identity.Commands.SubscribeToNewsletter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.v1.Identity.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class NewsletterSubscriberController: ControllerBase
    {
        private readonly IMediator _mediator;

        public NewsletterSubscriberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> SubscribeToNewsletter([FromBody] SubscribeToNewsletterCommand command)
        {
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}
