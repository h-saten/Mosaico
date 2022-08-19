using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.PreValidateNewToken
{
    public class PreValidateNewTokenCommandHandler : IRequestHandler<PreValidateNewTokenCommand, Guid>
    {
        public Task<Guid> Handle(PreValidateNewTokenCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Guid.NewGuid());
        }
    }
}