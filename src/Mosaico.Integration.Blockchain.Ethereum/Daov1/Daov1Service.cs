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
using Mosaico.Integration.Blockchain.Ethereum.Daov1.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.Daov1
{
    public partial class Daov1Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, Daov1Deployment daov1Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<Daov1Deployment>().SendRequestAndWaitForReceiptAsync(daov1Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, Daov1Deployment daov1Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<Daov1Deployment>().SendRequestAsync(daov1Deployment);
        }

        public static async Task<Daov1Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, Daov1Deployment daov1Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, daov1Deployment, cancellationTokenSource);
            return new Daov1Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public Daov1Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<byte[]> BALLOT_TYPEHASHQueryAsync(BALLOT_TYPEHASHFunction bALLOT_TYPEHASHFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BALLOT_TYPEHASHFunction, byte[]>(bALLOT_TYPEHASHFunction, blockParameter);
        }

        
        public Task<byte[]> BALLOT_TYPEHASHQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BALLOT_TYPEHASHFunction, byte[]>(null, blockParameter);
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

        public Task<string> AddOwnerRequestAsync(AddOwnerFunction addOwnerFunction)
        {
             return ContractHandler.SendRequestAsync(addOwnerFunction);
        }

        public Task<TransactionReceipt> AddOwnerRequestAndWaitForReceiptAsync(AddOwnerFunction addOwnerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addOwnerFunction, cancellationToken);
        }

        public Task<string> AddOwnerRequestAsync(string newOwner)
        {
            var addOwnerFunction = new AddOwnerFunction();
                addOwnerFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(addOwnerFunction);
        }

        public Task<TransactionReceipt> AddOwnerRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var addOwnerFunction = new AddOwnerFunction();
                addOwnerFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addOwnerFunction, cancellationToken);
        }

        public Task<string> AddTokenRequestAsync(AddTokenFunction addTokenFunction)
        {
             return ContractHandler.SendRequestAsync(addTokenFunction);
        }

        public Task<TransactionReceipt> AddTokenRequestAndWaitForReceiptAsync(AddTokenFunction addTokenFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addTokenFunction, cancellationToken);
        }

        public Task<string> AddTokenRequestAsync(string token, bool isVoting)
        {
            var addTokenFunction = new AddTokenFunction();
                addTokenFunction.Token = token;
                addTokenFunction.IsVoting = isVoting;
            
             return ContractHandler.SendRequestAsync(addTokenFunction);
        }

        public Task<TransactionReceipt> AddTokenRequestAndWaitForReceiptAsync(string token, bool isVoting, CancellationTokenSource cancellationToken = null)
        {
            var addTokenFunction = new AddTokenFunction();
                addTokenFunction.Token = token;
                addTokenFunction.IsVoting = isVoting;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addTokenFunction, cancellationToken);
        }

        public Task<string> BurnRequestAsync(BurnFunction burnFunction)
        {
             return ContractHandler.SendRequestAsync(burnFunction);
        }

        public Task<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(BurnFunction burnFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public Task<string> BurnRequestAsync(string tokenAddress, BigInteger amount)
        {
            var burnFunction = new BurnFunction();
                burnFunction.TokenAddress = tokenAddress;
                burnFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(burnFunction);
        }

        public Task<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(string tokenAddress, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var burnFunction = new BurnFunction();
                burnFunction.TokenAddress = tokenAddress;
                burnFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public Task<string> CastVoteRequestAsync(CastVoteFunction castVoteFunction)
        {
             return ContractHandler.SendRequestAsync(castVoteFunction);
        }

        public Task<TransactionReceipt> CastVoteRequestAndWaitForReceiptAsync(CastVoteFunction castVoteFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(castVoteFunction, cancellationToken);
        }

        public Task<string> CastVoteRequestAsync(BigInteger proposalId, byte support)
        {
            var castVoteFunction = new CastVoteFunction();
                castVoteFunction.ProposalId = proposalId;
                castVoteFunction.Support = support;
            
             return ContractHandler.SendRequestAsync(castVoteFunction);
        }

        public Task<TransactionReceipt> CastVoteRequestAndWaitForReceiptAsync(BigInteger proposalId, byte support, CancellationTokenSource cancellationToken = null)
        {
            var castVoteFunction = new CastVoteFunction();
                castVoteFunction.ProposalId = proposalId;
                castVoteFunction.Support = support;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(castVoteFunction, cancellationToken);
        }

        public Task<string> CreateRequestAsync(CreateFunction createFunction)
        {
             return ContractHandler.SendRequestAsync(createFunction);
        }

        public Task<TransactionReceipt> CreateRequestAndWaitForReceiptAsync(CreateFunction createFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createFunction, cancellationToken);
        }

        public Task<string> CreateRequestAsync(ERC20Settings contractSettings)
        {
            var createFunction = new CreateFunction();
                createFunction.ContractSettings = contractSettings;
            
             return ContractHandler.SendRequestAsync(createFunction);
        }

        public Task<TransactionReceipt> CreateRequestAndWaitForReceiptAsync(ERC20Settings contractSettings, CancellationTokenSource cancellationToken = null)
        {
            var createFunction = new CreateFunction();
                createFunction.ContractSettings = contractSettings;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createFunction, cancellationToken);
        }

        public Task<List<string>> GetOwnersQueryAsync(GetOwnersFunction getOwnersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOwnersFunction, List<string>>(getOwnersFunction, blockParameter);
        }

        
        public Task<List<string>> GetOwnersQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOwnersFunction, List<string>>(null, blockParameter);
        }

        public Task<List<string>> GetTokensQueryAsync(GetTokensFunction getTokensFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTokensFunction, List<string>>(getTokensFunction, blockParameter);
        }

        
        public Task<List<string>> GetTokensQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTokensFunction, List<string>>(null, blockParameter);
        }

        public Task<BigInteger> GetWeightQueryAsync(GetWeightFunction getWeightFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetWeightFunction, BigInteger>(getWeightFunction, blockParameter);
        }

        
        public Task<BigInteger> GetWeightQueryAsync(string account, string tokenAddress, BlockParameter blockParameter = null)
        {
            var getWeightFunction = new GetWeightFunction();
                getWeightFunction.Account = account;
                getWeightFunction.TokenAddress = tokenAddress;
            
            return ContractHandler.QueryAsync<GetWeightFunction, BigInteger>(getWeightFunction, blockParameter);
        }

        public Task<bool> HasVotedQueryAsync(HasVotedFunction hasVotedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<HasVotedFunction, bool>(hasVotedFunction, blockParameter);
        }

        
        public Task<bool> HasVotedQueryAsync(BigInteger proposalId, string account, BlockParameter blockParameter = null)
        {
            var hasVotedFunction = new HasVotedFunction();
                hasVotedFunction.ProposalId = proposalId;
                hasVotedFunction.Account = account;
            
            return ContractHandler.QueryAsync<HasVotedFunction, bool>(hasVotedFunction, blockParameter);
        }

        public Task<BigInteger> HashProposalQueryAsync(HashProposalFunction hashProposalFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<HashProposalFunction, BigInteger>(hashProposalFunction, blockParameter);
        }

        
        public Task<BigInteger> HashProposalQueryAsync(string description, string target, BlockParameter blockParameter = null)
        {
            var hashProposalFunction = new HashProposalFunction();
                hashProposalFunction.Description = description;
                hashProposalFunction.Target = target;
            
            return ContractHandler.QueryAsync<HashProposalFunction, BigInteger>(hashProposalFunction, blockParameter);
        }

        public Task<bool> IsManagedTokenQueryAsync(IsManagedTokenFunction isManagedTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsManagedTokenFunction, bool>(isManagedTokenFunction, blockParameter);
        }

        
        public Task<bool> IsManagedTokenQueryAsync(string token, BlockParameter blockParameter = null)
        {
            var isManagedTokenFunction = new IsManagedTokenFunction();
                isManagedTokenFunction.Token = token;
            
            return ContractHandler.QueryAsync<IsManagedTokenFunction, bool>(isManagedTokenFunction, blockParameter);
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

        public Task<bool> IsVotingEnabledQueryAsync(IsVotingEnabledFunction isVotingEnabledFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsVotingEnabledFunction, bool>(isVotingEnabledFunction, blockParameter);
        }

        
        public Task<bool> IsVotingEnabledQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsVotingEnabledFunction, bool>(null, blockParameter);
        }

        public Task<bool> IsVotingTokenQueryAsync(IsVotingTokenFunction isVotingTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsVotingTokenFunction, bool>(isVotingTokenFunction, blockParameter);
        }

        
        public Task<bool> IsVotingTokenQueryAsync(string token, BlockParameter blockParameter = null)
        {
            var isVotingTokenFunction = new IsVotingTokenFunction();
                isVotingTokenFunction.Token = token;
            
            return ContractHandler.QueryAsync<IsVotingTokenFunction, bool>(isVotingTokenFunction, blockParameter);
        }

        public Task<string> MintRequestAsync(MintFunction mintFunction)
        {
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(MintFunction mintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<string> MintRequestAsync(string tokenAddress, BigInteger amount)
        {
            var mintFunction = new MintFunction();
                mintFunction.TokenAddress = tokenAddress;
                mintFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(string tokenAddress, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var mintFunction = new MintFunction();
                mintFunction.TokenAddress = tokenAddress;
                mintFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        
        public Task<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public Task<bool> OnlyOwnerProposalsQueryAsync(OnlyOwnerProposalsFunction onlyOwnerProposalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OnlyOwnerProposalsFunction, bool>(onlyOwnerProposalsFunction, blockParameter);
        }

        
        public Task<bool> OnlyOwnerProposalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OnlyOwnerProposalsFunction, bool>(null, blockParameter);
        }

        public Task<string> PauseRequestAsync(PauseFunction pauseFunction)
        {
             return ContractHandler.SendRequestAsync(pauseFunction);
        }

        public Task<string> PauseRequestAsync()
        {
             return ContractHandler.SendRequestAsync<PauseFunction>();
        }

        public Task<TransactionReceipt> PauseRequestAndWaitForReceiptAsync(PauseFunction pauseFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pauseFunction, cancellationToken);
        }

        public Task<TransactionReceipt> PauseRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<PauseFunction>(null, cancellationToken);
        }

        public Task<string> PauseTokenRequestAsync(PauseTokenFunction pauseTokenFunction)
        {
             return ContractHandler.SendRequestAsync(pauseTokenFunction);
        }

        public Task<TransactionReceipt> PauseTokenRequestAndWaitForReceiptAsync(PauseTokenFunction pauseTokenFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pauseTokenFunction, cancellationToken);
        }

        public Task<string> PauseTokenRequestAsync(string tokenAddress)
        {
            var pauseTokenFunction = new PauseTokenFunction();
                pauseTokenFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAsync(pauseTokenFunction);
        }

        public Task<TransactionReceipt> PauseTokenRequestAndWaitForReceiptAsync(string tokenAddress, CancellationTokenSource cancellationToken = null)
        {
            var pauseTokenFunction = new PauseTokenFunction();
                pauseTokenFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pauseTokenFunction, cancellationToken);
        }

        public Task<bool> PausedQueryAsync(PausedFunction pausedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(pausedFunction, blockParameter);
        }

        
        public Task<bool> PausedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(null, blockParameter);
        }

        public Task<BigInteger> ProposalDeadlineQueryAsync(ProposalDeadlineFunction proposalDeadlineFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ProposalDeadlineFunction, BigInteger>(proposalDeadlineFunction, blockParameter);
        }

        
        public Task<BigInteger> ProposalDeadlineQueryAsync(BigInteger proposalId, BlockParameter blockParameter = null)
        {
            var proposalDeadlineFunction = new ProposalDeadlineFunction();
                proposalDeadlineFunction.ProposalId = proposalId;
            
            return ContractHandler.QueryAsync<ProposalDeadlineFunction, BigInteger>(proposalDeadlineFunction, blockParameter);
        }

        public Task<BigInteger> ProposalSnapshotQueryAsync(ProposalSnapshotFunction proposalSnapshotFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ProposalSnapshotFunction, BigInteger>(proposalSnapshotFunction, blockParameter);
        }

        
        public Task<BigInteger> ProposalSnapshotQueryAsync(BigInteger proposalId, BlockParameter blockParameter = null)
        {
            var proposalSnapshotFunction = new ProposalSnapshotFunction();
                proposalSnapshotFunction.ProposalId = proposalId;
            
            return ContractHandler.QueryAsync<ProposalSnapshotFunction, BigInteger>(proposalSnapshotFunction, blockParameter);
        }

        public Task<ProposalVotesOutputDTO> ProposalVotesQueryAsync(ProposalVotesFunction proposalVotesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<ProposalVotesFunction, ProposalVotesOutputDTO>(proposalVotesFunction, blockParameter);
        }

        public Task<ProposalVotesOutputDTO> ProposalVotesQueryAsync(BigInteger proposalId, BlockParameter blockParameter = null)
        {
            var proposalVotesFunction = new ProposalVotesFunction();
                proposalVotesFunction.ProposalId = proposalId;
            
            return ContractHandler.QueryDeserializingToObjectAsync<ProposalVotesFunction, ProposalVotesOutputDTO>(proposalVotesFunction, blockParameter);
        }

        public Task<string> ProposeRequestAsync(ProposeFunction proposeFunction)
        {
             return ContractHandler.SendRequestAsync(proposeFunction);
        }

        public Task<TransactionReceipt> ProposeRequestAndWaitForReceiptAsync(ProposeFunction proposeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(proposeFunction, cancellationToken);
        }

        public Task<string> ProposeRequestAsync(string description, string tokenAddress)
        {
            var proposeFunction = new ProposeFunction();
                proposeFunction.Description = description;
                proposeFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAsync(proposeFunction);
        }

        public Task<TransactionReceipt> ProposeRequestAndWaitForReceiptAsync(string description, string tokenAddress, CancellationTokenSource cancellationToken = null)
        {
            var proposeFunction = new ProposeFunction();
                proposeFunction.Description = description;
                proposeFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(proposeFunction, cancellationToken);
        }

        public Task<BigInteger> QuorumQueryAsync(QuorumFunction quorumFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<QuorumFunction, BigInteger>(quorumFunction, blockParameter);
        }

        
        public Task<BigInteger> QuorumQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<QuorumFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> RemoveOwnerRequestAsync(RemoveOwnerFunction removeOwnerFunction)
        {
             return ContractHandler.SendRequestAsync(removeOwnerFunction);
        }

        public Task<TransactionReceipt> RemoveOwnerRequestAndWaitForReceiptAsync(RemoveOwnerFunction removeOwnerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeOwnerFunction, cancellationToken);
        }

        public Task<string> RemoveOwnerRequestAsync(string owner)
        {
            var removeOwnerFunction = new RemoveOwnerFunction();
                removeOwnerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAsync(removeOwnerFunction);
        }

        public Task<TransactionReceipt> RemoveOwnerRequestAndWaitForReceiptAsync(string owner, CancellationTokenSource cancellationToken = null)
        {
            var removeOwnerFunction = new RemoveOwnerFunction();
                removeOwnerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeOwnerFunction, cancellationToken);
        }

        public Task<byte> StateQueryAsync(StateFunction stateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StateFunction, byte>(stateFunction, blockParameter);
        }

        
        public Task<byte> StateQueryAsync(BigInteger proposalId, BlockParameter blockParameter = null)
        {
            var stateFunction = new StateFunction();
                stateFunction.ProposalId = proposalId;
            
            return ContractHandler.QueryAsync<StateFunction, byte>(stateFunction, blockParameter);
        }

        public Task<BigInteger> TokenQuorumQueryAsync(TokenQuorumFunction tokenQuorumFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokenQuorumFunction, BigInteger>(tokenQuorumFunction, blockParameter);
        }

        
        public Task<BigInteger> TokenQuorumQueryAsync(string tokenAddress, BlockParameter blockParameter = null)
        {
            var tokenQuorumFunction = new TokenQuorumFunction();
                tokenQuorumFunction.TokenAddress = tokenAddress;
            
            return ContractHandler.QueryAsync<TokenQuorumFunction, BigInteger>(tokenQuorumFunction, blockParameter);
        }

        public Task<string> UnpauseRequestAsync(UnpauseFunction unpauseFunction)
        {
             return ContractHandler.SendRequestAsync(unpauseFunction);
        }

        public Task<string> UnpauseRequestAsync()
        {
             return ContractHandler.SendRequestAsync<UnpauseFunction>();
        }

        public Task<TransactionReceipt> UnpauseRequestAndWaitForReceiptAsync(UnpauseFunction unpauseFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unpauseFunction, cancellationToken);
        }

        public Task<TransactionReceipt> UnpauseRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<UnpauseFunction>(null, cancellationToken);
        }

        public Task<string> UnpauseTokenRequestAsync(UnpauseTokenFunction unpauseTokenFunction)
        {
             return ContractHandler.SendRequestAsync(unpauseTokenFunction);
        }

        public Task<TransactionReceipt> UnpauseTokenRequestAndWaitForReceiptAsync(UnpauseTokenFunction unpauseTokenFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unpauseTokenFunction, cancellationToken);
        }

        public Task<string> UnpauseTokenRequestAsync(string tokenAddress)
        {
            var unpauseTokenFunction = new UnpauseTokenFunction();
                unpauseTokenFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAsync(unpauseTokenFunction);
        }

        public Task<TransactionReceipt> UnpauseTokenRequestAndWaitForReceiptAsync(string tokenAddress, CancellationTokenSource cancellationToken = null)
        {
            var unpauseTokenFunction = new UnpauseTokenFunction();
                unpauseTokenFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unpauseTokenFunction, cancellationToken);
        }

        public Task<string> VersionQueryAsync(VersionFunction versionFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VersionFunction, string>(versionFunction, blockParameter);
        }

        
        public Task<string> VersionQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VersionFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> VotingDelayQueryAsync(VotingDelayFunction votingDelayFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VotingDelayFunction, BigInteger>(votingDelayFunction, blockParameter);
        }

        
        public Task<BigInteger> VotingDelayQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VotingDelayFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> VotingPeriodQueryAsync(VotingPeriodFunction votingPeriodFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VotingPeriodFunction, BigInteger>(votingPeriodFunction, blockParameter);
        }

        
        public Task<BigInteger> VotingPeriodQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VotingPeriodFunction, BigInteger>(null, blockParameter);
        }
    }
}
