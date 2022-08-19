using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateSalesAgent
{
    // restriction is in the command handler
    public class UpdateSalesAgentCommand : IRequest
    {
        public Guid TransactionId { get; set; }
        public Guid? AgentId { get; set; }
    }
}