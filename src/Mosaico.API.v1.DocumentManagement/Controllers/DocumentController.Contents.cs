using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.DocumentManagement.Commands.RemoveDocumentContent;
using System;
using System.Threading.Tasks;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    public partial class DocumentController
    {
        [HttpDelete("{id}/contents/{language}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> RemoveDocumentContent([FromRoute] Guid id, [FromRoute] string language)
        {
            var command = new RemoveProjectDocumentCommand { DocumentId = id, Language = language};

            await _mediator.Send(command);

            return new SuccessResult();
        }
    }
}
