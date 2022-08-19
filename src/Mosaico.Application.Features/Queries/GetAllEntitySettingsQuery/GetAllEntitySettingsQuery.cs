using System;
using MediatR;

namespace Mosaico.Application.Features.Queries.GetAllEntitySettingsQuery
{
    public class GetAllEntitySettingsQuery : IRequest<GetAllEntitySettingsQueryResponse>
    {
        public string Category { get; set; }
        public Guid? EntityId { get; set; }
    }
}