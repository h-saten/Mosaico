// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "./StakingUpgradable.sol";

contract RahimCoinDivident is Initializable, ContextUpgradeable, OwnableUpgradeable, PausableUpgradeable, ReentrancyGuardUpgradeable {
    using SafeMath for uint256;
    using AddressUpgradeable for address;
    using CountersUpgradeable for CountersUpgradeable.Counter;

    //user balance in RC
    mapping(address => uint256) private _balances;
    uint256 _rate;
    // total amount of frozen staking tokens
    uint256 private _totalStaked;
    // total amount of payed rewards
    uint256 private _payedRewards;
    address private _rewardTokenAddress;
    // address of the smart contract which is staked
    address private _stakingToken;
    // in tokens, how many tokens minimum user can stake
    uint256 private _minimumStakingAmount;

    event Staked(address indexed staker, uint256 amount, uint256 timestamp);
    event Distributed(uint256 amount, uint256 stakeholderCount);

    function initialize() public initializer {
        __Context_init();
        __Ownable_init();
        __Pausable_init();
        __ReentrancyGuard_init();
        _rate = 11.5 * 10 ** 2;
        _rewardTokenAddress = 0xc2132D05D31c914a87C6611C10748AEb04B58e8F;
        _stakingToken = 0x7734A215960c7F5c2f1F6bfBAF90C1EC1b148c31;
        _minimumStakingAmount = 1;
    }

    function distribute(address rewardTokenAddress, uint256 amount) public nonReentrant
    {
        require(_rewardTokenAddress != address(0), "Invalid reward token address");
        require(amount >= 0, "Reward cannot be 0");
        IERC20 token = IERC20(_rewardTokenAddress);
        require(token.allowance(_msgSender(), address(this)) >= amount, "Staking: Insufficient funds");
        token.transferFrom(_msgSender(), address(this), amount);
        _payedRewards = _payedRewards.add(amount);
        emit Distributed(amount, amount);
    }

    function stake(uint256 _amount) external whenNotPaused nonReentrant {
      IERC20 token = IERC20(_stakingToken);
      require(token.allowance(_msgSender(),  address(this)) >= _amount, "Staking: Cannot stake more than you own");
      require(_amount >= _minimumStakingAmount, "Staking: Cannot stake less than the minimum amount");
      token.transferFrom(_msgSender(), address(this), _amount);
      _totalStaked = _totalStaked.add(_amount);
      _balances[_msgSender()] = _balances[_msgSender()].add(_amount);
      emit Staked(_msgSender(), _amount, block.timestamp);
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

    function rewardRate() public view returns (uint256) {
        return _rate;
    }

    function setRewardRate(uint256 rate_) public onlyOwner {
        require(rate_ > 0, "Rate cannot be 0");
        _rate = rate_;
    }

    // get claimable balance of the token (USDT/USDC) for the given user (wallet)
    function claimableBalanceOf(address token, address wallet) public view returns(uint256) {
        uint256 balance = _balances[wallet];
        IERC20 rewardToken_ = IERC20(_rewardTokenAddress);
        uint256 contractBalance = rewardToken_.balanceOf(address(this));
        if(contractBalance == 0)
            return 0;
        uint256 reward = balance.mul(_rate).div(10 ** 4);
        return reward;
    }

    function claim(address tokenAddress) whenNotPaused public nonReentrant {
        uint256 withdrawAmount = claimableBalanceOf(tokenAddress, _msgSender());
        require(withdrawAmount > 0, "Nothing to withdraw");
        IERC20 token = IERC20(_rewardTokenAddress);
        require(token.balanceOf(address(this)) >= withdrawAmount, "Insufficient funds on smart contract. Please try again later");
        token.transfer(_msgSender(), withdrawAmount);
    }

    function withdraw() whenNotPaused public nonReentrant {
        IERC20 _stakingToken = IERC20(_stakingToken);
        uint256 balance = _balances[_msgSender()];
        require(balance > 0, "Nothing to withdraw");
        require(_stakingToken.balanceOf(address(this)) >= balance, "Insufficient funds on smart contract. Please try again later");
        _balances[_msgSender()] = _balances[_msgSender()].sub(balance);
        _stakingToken.transfer(_msgSender(), balance);
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
}
