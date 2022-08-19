// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "./StakingUpgradable.sol";
import "@opengsn/contracts/src/BaseRelayRecipient.sol";

contract MilkyDividend is Initializable, ContextUpgradeable, OwnableUpgradeable, PausableUpgradeable, ReentrancyGuardUpgradeable, BaseRelayRecipient  {
    using SafeMath for uint256;
    using AddressUpgradeable for address;
    using CountersUpgradeable for CountersUpgradeable.Counter;

    //user balance in MIC
    mapping(address => uint256) private _balances;
    //user balance in USDC -> USER
    mapping(address => mapping(address => uint256)) private _claimableBalances;
    // total amount of frozen staking tokens
    uint256 private _totalStaked;
    // total amount of payed rewards
    uint256 private _payedRewards;
    address private _rewardTokenAddress;
    // address of the smart contract which is staked
    address private _stakingToken;
    // in tokens, how many tokens minimum user can stake
    uint256 private _minimumStakingAmount;
    uint256 private _rewardTokenDecimals;
    // USER => LIST OF STAKES
    mapping(address => Stake[]) private _userStakes;
    // ALL STAKES
    Stake[] public _allStakes;
    uint256 public lastRewardDate;
    event Staked(address indexed staker, uint256 amount, uint256 timestamp);
    event Distributed(uint256 amount, uint256 stakeholderCount);

    function initialize() public initializer {
        __Context_init();
        __Ownable_init();
        __Pausable_init();
        __ReentrancyGuard_init();
        _setTrustedForwarder(0xdA78a11FD57aF7be2eDD804840eA7f4c2A38801d);
        _rewardTokenDecimals = 6;
        _rewardTokenAddress = 0x2791Bca1f2de4661ED88A30C99A7a9449Aa84174;
        _stakingToken = 0x296480179b8b79c9C9588b275E69d58f0d1BCa67;
        _minimumStakingAmount = 1;
    }

    function distribute(address rewardTokenAddress, uint256 amount) public nonReentrant
    {
        require(_rewardTokenAddress != address(0), "Invalid reward token address");
        require(amount >= 0, "Reward cannot be 0");
        IERC20 token = IERC20(_rewardTokenAddress);
        require(token.allowance(_msgSender(), address(this)) >= amount, "Staking: Insufficient funds");
        uint256 rewardDayDiff = (block.timestamp - lastRewardDate) / 60 / 60 / 24;
        require(rewardDayDiff >= 1, "MICDIV: You cannot distribute reward at the same day");
        token.transferFrom(_msgSender(), address(this), amount);
        _payedRewards = _payedRewards.add(amount);
        _recalculateClaimable(_rewardTokenAddress, amount, rewardDayDiff);
        lastRewardDate = block.timestamp;
        emit Distributed(amount, amount);
    }

    function stake(uint256 _amount) external whenNotPaused nonReentrant {
      IERC20 token = IERC20(_stakingToken);
      require(token.allowance(_msgSender(), address(this)) >= _amount, "Staking: Cannot stake more than you own");
      require(_amount >= _minimumStakingAmount, "Staking: Cannot stake less than the minimum amount");
      token.transferFrom(_msgSender(), address(this), _amount);
      _totalStaked = _totalStaked.add(_amount);
      Stake memory userStake = Stake(_msgSender(), _amount, block.timestamp, _stakingToken, true);
      _userStakes[_msgSender()].push(userStake);
      _allStakes.push(userStake);
      _balances[_msgSender()] = _balances[_msgSender()].add(_amount);
      emit Staked(_msgSender(), _amount, block.timestamp);
    }

    function totalStaked() public view returns (uint256){
        return _totalStaked;
    }

    function totalReward() public view returns (uint256){
        return _payedRewards;
    }

    function balanceOf(address _staker) public view returns(uint256){
        return _balances[_staker];
    }

    function minimumStakingAmount() public view returns (uint256) {
        return _minimumStakingAmount;
    }

    function stakingToken() public view returns (address){ 
        return _stakingToken;
    }

    function rewardToken() public view returns (address){ 
        return _rewardTokenAddress;
    }

    function setRewardToken(address rewardTokenAddress_, uint256 decimals_) public onlyOwner {
        require(rewardTokenAddress_ != address(0), "Invalid reward token address");
        require(decimals_ > 0, "Decimals cannot be 0");
        _rewardTokenAddress = rewardTokenAddress_;
        _rewardTokenDecimals = decimals_;
    }

    // get claimable balance of the token (USDT/USDC) for the given user (wallet)
    function claimableBalanceOf(address token, address wallet) public view returns(uint256) {
        return _claimableBalances[token][wallet];
    }

    function claim(address tokenAddress) whenNotPaused public nonReentrant returns(uint256) {
        uint256 withdrawAmount = claimableBalanceOf(tokenAddress, _msgSender());
        require(withdrawAmount > 0, "Nothing to withdraw");
        IERC20 token = IERC20(tokenAddress);
        require(token.balanceOf(address(this)) >= withdrawAmount, "Insufficient funds on smart contract. Please try again later");
        token.transfer(_msgSender(), withdrawAmount);
        _claimableBalances[tokenAddress][_msgSender()] = _claimableBalances[tokenAddress][_msgSender()].sub(withdrawAmount);
        return withdrawAmount;
    }

    function withdraw() whenNotPaused public nonReentrant {
        IERC20 stakingToken_ = IERC20(_stakingToken);
        uint256 balance = _balances[_msgSender()];
        require(balance > 0, "Nothing to withdraw");
        require(stakingToken_.balanceOf(address(this)) >= balance, "Insufficient funds on smart contract. Please try again later");
        delete _userStakes[_msgSender()];
        for (uint256 i = 0; i < _allStakes.length; i++){
            if(_allStakes[i].staker == _msgSender()){
                _totalStaked = _totalStaked.sub(_allStakes[i].amount);
                delete _allStakes[i];
            }
        }
        stakingToken_.transfer(_msgSender(), balance);
        _balances[_msgSender()] = _balances[_msgSender()].sub(balance);
    }

    function pause() public whenNotPaused onlyOwner {
        _pause();
    }

    function unpause() public whenPaused onlyOwner {
        _unpause();
    }

    function finalize() public onlyOwner {
        IERC20 token = IERC20(_rewardTokenAddress);
        uint256 withdrawAmount = token.balanceOf(address(this));
        require(withdrawAmount > 0, "Nothing to withdraw");
        token.transfer(_msgSender(), withdrawAmount);
    }

    function _msgSender() internal override(BaseRelayRecipient, ContextUpgradeable) view returns (address ret) {
        return BaseRelayRecipient._msgSender();
    }

    function _msgData() internal override(BaseRelayRecipient, ContextUpgradeable) view returns (bytes calldata ret) {
        return BaseRelayRecipient._msgData();
    }

    function versionRecipient() external override pure returns (string memory) {
        return "2.2.6";
    }

    function setTrustedForwarder(address _forwarder) public onlyOwner {
        _setTrustedForwarder(_forwarder);
    }

    function _recalculateClaimable(address rewardTokenAddress, uint256 amount, uint256 rewardDayDiff) internal {
        uint256 dailyPool = amount.div(rewardDayDiff);
        uint256 dailyRewardPerToken = dailyPool.div(_totalStaked.div(10**(18-_rewardTokenDecimals)));
        for (uint256 i = 0; i < _allStakes.length; i++) {
            Stake memory stake_ = _allStakes[i];
            if(stake_.active == true) {
                uint256 daysDiff = (block.timestamp - stake_.since) / 60 / 60 / 24;
                if(daysDiff > rewardDayDiff) {
                    daysDiff = rewardDayDiff;
                }
                uint256 reward = daysDiff.mul(stake_.amount).mul(dailyRewardPerToken);
                _claimableBalances[rewardTokenAddress][stake_.staker] = _claimableBalances[rewardTokenAddress][stake_.staker].add(reward);
            }
        }
    }
}
