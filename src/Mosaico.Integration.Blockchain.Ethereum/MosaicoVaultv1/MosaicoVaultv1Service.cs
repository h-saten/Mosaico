using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoVaultv1.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoVaultv1
{
    public partial class MosaicoVaultv1Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, MosaicoVaultv1Deployment mosaicoVaultv1Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<MosaicoVaultv1Deployment>().SendRequestAndWaitForReceiptAsync(mosaicoVaultv1Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, MosaicoVaultv1Deployment mosaicoVaultv1Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<MosaicoVaultv1Deployment>().SendRequestAsync(mosaicoVaultv1Deployment);
        }

        public static async Task<MosaicoVaultv1Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, MosaicoVaultv1Deployment mosaicoVaultv1Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, mosaicoVaultv1Deployment, cancellationTokenSource);
            return new MosaicoVaultv1Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public MosaicoVaultv1Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> OwnersQueryAsync(OwnersFunction ownersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnersFunction, string>(ownersFunction, blockParameter);
        }

        
        public Task<string> OwnersQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var ownersFunction = new OwnersFunction();
                ownersFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<OwnersFunction, string>(ownersFunction, blockParameter);
        }

        public Task<string> CancelDepositRequestAsync(CancelDepositFunction cancelDepositFunction)
        {
             return ContractHandler.SendRequestAsync(cancelDepositFunction);
        }

        public Task<TransactionReceipt> CancelDepositRequestAndWaitForReceiptAsync(CancelDepositFunction cancelDepositFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(cancelDepositFunction, cancellationToken);
        }

        public Task<string> CancelDepositRequestAsync(BigInteger id)
        {
            var cancelDepositFunction = new CancelDepositFunction();
                cancelDepositFunction.Id = id;
            
             return ContractHandler.SendRequestAsync(cancelDepositFunction);
        }

        public Task<TransactionReceipt> CancelDepositRequestAndWaitForReceiptAsync(BigInteger id, CancellationTokenSource cancellationToken = null)
        {
            var cancelDepositFunction = new CancelDepositFunction();
                cancelDepositFunction.Id = id;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(cancelDepositFunction, cancellationToken);
        }

        public Task<string> DepositRequestAsync(DepositFunction depositFunction)
        {
             return ContractHandler.SendRequestAsync(depositFunction);
        }

        public Task<TransactionReceipt> DepositRequestAndWaitForReceiptAsync(DepositFunction depositFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(depositFunction, cancellationToken);
        }

        public Task<string> DepositRequestAsync(string token, string withdrawer, BigInteger amount, BigInteger unlockTimestamp)
        {
            var depositFunction = new DepositFunction();
                depositFunction.Token = token;
                depositFunction.Withdrawer = withdrawer;
                depositFunction.Amount = amount;
                depositFunction.UnlockTimestamp = unlockTimestamp;
            
             return ContractHandler.SendRequestAsync(depositFunction);
        }

        public Task<TransactionReceipt> DepositRequestAndWaitForReceiptAsync(string token, string withdrawer, BigInteger amount, BigInteger unlockTimestamp, CancellationTokenSource cancellationToken = null)
        {
            var depositFunction = new DepositFunction();
                depositFunction.Token = token;
                depositFunction.Withdrawer = withdrawer;
                depositFunction.Amount = amount;
                depositFunction.UnlockTimestamp = unlockTimestamp;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(depositFunction, cancellationToken);
        }

        public Task<BigInteger> DepositsByTokenAddressQueryAsync(DepositsByTokenAddressFunction depositsByTokenAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DepositsByTokenAddressFunction, BigInteger>(depositsByTokenAddressFunction, blockParameter);
        }

        
        public Task<BigInteger> DepositsByTokenAddressQueryAsync(string returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var depositsByTokenAddressFunction = new DepositsByTokenAddressFunction();
                depositsByTokenAddressFunction.ReturnValue1 = returnValue1;
                depositsByTokenAddressFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<DepositsByTokenAddressFunction, BigInteger>(depositsByTokenAddressFunction, blockParameter);
        }

        public Task<BigInteger> DepositsByWithdrawersQueryAsync(DepositsByWithdrawersFunction depositsByWithdrawersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DepositsByWithdrawersFunction, BigInteger>(depositsByWithdrawersFunction, blockParameter);
        }

        
        public Task<BigInteger> DepositsByWithdrawersQueryAsync(string returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var depositsByWithdrawersFunction = new DepositsByWithdrawersFunction();
                depositsByWithdrawersFunction.ReturnValue1 = returnValue1;
                depositsByWithdrawersFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<DepositsByWithdrawersFunction, BigInteger>(depositsByWithdrawersFunction, blockParameter);
        }

        public Task<BigInteger> DepositsCountQueryAsync(DepositsCountFunction depositsCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DepositsCountFunction, BigInteger>(depositsCountFunction, blockParameter);
        }

        
        public Task<BigInteger> DepositsCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DepositsCountFunction, BigInteger>(null, blockParameter);
        }

        public Task<List<BigInteger>> GetDepositsByTokenAddressQueryAsync(GetDepositsByTokenAddressFunction getDepositsByTokenAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetDepositsByTokenAddressFunction, List<BigInteger>>(getDepositsByTokenAddressFunction, blockParameter);
        }

        
        public Task<List<BigInteger>> GetDepositsByTokenAddressQueryAsync(string id, BlockParameter blockParameter = null)
        {
            var getDepositsByTokenAddressFunction = new GetDepositsByTokenAddressFunction();
                getDepositsByTokenAddressFunction.Id = id;
            
            return ContractHandler.QueryAsync<GetDepositsByTokenAddressFunction, List<BigInteger>>(getDepositsByTokenAddressFunction, blockParameter);
        }

        public Task<BigInteger> GetDepositsByWithdrawerQueryAsync(GetDepositsByWithdrawerFunction getDepositsByWithdrawerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetDepositsByWithdrawerFunction, BigInteger>(getDepositsByWithdrawerFunction, blockParameter);
        }

        
        public Task<BigInteger> GetDepositsByWithdrawerQueryAsync(string token, string withdrawer, BlockParameter blockParameter = null)
        {
            var getDepositsByWithdrawerFunction = new GetDepositsByWithdrawerFunction();
                getDepositsByWithdrawerFunction.Token = token;
                getDepositsByWithdrawerFunction.Withdrawer = withdrawer;
            
            return ContractHandler.QueryAsync<GetDepositsByWithdrawerFunction, BigInteger>(getDepositsByWithdrawerFunction, blockParameter);
        }

        public Task<List<string>> GetOwnersQueryAsync(GetOwnersFunction getOwnersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOwnersFunction, List<string>>(getOwnersFunction, blockParameter);
        }

        
        public Task<List<string>> GetOwnersQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOwnersFunction, List<string>>(null, blockParameter);
        }

        public Task<GetVaultByIdOutputDTO> GetVaultByIdQueryAsync(GetVaultByIdFunction getVaultByIdFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetVaultByIdFunction, GetVaultByIdOutputDTO>(getVaultByIdFunction, blockParameter);
        }

        public Task<GetVaultByIdOutputDTO> GetVaultByIdQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var getVaultByIdFunction = new GetVaultByIdFunction();
                getVaultByIdFunction.Id = id;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetVaultByIdFunction, GetVaultByIdOutputDTO>(getVaultByIdFunction, blockParameter);
        }

        public Task<List<BigInteger>> GetVaultsByWithdrawerQueryAsync(GetVaultsByWithdrawerFunction getVaultsByWithdrawerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetVaultsByWithdrawerFunction, List<BigInteger>>(getVaultsByWithdrawerFunction, blockParameter);
        }

        
        public Task<List<BigInteger>> GetVaultsByWithdrawerQueryAsync(string withdrawer, BlockParameter blockParameter = null)
        {
            var getVaultsByWithdrawerFunction = new GetVaultsByWithdrawerFunction();
                getVaultsByWithdrawerFunction.Withdrawer = withdrawer;
            
            return ContractHandler.QueryAsync<GetVaultsByWithdrawerFunction, List<BigInteger>>(getVaultsByWithdrawerFunction, blockParameter);
        }

        public Task<bool> IsOwnerQueryAsync(IsOwnerFunction isOwnerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsOwnerFunction, bool>(isOwnerFunction, blockParameter);
        }

        
        public Task<bool> IsOwnerQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isOwnerFunction = new IsOwnerFunction();
                isOwnerFunction.Account = account;
            
            return ContractHandler.QueryAsync<IsOwnerFunction, bool>(isOwnerFunction, blockParameter);
        }

        public Task<LockedTokenOutputDTO> LockedTokenQueryAsync(LockedTokenFunction lockedTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<LockedTokenFunction, LockedTokenOutputDTO>(lockedTokenFunction, blockParameter);
        }

        public Task<LockedTokenOutputDTO> LockedTokenQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var lockedTokenFunction = new LockedTokenFunction();
                lockedTokenFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryDeserializingToObjectAsync<LockedTokenFunction, LockedTokenOutputDTO>(lockedTokenFunction, blockParameter);
        }

        public Task<string> SendRequestAsync(SendFunction sendFunction)
        {
             return ContractHandler.SendRequestAsync(sendFunction);
        }

        public Task<TransactionReceipt> SendRequestAndWaitForReceiptAsync(SendFunction sendFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(sendFunction, cancellationToken);
        }

        public Task<string> SendRequestAsync(BigInteger id, string recipient, BigInteger amount)
        {
            var sendFunction = new SendFunction();
                sendFunction.Id = id;
                sendFunction.Recipient = recipient;
                sendFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(sendFunction);
        }

        public Task<TransactionReceipt> SendRequestAndWaitForReceiptAsync(BigInteger id, string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var sendFunction = new SendFunction();
                sendFunction.Id = id;
                sendFunction.Recipient = recipient;
                sendFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(sendFunction, cancellationToken);
        }

        public Task<BigInteger> WalletTokenBalanceQueryAsync(WalletTokenBalanceFunction walletTokenBalanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WalletTokenBalanceFunction, BigInteger>(walletTokenBalanceFunction, blockParameter);
        }

        
        public Task<BigInteger> WalletTokenBalanceQueryAsync(string returnValue1, string returnValue2, BlockParameter blockParameter = null)
        {
            var walletTokenBalanceFunction = new WalletTokenBalanceFunction();
                walletTokenBalanceFunction.ReturnValue1 = returnValue1;
                walletTokenBalanceFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<WalletTokenBalanceFunction, BigInteger>(walletTokenBalanceFunction, blockParameter);
        }

        public Task<string> WithdrawTokensRequestAsync(WithdrawTokensFunction withdrawTokensFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawTokensFunction);
        }

        public Task<TransactionReceipt> WithdrawTokensRequestAndWaitForReceiptAsync(WithdrawTokensFunction withdrawTokensFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawTokensFunction, cancellationToken);
        }

        public Task<string> WithdrawTokensRequestAsync(BigInteger id)
        {
            var withdrawTokensFunction = new WithdrawTokensFunction();
                withdrawTokensFunction.Id = id;
            
             return ContractHandler.SendRequestAsync(withdrawTokensFunction);
        }

        public Task<TransactionReceipt> WithdrawTokensRequestAndWaitForReceiptAsync(BigInteger id, CancellationTokenSource cancellationToken = null)
        {
            var withdrawTokensFunction = new WithdrawTokensFunction();
                withdrawTokensFunction.Id = id;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawTokensFunction, cancellationToken);
        }
    }
}
