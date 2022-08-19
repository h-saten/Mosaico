using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Extensions;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateTransactionFee
{
    public class UpdateTransactionFeeCommandHandler : IRequestHandler<UpdateTransactionFeeCommand>
    {
        private readonly IWalletDbContext _context;
        private readonly IUserManagementClient _userManagementClient;
        private readonly ICurrentUserContext _userContext;
        private readonly IFeeService _feeService;

        public UpdateTransactionFeeCommandHandler(IWalletDbContext context, IUserManagementClient userManagementClient, ICurrentUserContext userContext, IFeeService feeService)
        {
            _context = context;
            _userManagementClient = userManagementClient;
            _userContext = userContext;
            _feeService = feeService;
        }

        public async Task<Unit> Handle(UpdateTransactionFeeCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == request.TransactionId, cancellationToken);
            if (transaction == null || transaction.Type.Key != Domain.Wallet.Constants.TransactionType.Purchase || !transaction.ProjectId.HasValue) 
                throw new TransactionNotFoundException(request.TransactionId.ToString());
            var permissions = await _userManagementClient.GetUserPermissionsAsync(_userContext.UserId, transaction.ProjectId.Value, cancellationToken);
            if (!permissions.HasPermission(Authorization.Base.Constants.Permissions.Project.CanEditDetails) && !_userContext.IsGlobalAdmin)
                throw new ForbiddenException();
            transaction.FeePercentage = request.FeePercentage;
            transaction.MosaicoFee = _feeService.GetMosaicoFeeValueAsync(transaction.FeePercentage, transaction.Fee, transaction.PayedAmount.Value);
            transaction.MosaicoFeeInUSD = transaction.MosaicoFee * transaction.ExchangeRate;
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}