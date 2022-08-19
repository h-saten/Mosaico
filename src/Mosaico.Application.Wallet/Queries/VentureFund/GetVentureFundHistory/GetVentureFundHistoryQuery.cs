using System;
using MediatR;

namespace Mosaico.Application.Wallet.Queries.VentureFund.GetVentureFundHistory
{
    public class GetVentureFundHistoryQuery : IRequest<GetVentureFundHistoryQueryResponse>
    {
        public string Name { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
    }
}