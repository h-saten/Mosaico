using MediatR;
using Mosaico.Application.ProjectManagement.DTOs.Airdrop;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.Airdrop.GetAirdrop
{
    public class GetAirdropQuery : IRequest<AirdropDTO>
    {
        public string Id { get; set; }
    }
}