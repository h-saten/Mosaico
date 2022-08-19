using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;

namespace Mosaico.Application.Wallet.Queries.GetActiveBlockchains
{
    public class GetActiveBlockchainsQueryHandler : IRequestHandler<GetActiveBlockchainsQuery, GetActiveBlockchainsQueryResponse>
    {
        private readonly BlockchainConfiguration _blockchainConfiguration;

        public GetActiveBlockchainsQueryHandler(BlockchainConfiguration blockchainConfiguration)
        {
            _blockchainConfiguration = blockchainConfiguration;
        }

        public Task<GetActiveBlockchainsQueryResponse> Handle(GetActiveBlockchainsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetActiveBlockchainsQueryResponse
            {
                Networks = _blockchainConfiguration.Networks.Select(n => new BlockchainConfigurationDTO
                {
                    Name = n.Name,
                    IsDefault = n.IsDefault,
                    LogoUrl = n.LogoUrl,
                    Endpoint = n.Endpoint,
                    ChainId = n.Chain,
                    EtherscanUrl = n.EtherscanUrl
                }).ToList()
            });
        }
    }
}