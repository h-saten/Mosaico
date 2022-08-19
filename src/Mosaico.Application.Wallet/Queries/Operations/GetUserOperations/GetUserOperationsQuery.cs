using MediatR;
using Mosaico.Authorization.Base;
using Newtonsoft.Json;

namespace Mosaico.Application.Wallet.Queries.Operations.GetUserOperations
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetUserOperationsQuery : IRequest<GetUserOperationsQueryResponse>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}