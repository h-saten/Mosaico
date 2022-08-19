using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Core.Commands.SendContactForm;

namespace Mosaico.API.v1.Core.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("api/contact")]
    [Route("api/v{version:apiVersion}/contact")]
    
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> SendMessage([FromBody] SendContactFormCommand command)
        {

            if (command == null)
                throw new InvalidParameterException(nameof(command));

            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
    }
}