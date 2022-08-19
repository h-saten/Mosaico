using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.Airdrop.AddAirdropParticipants;
using Mosaico.Application.ProjectManagement.Commands.Airdrop.ClaimAirdrop;
using Mosaico.Application.ProjectManagement.Commands.Airdrop.CreateAirdrop;
using Mosaico.Application.ProjectManagement.Commands.Airdrop.DistributeAirdrop;
using Mosaico.Application.ProjectManagement.Commands.Airdrop.ImportAirdropParticipants;
using Mosaico.Application.ProjectManagement.Commands.Airdrop.StopAirdrop;
using Mosaico.Application.ProjectManagement.DTOs.Airdrop;
using Mosaico.Application.ProjectManagement.Queries.Airdrop.GetAirdrop;
using Mosaico.Application.ProjectManagement.Queries.Airdrop.GetAirdropParticipants;
using Mosaico.Application.ProjectManagement.Queries.Airdrop.GetProjectAirdrops;
using Mosaico.Authorization.Base;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/projects/{id}/airdrops")]
    [Route("api/v{version:apiVersion}/projects/{id}/airdrops")]
    public class AirdropController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _currentUserContext;
        
        public AirdropController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            _mediator = mediator;
            _currentUserContext = currentUserContext;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateAirdrop([FromRoute] Guid id, [FromBody] CreateAirdropCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.Name = command.Name.Trim();
            command.ProjectId = id;
            var airdropId = await _mediator.Send(command);
            return new SuccessResult(airdropId) { StatusCode = 201 };
        }
        
        [HttpDelete("{airdropId}")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> StopAirdrop([FromRoute] Guid id, [FromRoute] Guid airdropId)
        {
            await _mediator.Send(new StopAirdropCommand
            {
                AirdropId = airdropId,
                ProjectId = id
            });
            return new SuccessResult();
        }
        
        [HttpPost]
        [Route("/api/airdrops/{airdropId}")]
        [Route("/api/v{version:apiVersion}/airdrops/{airdropId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> ClaimAirdrop([FromRoute] Guid airdropId)
        {
            await _mediator.Send(new ClaimAirdropCommand
            {
                AirdropId = airdropId
            });
            return new SuccessResult();
        }
        
        [HttpPut("{airdropId}/participants")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> AddParticipants([FromRoute] Guid id, [FromRoute] Guid airdropId, [FromBody] AddAirdropParticipantsCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.AirdropId = airdropId;
            command.ProjectId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("{airdropId}/participants/import")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> Import([FromRoute] Guid id, [FromRoute] Guid airdropId, [FromForm] IFormFile file)
        {
            var fileContent = Array.Empty<byte>();
            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    fileContent = stream.ToArray();
                }
            }
            var response = await _mediator.Send(new ImportAirdropParticipantsCommand
            {
                AirdropId = airdropId,
                ProjectId = id,
                File = fileContent
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("{airdropId}/participants")]
        [ProducesResponseType(typeof(SuccessResult<List<AirdropParticipantDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> AddParticipants([FromRoute] Guid id, [FromRoute] Guid airdropId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            if (take <= 0) take = 10;
            if (skip < 0) skip = 0;
            var response = await _mediator.Send(new GetAirdropParticipantsQuery
            {
                AirdropId = airdropId,
                ProjectId = id,
                Skip = skip,
                Take = take
            });
            return new SuccessResult(response);
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<List<AirdropDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetAirdrops([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetProjectAirdropsQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }
        
        [HttpGet]
        [Route("/api/airdrops/{airdropId}")]
        [Route("/api/v{version:apiVersion}/airdrops/{airdropId}")]
        [ProducesResponseType(typeof(SuccessResult<List<AirdropDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAirdrop([FromRoute] string airdropId)
        {
            var response = await _mediator.Send(new GetAirdropQuery
            {
                Id = airdropId
            });
            return new SuccessResult(response);
        }
        
        [HttpPut("{airdropId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> Distribute([FromRoute] Guid id, [FromRoute] Guid airdropId)
        {
            await _mediator.Send(new DistributeAirdropCommand
            {
                AirdropId = airdropId,
                ProjectId = id
            });
            return new SuccessResult();
        }
        
        
    }
}