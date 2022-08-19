using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.Vault.CreateVaultDeposit;
using Mosaico.Application.Wallet.Commands.Vault.DeployVault;
using Mosaico.Application.Wallet.Commands.Vault.SendVaultTokens;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/vaults")]
    [Route("api/v{version:apiVersion}/vaults")]
    [Authorize]
    public class VaultController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VaultController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> CreateVault([FromBody] DeployVaultCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            await _mediator.Send(command);
            return new SuccessResult(true);
        }
        
        [HttpPost("{id}/deposit")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeployDeposit([FromRoute] Guid id, [FromBody] CreateVaultDepositCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.VaultId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("{id}/transfer")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> SendVaultResources([FromRoute] Guid id, [FromBody] SendVaultTokensCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.VaultId = id;
            await _mediator.Send(command);
            return new SuccessResult(true);
        }
    }
}