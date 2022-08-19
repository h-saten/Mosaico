using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.DocumentManagement.Commands.Staking.UploadStakingTerms;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/staking/{id}/terms")]
    [Route("api/v{version:apiVersion}/staking/{id}/terms")]
    public class StakingDocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StakingDocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UploadStakingTerms([FromRoute] Guid id, [FromQuery] Guid tokenId, [FromForm] IFormFile file, [FromQuery] string language = Mosaico.Base.Constants.Languages.English)
        {
            var command = new UploadStakingTermsCommand
            {
                StakingPairId = id,
                TokenId = tokenId,
                Language = language
            };
            
            if (file != null && file.Length > 0)
            {
                command.FileName = file.FileName;
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    command.Content = stream.ToArray();
                }
            }

            var fileId = await _mediator.Send(command);
            return new SuccessResult(fileId) { StatusCode = 201 };
        }
    }
}