using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.ImportTokenDetails
{
    [Cache("{{Chain}}_{{ContractAddress}}")]
    public class ImportTokenDetailsQuery : IRequest<ImportTokenDetailsResponse>
    {
        public string Chain { get; set; }
        public string ContractAddress { get; set; }
    }
}