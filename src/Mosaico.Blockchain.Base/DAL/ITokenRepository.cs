using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL.Models;

namespace Mosaico.Blockchain.Base.DAL
{
    public interface ITokenRepository
    {
        // TODO blocks number range to vo
        Task<List<ERC20Transfer>> Erc20TransfersAsync(string contractAddress, string chain, ulong? fromBlock = null, ulong? toBlock = null);
    }
}