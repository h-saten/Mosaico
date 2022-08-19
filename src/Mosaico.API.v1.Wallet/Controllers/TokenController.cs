using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.ExternalExchange.DeleteExternalExchange;
using Mosaico.Application.Wallet.Commands.ExternalExchange.UpsertExternalExchange;
using Mosaico.Application.Wallet.Commands.TokenManagement.BurnToken;
using Mosaico.Application.Wallet.Commands.TokenManagement.CreateToken;
using Mosaico.Application.Wallet.Commands.TokenManagement.DeployToken;
using Mosaico.Application.Wallet.Commands.TokenManagement.EnableDeflation;
using Mosaico.Application.Wallet.Commands.TokenManagement.EnableStaking;
using Mosaico.Application.Wallet.Commands.TokenManagement.EnableVesting;
using Mosaico.Application.Wallet.Commands.TokenManagement.ImportToken;
using Mosaico.Application.Wallet.Commands.TokenManagement.MintToken;
using Mosaico.Application.Wallet.Commands.TokenManagement.PreValidateNewToken;
using Mosaico.Application.Wallet.Commands.TokenManagement.UpdateToken;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Permissions;
using Mosaico.Application.Wallet.Queries.Company.GetCompanyOwnedTokens;
using Mosaico.Application.Wallet.Queries.GetToken;
using Mosaico.Application.Wallet.Queries.ImportTokenDetails;
using Mosaico.Application.Wallet.Queries.TokenPermission;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/tokens")]
    [Route("api/v{version:apiVersion}/tokens")]
    public partial class TokenController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TokenController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResult<TokenDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetTokenQuery
            {
                Id = id
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("{id}/permissions")]
        [ProducesResponseType(typeof(SuccessResult<TokenPermissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenPermissions([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new TokenPermissionQuery
            {
                TokenId = id
            });
            return new SuccessResult(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<List<TokenDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyTokens([FromQuery] Guid companyId)
        {
            var response = await _mediator.Send(new GetCompanyOwnedTokensQuery
            {
                CompanyId = companyId
            });
            return new SuccessResult(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> CreateToken([FromBody] CreateTokenCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            var response = await _mediator.Send(command);
            return new SuccessResult(response){StatusCode = StatusCodes.Status201Created};
        }

        [HttpPost("import")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> ImportToken([FromBody] ImportTokenCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            var response = await _mediator.Send(command);
            return new SuccessResult(response){StatusCode = StatusCodes.Status201Created};
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdateToken([FromRoute] Guid id, [FromBody] UpdateTokenCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TokenId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("{id}/deployment")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeployToken([FromRoute] Guid id)
        {
            await _mediator.Send(new DeployTokenCommand
            {
                TokenId = id
            });
            return new SuccessResult();
        }
        
        [HttpPost("prevalidate")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> PrevalidateToken([FromBody] PreValidateNewTokenCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            var response = await _mediator.Send(command);
            return new SuccessResult(response){StatusCode = StatusCodes.Status201Created};
        }

        [HttpPost("{id}/exchange")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdateExchange([FromRoute] Guid id, [FromBody] UpsertExternalExchangeCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TokenId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpDelete("{id}/exchange/{exchangeId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdateExchange([FromRoute] Guid id, [FromRoute] Guid exchangeId)
        {
            await _mediator.Send(new DeleteExternalExchangeCommand
            {
                TokenId = id,
                ExternalExchangeId = exchangeId
            });
            return new SuccessResult();
        }

        [HttpPut("{id}/mint")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> MintAsync([FromRoute] Guid id, [FromBody] MintTokenCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TokenId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPut("{id}/burn")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> BurnAsync([FromRoute] Guid id, [FromBody] BurnTokenCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TokenId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpGet("{network}/{address}/details")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> ImportToken([FromRoute] string network, [FromRoute] string address)
        {
            var response = await _mediator.Send(new ImportTokenDetailsQuery
            {
                ContractAddress = address,
                Chain = network
            });
            return new SuccessResult(response);

        }

        [HttpPost("{id}/stakings")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EnableStaking([FromRoute] Guid id, [FromBody] EnableStakingCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TokenId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpPost("{id}/deflation")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EnableDeflation([FromRoute] Guid id, [FromBody] EnableDeflationCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TokenId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("{id}/vesting")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EnableVesting([FromRoute] Guid id, [FromBody] EnableVestingCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TokenId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}