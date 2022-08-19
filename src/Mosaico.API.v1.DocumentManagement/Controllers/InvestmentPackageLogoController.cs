using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadInvestmentPackageLogo;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/pages/{id}/packages/{packageId}/logo")]
    [Route("api/v{version:apiVersion}/pages/{id}/packages/{packageId}/logo")]
    public class InvestmentPackageLogoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvestmentPackageLogoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UploadInvestmentPackageLogo([FromRoute] Guid id, [FromRoute] Guid packageId, [FromForm] IFormFile file)
        {
            var command = new UploadInvestmentPackageLogoCommand
            {
                PageId = id,
                InvestmentPackageId = packageId
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