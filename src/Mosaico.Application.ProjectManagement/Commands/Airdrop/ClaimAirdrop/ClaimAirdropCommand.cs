using System;
using MediatR;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.ClaimAirdrop
{
    public class ClaimAirdropCommand : IRequest
    {
        public Guid AirdropId { get; set; }
    }
}