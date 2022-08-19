using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.TokenManagement.UpsertTokenDistribution;
using Mosaico.Application.Wallet.Commands.Vault.CreateVaultDeposit;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Queries.GetTokenDistribution;

namespace Mosaico.API.v1.Wallet.Controllers
{
    public partial class TokenController
    {
        [HttpGet("{id}/distribution")]
        [ProducesResponseType(typeof(SuccessResult<List<TokenDistributionDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenDistribution([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetTokenDistributionQuery
            {
                TokenId = id
            });
            return new SuccessResult(response);
        }
        
        [HttpPost("{id}/distribution")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpsertTokenDistribution([FromRoute] Guid id, [FromBody] UpsertTokenDistributionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TokenId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}