using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.Wallet.WalletCurrencySend;
using Mosaico.Application.Wallet.Commands.Wallet.WalletTokenSend;
using Mosaico.Application.Wallet.Queries.GetPaymentCurrencyBalance;
using Mosaico.Application.Wallet.Queries.GetWalletBalanceHistory;
using Mosaico.Application.Wallet.Queries.Vesting.GetWalletVestings;
using Mosaico.Application.Wallet.Queries.WalletStagePurchaseSummary;
using Mosaico.Application.Wallet.Queries.WalletToken;
using Mosaico.Application.Wallet.Queries.WalletTokens;
using Mosaico.Authorization.Base;
using Constants = Mosaico.Blockchain.Base.Constants;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/wallets")]
    [Route("api/v{version:apiVersion}/wallets")]
    [Authorize]
    public partial class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _currentUser;
        
        public WalletController(IMediator mediator, ICurrentUserContext currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("{userId}/{network}/tokens")]
        [ProducesResponseType(typeof(SuccessResult<WalletTokensQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWalletTokens([FromRoute] string userId, [FromRoute] string network = Constants.BlockchainNetworks.Ethereum)
        {
            var response = await _mediator.Send(new WalletTokensQuery
            {
                Network = network,
                UserId = userId
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("token/{token:guid}/balance")]
        [ProducesResponseType(typeof(SuccessResult<WalletTokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetWalletTokenBalance([FromRoute] Guid token)
        {
            var response = await _mediator.Send(new WalletTokenQuery
            {
                TokenId = token,
                UserId = _currentUser.UserId,
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("{userId}/{network}/vestings")]
        [ProducesResponseType(typeof(SuccessResult<GetWalletVestingsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWalletVestings([FromRoute] string userId, [FromRoute] string network = Constants.BlockchainNetworks.Ethereum)
        {
            var response = await _mediator.Send(new GetWalletVestingsQuery
            {
                Network = network,
                UserId = userId
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("{userId}/{network}/stage/{stage}")]
        [ProducesResponseType(typeof(SuccessResult<WalletStagePurchaseSummaryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWalletStageSummary([FromRoute] string userId, [FromRoute] string stage, [FromRoute] string network = Constants.BlockchainNetworks.Ethereum)
        {
            var response = await _mediator.Send(new WalletStagePurchaseSummaryQuery
            {
                Network = network,
                UserId = userId,
                StageId = stage
            });
            return new SuccessResult(response);
        }
        
        
        [HttpPost("{wallet}/{network}/tokens/transaction")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TransferTokens([FromRoute] string wallet, [FromRoute] string network, [FromBody] WalletSendTokenCommand command)
        {
            if (command == null)
            {
                throw new InvalidParameterException(nameof(command));
            }

            command.Network = network;
            command.WalletAddress = wallet;
            
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("{wallet}/{network}/currency/transaction")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TransferCurrency([FromRoute] string wallet, [FromRoute] string network, [FromBody] WalletSendCurrencyCommand command)
        {
            if (command == null)
            {
                throw new InvalidParameterException(nameof(command));
            }

            command.Network = network;
            command.WalletAddress = wallet;
            
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpGet("{wallet}/{network}/history")]
        [ProducesResponseType(typeof(SuccessResult<GetWalletBalanceHistoryQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetWalletHistory([FromRoute] string wallet, [FromRoute] string network)
        {
            var response = await _mediator.Send(new GetWalletBalanceHistoryQuery
            {
                Network = network,
                WalletAddress = wallet
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("{userId}/{network}/balance")]
        [ProducesResponseType(typeof(SuccessResult<GetPaymentCurrencyBalanceQuery>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetNativeBalance([FromRoute] string userId, [FromRoute] string network)
        {
            var response = await _mediator.Send(new GetPaymentCurrencyBalanceQuery
            {
                Network = network,
                UserId = userId
            });
            return new SuccessResult(response);
        }
    }
}