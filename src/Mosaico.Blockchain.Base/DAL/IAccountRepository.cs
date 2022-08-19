using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL.Models;

namespace Mosaico.Blockchain.Base.DAL
{
    public interface IAccountRepository
    {
        Task<NativeBalance> AccountBalanceAsync(string walletAddress, string chain);
        Task<BigInteger> Erc20BalanceAsync(string walletAddress, string tokenAddress, string chain, string tokenType = "ERC20");
    }
}