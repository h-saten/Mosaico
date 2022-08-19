using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Transactions.ExportTransactions
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class ExportTransactionsCommand : IRequest<ExportTransactionsCommandResponse>
    {
        public string Format { get; set; }
        public Guid ProjectId { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
        public List<string> Statuses { get; set; } = new List<string>();
        public List<string> PaymentMethods { get; set; } = new List<string>();
    }
}