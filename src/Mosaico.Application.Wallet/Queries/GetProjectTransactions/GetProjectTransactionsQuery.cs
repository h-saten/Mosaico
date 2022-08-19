using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.GetProjectTransactions
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetProjectTransactionsQuery : IRequest<GetProjectTransactionsQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
        public List<string> Statuses { get; set; } = new List<string>();
        public List<string> PaymentMethods { get; set; } = new List<string>();
        public string CorrelationId { get; set; }
    }
}