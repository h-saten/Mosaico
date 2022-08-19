using System;
using MediatR;
using Mosaico.Application.Wallet.Commands.TokenManagement.CreateToken;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.PreValidateNewToken
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class PreValidateNewTokenCommand : CreateTokenCommand, IRequest<Guid>
    {
    }
}