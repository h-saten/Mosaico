// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "../../node_modules/@openzeppelin/contracts/utils/cryptography/draft-EIP712.sol";
import "../../node_modules/@openzeppelin/contracts/utils/Timers.sol";
import "../../node_modules/@openzeppelin/contracts/utils/math/SafeCast.sol";
import "../MosaicoERC20v1.sol";
import "../MultiOwnable.sol";
import "./TokenManagerv1.sol";
import "../../node_modules/@openzeppelin/contracts/security/Pausable.sol";
import "../../node_modules/@openzeppelin/contracts/security/ReentrancyGuard.sol";

contract Daov1 is EIP712, MultiOwnable, Pausable, ReentrancyGuard, TokenManagerv1 {
    using SafeCast for uint256;
    using Timers for Timers.BlockNumber;

    struct Vote {
        uint256 againstVotes;
        uint256 forVotes;
        uint256 abstainVotes;
        mapping(address => bool) hasVoted;
    }

    enum VoteResult {
        Against,
        For,
        Abstain
    }

    enum ProposalState {
        Pending,
        Active,
        Canceled,
        Defeated,
        Succeeded,
        Queued,
        Expired,
        Executed
    }

    struct Proposal {
        Timers.BlockNumber voteStart;
        Timers.BlockNumber voteEnd;
        address tokenAddress;
        bool executed;
        bool canceled;
    }

    struct DaoSettings {
        /* name of the dao */
        string name;
        /* is anyone able to create proposals and vote */
        bool isVotingEnabled;
        /* if only owners can create proposals */
        bool onlyOwnerProposals;
        /* 1 block - no delay (13.2 sec) */
        uint256 initialVotingDelay;
        /* 45818 blocks - 1 week; 6545 blocks- 1 day. */
        uint256 initialVotingPeriod;
        /* How many % of hodlers should vote to accomplish consensus */
        uint256 quorum;
        /* First owner of the DAO */
        address owner;
    }

    /* 
        PROPERTIES
    */
    mapping(uint256 => Proposal) private _proposals;
    mapping(uint256 => Vote) private _proposalVotes;
    DaoSettings internal _settings;
    bytes32 public constant BALLOT_TYPEHASH = keccak256("Ballot(uint256 proposalId,uint8 support)");

    /* 
        EVENTS
    */

    event ProposalCreated(
        uint256 proposalId,
        address proposer,
        uint256 startBlock,
        uint256 endBlock,
        string description,
        address tokenAddress
    );
    event ProposalCanceled(uint256 proposalId);
    event ProposalExecuted(uint256 proposalId);
    event VoteCast(address indexed voter, uint256 proposalId, uint8 support, uint256 weight, string reason);
    
    /* MODIFIERS  */
    modifier onlyWhenVotingEnabled() {
        require(_settings.isVotingEnabled == true, "DAO: Voting is disabled");
        _;
    }

    modifier onlyWallet() {
        require(msg.sender == address(this));
        _;
    }

    /* BODY */

    constructor(DaoSettings memory settings) EIP712(settings.name, version()) MultiOwnable(_msgSender()) {
        _settings = settings;
        if(settings.owner != _msgSender()){
            addOwner(settings.owner);
        }
    }

    function name() public view virtual returns (string memory) {
        return _settings.name;
    }

    function hasVoted(uint256 proposalId, address account) public view virtual returns (bool) {
        return _proposalVotes[proposalId].hasVoted[account];
    }

    function votingDelay() public view returns (uint256) {
        return _settings.initialVotingDelay;
    }

    function isVotingEnabled() public view returns (bool) {
        return _settings.isVotingEnabled;
    }

    function onlyOwnerProposals() public view returns (bool) {
        return _settings.onlyOwnerProposals;
    }

    function votingPeriod() public view returns (uint256) {
        return _settings.initialVotingPeriod;
    }

    function quorum() public view returns (uint256) {
        return _settings.quorum;
    }

    function tokenQuorum(address tokenAddress)
        public
        view
        returns (uint256)
    {
        require(isManagedToken(tokenAddress), "DAO: Token is not part of DAO");
        MosaicoERC20v1 token = MosaicoERC20v1(tokenAddress);
        return (quorum() * token.totalSupply()) / 100;
    }

    function state(uint256 proposalId)
        public
        view
        virtual
        returns (ProposalState)
    {
        Proposal memory proposal = _proposals[proposalId];

        if (proposal.executed) {
            return ProposalState.Executed;
        } else if (proposal.canceled) {
            return ProposalState.Canceled;
        } else if (proposalSnapshot(proposalId) > block.number) {
            return ProposalState.Pending;
        } else if (proposalDeadline(proposalId) > block.number) {
            return ProposalState.Active;
        } else if (proposalSnapshot(proposalId) > 0 && proposalDeadline(proposalId) <= block.number) {
            return
                _quorumReached(proposalId, proposal.tokenAddress) && _voteSucceeded(proposalId)
                    ? ProposalState.Succeeded
                    : ProposalState.Defeated;
        } else {
            revert("Governor: unknown proposal id");
        }
    }

    function proposalSnapshot(uint256 proposalId) public view virtual returns (uint256) {
        return _proposals[proposalId].voteStart.getDeadline();
    }

    function proposalDeadline(uint256 proposalId) public view virtual returns (uint256) {
        return _proposals[proposalId].voteEnd.getDeadline();
    }

    function version() public view virtual returns (string memory) {
        return "1";
    }

    function hashProposal(
        string memory description,
        address target
    ) public view virtual returns (uint256) {
        return
            uint256(
                keccak256(
                    abi.encode(keccak256(bytes(description)), target)
                )
            );
    }

    function propose(string memory description, address tokenAddress) onlyWhenVotingEnabled whenNotPaused public virtual returns (uint256) {
        uint256 proposalId = hashProposal(
            description,
            tokenAddress
        );

        require(_settings.onlyOwnerProposals == false || isOwner(_msgSender()) == true, "DAO: Unauthorized vote");
        require(isManagedToken(tokenAddress) == true && isVotingToken(tokenAddress) == true, "DAO: Token is not part of DAO");

        Proposal storage proposal = _proposals[proposalId];
        require(
            proposal.voteStart.isUnset(),
            "DAO: proposal already exists"
        );

        uint64 snapshot = block.number.toUint64() + votingDelay().toUint64();
        uint64 deadline = snapshot + votingPeriod().toUint64();

        proposal.voteStart.setDeadline(snapshot);
        proposal.voteEnd.setDeadline(deadline);
        proposal.tokenAddress = tokenAddress;

        emit ProposalCreated(
            proposalId,
            _msgSender(),
            snapshot,
            deadline,
            description,
            tokenAddress
        );

        return proposalId;
    }

    function castVote(uint256 proposalId, uint8 support)
        public
        virtual
        returns (uint256)
    {
        address voter = _msgSender();
        return _castVote(proposalId, voter, support, "");
    }

    function _castVote(uint256 proposalId, address account, uint8 support, string memory reason) whenNotPaused internal virtual returns (uint256) {
        Proposal storage proposal = _proposals[proposalId];
        require(
            state(proposalId) == ProposalState.Active,
            "Governor: vote not currently active"
        );
        uint256 weight = getWeight(account, proposal.tokenAddress);
        require(weight > 0, "Not sufficient weight");
        _countVote(proposalId, account, support, weight);
        emit VoteCast(account, proposalId, support, weight, reason);
        return weight;
    }

    function proposalVotes(uint256 proposalId)
        public
        view
        virtual
        returns (
            uint256 againstVotes,
            uint256 forVotes,
            uint256 abstainVotes
        )
    {
        Vote storage proposalvote = _proposalVotes[proposalId];
        return (proposalvote.againstVotes, proposalvote.forVotes, proposalvote.abstainVotes);
    }

    function _quorumReached(uint256 proposalId, address tokenAddress) internal view virtual returns (bool) {
        Vote storage proposalvote = _proposalVotes[proposalId];
        return tokenQuorum(tokenAddress) <= proposalvote.forVotes + proposalvote.abstainVotes;
    }

    function _voteSucceeded(uint256 proposalId) internal view virtual returns (bool) {
        Vote storage proposalvote = _proposalVotes[proposalId];

        return proposalvote.forVotes > proposalvote.againstVotes;
    }

    function _countVote(
        uint256 proposalId,
        address account,
        uint8 support,
        uint256 weight
    ) internal virtual {
        Vote storage proposalvote = _proposalVotes[proposalId];

        require(!proposalvote.hasVoted[account], "GovernorVotingSimple: vote already cast");
        proposalvote.hasVoted[account] = true;

        if (support == uint8(VoteResult.Against)) {
            proposalvote.againstVotes += weight;
        } else if (support == uint8(VoteResult.For)) {
            proposalvote.forVotes += weight;
        } else if (support == uint8(VoteResult.Abstain)) {
            proposalvote.abstainVotes += weight;
        } else {
            revert("GovernorVotingSimple: invalid value for enum VoteType");
        }
    }

    function addToken(address token, bool isVoting) public onlyOwner {
        super._addToken(token, isVoting);
    }

    function mint(address tokenAddress, uint256 amount) public onlyOwner {
        super._mint(tokenAddress, amount, _msgSender());
    }

    function burn(address tokenAddress, uint256 amount) public onlyOwner {
        super._burn(tokenAddress, amount);
    }

    function pauseToken(address tokenAddress) public onlyOwner {
        super._pause(tokenAddress);
    }

    function unpauseToken(address tokenAddress) public onlyOwner {
        super._unpause(tokenAddress);
    }

    function addOwner(address newOwner) public onlyOwner {
        super._setOwner(newOwner);
    }

    function removeOwner(address owner) public onlyOwner {
        super._removeOwner(owner);
    }

    function pause() public onlyOwner {
        super._pause();
    }

    function unpause() public onlyOwner {
        super._unpause();
    }

    function create(MosaicoStructs.ERC20Settings memory contractSettings) public onlyOwner whenNotPaused returns(address){
        return super._create(contractSettings);
    }
}
