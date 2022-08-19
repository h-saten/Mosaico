using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Extensions;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateSalesAgent
{
    public class UpdateSalesAgentCommandHandler : IRequestHandler<UpdateSalesAgentCommand>
    {
        private readonly IWalletDbContext _context;
        private readonly IUserManagementClient _userManagementClient;
        private readonly ICurrentUserContext _userContext;

        public UpdateSalesAgentCommandHandler(IWalletDbContext context, IUserManagementClient userManagementClient, ICurrentUserContext userContext)
        {
            _context = context;
            _userManagementClient = userManagementClient;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(UpdateSalesAgentCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == request.TransactionId, cancellationToken);
            if (transaction == null || transaction.Type.Key != Domain.Wallet.Constants.TransactionType.Purchase || !transaction.ProjectId.HasValue) 
                throw new TransactionNotFoundException(request.TransactionId.ToString());
            var permissions = await _userManagementClient.GetUserPermissionsAsync(_userContext.UserId, transaction.ProjectId.Value, cancellationToken);
            if (!permissions.HasPermission(Authorization.Base.Constants.Permissions.Project.CanEditDetails) && !_userContext.IsGlobalAdmin)
                throw new ForbiddenException();
            if (request.AgentId.HasValue)
            {
                var salesAgent =
                    await _context.SalesAgents.FirstOrDefaultAsync(s => s.Id == request.AgentId, cancellationToken);
                if (salesAgent == null) throw new SalesAgentNotFoundException(request.AgentId.Value);
                transaction.SalesAgent = salesAgent;
                transaction.SalesAgentId = salesAgent.Id;
            }
            else
            {
                transaction.SalesAgent = null;
                transaction.SalesAgentId = null;
            }

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}