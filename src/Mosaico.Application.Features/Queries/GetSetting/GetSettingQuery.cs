using System;
using MediatR;

namespace Mosaico.Application.Features.Queries.GetSetting
{
    public class GetSettingQuery : IRequest<GetSettingQueryResponse>
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public Guid? EntityId { get; set; }
    }
}