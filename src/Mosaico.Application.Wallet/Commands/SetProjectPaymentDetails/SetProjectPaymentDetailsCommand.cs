using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.SetProjectPaymentDetails
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class SetProjectPaymentDetailsCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public string Account { get; set; }
        public string BankName { get; set; }
        public string Swift { get; set; }
        public string Key { get; set; }
        public string AccountAddress { get; set; }
    }
}