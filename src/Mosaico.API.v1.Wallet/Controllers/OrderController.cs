using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.Checkout.ConfirmBankTransfer;
using Mosaico.Application.Wallet.Commands.Checkout.CreateBankTransferTransaction;
using Mosaico.Application.Wallet.Commands.Checkout.CreateBinanceTransaction;
using Mosaico.Application.Wallet.Commands.Checkout.CreateMetamaskTransaction;
using Mosaico.Application.Wallet.Commands.Checkout.CreateMosaicoTransaction;
using Mosaico.Application.Wallet.Commands.Checkout.CreateRampTransaction;
using Mosaico.Application.Wallet.Commands.Checkout.CreateTransakTransaction;
using Mosaico.Application.Wallet.Commands.Checkout.PrevalidatePurchase;
using Mosaico.Application.Wallet.Commands.Transactions.InitiateTransaction;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Queries.Checkout.BankTransferCheckoutContext;
using Mosaico.Application.Wallet.Queries.Checkout.BinancePayCheckoutContext;
using Mosaico.Application.Wallet.Queries.Checkout.CreditCardCheckoutContext;
using Mosaico.Application.Wallet.Queries.Checkout.KangaCheckoutContext;
using Mosaico.Application.Wallet.Queries.Checkout.MetamaskCheckoutContext;
using Mosaico.Application.Wallet.Queries.Checkout.MosaicoWalletCheckoutContext;
using Mosaico.Application.Wallet.Queries.GetTransaction;
using Mosaico.Application.Wallet.Queries.GetTransactionStatus;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Wallet.Entities;
using Constants = Mosaico.Domain.ProjectManagement.Constants;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/projects/{projectId}/orders")]
    [Route("api/v{version:apiVersion}/projects/{projectId}/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ICurrentUserContext _userContext;
        private readonly IMediator _mediator;

        public OrderController(ICurrentUserContext userContext, IMediator mediator)
        {
            _userContext = userContext;
            _mediator = mediator;
        }
        
        [HttpGet("/api/orders/{id}/status")]
        [ProducesResponseType(typeof(SuccessResult<GetTransactionStatusQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetOrderStatus([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetTransactionStatusQuery
            {
                Id = id
            });
            return new SuccessResult(response);
        }
        
        [HttpPost("bankTransfer")]
        [ProducesResponseType(typeof(SuccessResult<ProjectBankPaymentDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> CreateBankTransferAsync([FromRoute] Guid projectId, [FromBody] CreateBankTransferTransactionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = projectId;
            command.UserId = _userContext.UserId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResult<ProjectBankPaymentDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> ConfirmBankTransferAsync([FromRoute] Guid projectId, [FromRoute] Guid id)
        {
            await _mediator.Send(new ConfirmBankTransferCommand
            {
                TransactionId = id,
                ProjectId = projectId
            });
            return new SuccessResult();
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResult<TransactionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetTransaction([FromRoute] Guid projectId, [FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetTransactionQuery
            {
                Id = id,
                ProjectId = projectId
            });
            return new SuccessResult(response);
        }

        [HttpGet("quote")]
        [ProducesResponseType(typeof(SuccessResult<CreditCardCheckoutContextQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetCreditCardContext([FromRoute] Guid projectId, [FromQuery] string type)
        {
            switch (type)
            {
                case Constants.PaymentMethods.CreditCard:
                {
                    var response = await _mediator.Send(new CreditCardCheckoutContextQuery
                    {
                        ProjectId = projectId,
                        UserId = _userContext.UserId
                    });
                    return new SuccessResult(response);
                }
                case Constants.PaymentMethods.BankTransfer:
                {
                    var response = await _mediator.Send(new BankTransferCheckoutContextQuery
                    {
                        ProjectId = projectId,
                        UserId = _userContext.UserId
                    });
                    return new SuccessResult(response);
                }
                case Constants.PaymentMethods.Metamask:
                {
                    var response = await _mediator.Send(new MetamaskCheckoutContextQuery
                    {
                        ProjectId = projectId,
                        UserId = _userContext.UserId
                    });
                    return new SuccessResult(response);
                }
                case Constants.PaymentMethods.KangaExchange:
                {
                    var response = await _mediator.Send(new KangaCheckoutContextQuery
                    {
                        ProjectId = projectId,
                        UserId = _userContext.UserId
                    });
                    return new SuccessResult(response);
                }
                case Constants.PaymentMethods.MosaicoWallet:
                {
                    var response = await _mediator.Send(new MosaicoWalletCheckoutContextQuery
                    {
                        ProjectId = projectId,
                        UserId = _userContext.UserId
                    });
                    return new SuccessResult(response);
                }
                case Constants.PaymentMethods.Binance:
                {
                    var response = await _mediator.Send(new BinancePayCheckoutContextQuery
                    {
                        ProjectId = projectId,
                        UserId = _userContext.UserId
                    });
                    return new SuccessResult(response);
                }
                default:
                    throw new InvalidParameterException(nameof(type));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InitiatePurchaseTransaction([FromRoute] Guid projectId, [FromBody] InitiateTransactionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = projectId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
        
        [HttpPost("ramp")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InitiateRampTransaction([FromRoute] Guid projectId, [FromBody] CreateRampTransactionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = projectId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
        
        [HttpPost("transak")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InitiateTransakTransaction([FromRoute] Guid projectId, [FromBody] CreateTransakTransactionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = projectId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
        
        [HttpPost("metamask")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InitiateMetamaskTransaction([FromRoute] Guid projectId, [FromBody] CreateMetamaskTransactionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = projectId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
        
        [HttpPost("mosaico-wallet")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InitiateMosaicoWallet([FromRoute] Guid projectId, [FromBody] CreateMosaicoTransactionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = projectId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
        
        [HttpPost("binance")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InitiateBinance([FromRoute] Guid projectId, [FromBody] CreateBinanceTransactionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = projectId;
            command.UserId = _userContext.UserId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }

        [HttpPost("validation")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidatePurchase([FromRoute] Guid projectId, [FromBody] PrevalidatePurchaseCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = projectId;
            command.UserId = _userContext.UserId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
    }
}