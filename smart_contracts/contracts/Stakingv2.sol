// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "../node_modules/@openzeppelin/contracts/token/ERC20/IERC20.sol";
import "../node_modules/@openzeppelin/contracts/utils/math/SafeMath.sol";
import "../node_modules/@openzeppelin/contracts/utils/Context.sol";
import "../node_modules/@openzeppelin/contracts/security/Pausable.sol";
import "./MultiOwnable.sol";
import "../node_modules/@openzeppelin/contracts/security/ReentrancyGuard.sol";

contract Stakingv2 is Context, Pausable, MultiOwnable, ReentrancyGuard {
    using SafeMath for uint256;

    struct StakingSettings {
        // in tokens, max amount of tokens staker can receive in one cycle
        uint256 maxRewardPerStaker;
        // In DAYS, how often the reward is generated
        uint rewardCycle;
        // address of the smart contract which is staked
        address stakingToken;
        // address of the smart contract which is rewarded
        address rewardToken;
        // in tokens, how many tokens minimum user can stake
        uint256 minimumStakingAmount;
        // in days, how many days user should wait till he can withdraw
        uint256 withdrawThreshold;
        // platform wallet where money will be sent
        address platformWallet;
    }

    struct Stake {
        address staker;
        uint256 amount;
        uint256 since;
    }

    struct Stakeholder {
        address staker;
        uint256 lastClaim;
        Stake[] stakes;
    }

    struct StakeholderBalance {
        address staker;
        uint256 balance;
    }

    event Staked(address indexed staker, uint256 amount, uint256 index, uint256 timestamp);
    event Distributed(uint256 amount, uint256 stakeholderCount);
    uint256 internal _totalStaked;
    uint256 internal _totalReward;
    Stakeholder[] internal stakeholders;
    mapping(address => uint256) internal _balances;
    uint256 internal _lastRewardPayment;
    /**
    * @notice 
    * stakes is used to keep track of the INDEX for the stakers in the stakes array
     */
    mapping(address => uint256) internal stakes;

    StakingSettings internal _settings;

    constructor(StakingSettings memory settings) MultiOwnable(_msgSender()) {
        _settings = settings;
        stakeholders.push(); // empty first array value for not make misunderstading in checking if index exist 
    }

    function _addStakeholder(address staker) internal returns (uint256){
        stakeholders.push();
        uint256 userIndex = stakeholders.length - 1;
        stakeholders[userIndex].staker = staker;
        stakes[staker] = userIndex;
        _balances[staker] = 0;
        return userIndex; 
    }

    /**
    * @notice
    * _Stake is used to make a stake for an sender. It will remove the amount staked from the stakers account and place those tokens inside a stake container
    * StakeID 
    */
    function _stake(uint256 _amount) internal returns(uint256) {
        // Simple check so that user does not stake 0 
        require(_amount > 0, "Staking: Cannot stake nothing");
        // Mappings in solidity creates all values, but empty, so we can just check the address
        uint256 index = stakes[_msgSender()];
        // See if the staker already has a staked index or if its the first time
        if(index == 0){
            // This stakeholder stakes for the first time
            // We need to add him to the stakeHolders and also map it into the Index of the stakes
            // The index returned will be the index of the stakeholder in the stakeholders array
            index = _addStakeholder(_msgSender());
        }

        // Use the index to push a new Stake
        // push a newly created Stake with the current block timestamp.
        // block.timestamp = timestamp of the current block in seconds since the epoch
        stakeholders[index].lastClaim = block.timestamp;
        stakeholders[index].stakes.push(Stake(_msgSender(), _amount, block.timestamp));
        _totalStaked += _amount;
        return index;
    }

    function setMaxReward(uint256 reward) public onlyOwner { 
        _settings.maxRewardPerStaker = reward;
    }

    function setRewardToken(address token) public onlyOwner {
        _settings.rewardToken = token;
    }

    function setRewardCycle(uint256 cycle) public onlyOwner {
        _settings.rewardCycle = cycle;
    }

    function setMinimumStakingAmount(uint256 amount) public onlyOwner {
        _settings.minimumStakingAmount = amount;
    }

    function minimumStakingAmount() public view returns (uint256) {
        return _settings.minimumStakingAmount;
    }

    function totalStaked() public view returns (uint256){
        return _totalStaked;
    }

    function totalReward() public view returns (uint256){
        return _totalReward;
    }

    function rewardCycle() public view returns (uint256){
        return _settings.rewardCycle;
    }

    function stake(uint256 _amount) external nonReentrant {
      IERC20 token = IERC20(_settings.stakingToken);
      require(_amount <= token.balanceOf(_msgSender()), "Staking: Cannot stake more than you own");
      require(_amount >= _settings.minimumStakingAmount, "Staking: Cannot stake less than the minimum amount");
      token.transferFrom(_msgSender(), address(this), _amount);
      uint256 stakerIndex = _stake(_amount);
      emit Staked(_msgSender(), _amount, stakerIndex, block.timestamp);
    }
    
    /**
    * @notice checks if user can withdraw reward today
     */
    function canClaim() public view returns (bool) {
        return ((block.timestamp - stakeholders[stakes[_msgSender()]].lastClaim) / 60 / 60 / 24) >= _settings.withdrawThreshold;
    }

    function _estimateReward(uint256 stakedAmount, uint256 totalStakedAmount, uint256 totalPool) internal view returns (uint256){ 
        uint256 reward = totalPool * (stakedAmount / totalStakedAmount);
        if(reward > _settings.maxRewardPerStaker){
            return _settings.maxRewardPerStaker;
        }
        return reward;
    }

    function distribute(uint256 amount) public nonReentrant onlyOwner returns(uint256) 
    {
        require(_getDaysSinceLastPayment() >= _settings.rewardCycle, "Staking: Reward cycle not finished yet");
        IERC20 token = IERC20(_settings.rewardToken);
        require(token.balanceOf(address(this)) >= amount, "Staking: Insufficient funds");
        uint256 totalValidStake = _totalStaked;
        StakeholderBalance[] memory sts = new StakeholderBalance[](stakeholders.length);
        for (uint256 s = 0; s < stakeholders.length; s += 1){
            if(stakeholders[s].staker != address(0)){
                uint256 stakedValue = 0;
                for (uint256 si = 0; si < stakeholders[s].stakes.length; si += 1){
                    uint256 stakedDays = _getStakedDays(stakeholders[s].stakes[si]);
                    if(stakedDays > 1) {
                        stakedValue += stakeholders[s].stakes[si].amount;
                    }
                    else {
                        totalValidStake -= stakeholders[s].stakes[si].amount;
                    }
                }
                sts[s] = StakeholderBalance(stakeholders[s].staker, stakedValue);
            }
        }
        if(totalValidStake > 0) {
            for (uint256 s = 0; s < sts.length; s += 1){
                if(sts[s].staker != address(0)) {
                    _balances[sts[s].staker] += _estimateReward(sts[s].balance, totalValidStake, amount);
                }
            }
        }
        _lastRewardPayment = block.timestamp;
        emit Distributed(amount, sts.length);
        return amount;
    }

    /**
    * @notice withdrawStake is used to withdraw stakes from the account holder
     */
    function claimReward(uint256 amount) public nonReentrant returns(uint256) {
        require(_balances[_msgSender()] >= amount, "Staking: Insufficient funds");
        require(canClaim() == true, "Staking: Cannot withdraw today");
        stakeholders[stakes[_msgSender()]].lastClaim = block.timestamp;
        IERC20 token = IERC20(_settings.rewardToken);
        // Return staked tokens to user
        token.transfer(_msgSender(), amount);
        _totalReward += amount;
        _balances[_msgSender()] -= amount;
        return amount;
    }

     /**
    * @notice
    * withdraw
    */
    function withdraw() public nonReentrant returns(uint256) {
        uint256 withdrawAmount = 0;
        uint256 user_index = stakes[_msgSender()];
        for (uint256 s = 0; s < stakeholders[user_index].stakes.length; s += 1){
            withdrawAmount += stakeholders[user_index].stakes[s].amount;
        }
        require(withdrawAmount > 0, "Staking: Nothing to withdraw");
        IERC20 token = IERC20(_settings.stakingToken);
        token.transfer(_msgSender(), withdrawAmount);
        _totalStaked -= withdrawAmount;
        _removeStakeholder(_msgSender());
        return withdrawAmount;
    }

    function _removeStakeholder(address stakeholder) internal returns(bool) {
        uint256 user_index = stakes[stakeholder];
        for (uint256 s = 0; s < stakeholders[user_index].stakes.length; s += 1){
            delete stakeholders[user_index].stakes[s];
        }
        delete stakeholders[user_index];
        delete stakes[stakeholder];
        delete _balances[stakeholder];
        return true;
    }

    /**
    * @notice
    * returns amount of days stake exists
    */
    function _getStakedDays(Stake memory st) internal view returns(uint256) {
        return (block.timestamp - st.since) / 60 / 60 / 24;
    }

    function _getDaysSinceLastPayment() internal view returns(uint256) {
        return (block.timestamp - _lastRewardPayment) / 60 / 60 / 24;
    }

    function balanceOf(address _staker) public view returns(uint256){
        return _balances[_staker];
    }

    function stakedValueOf(address _staker) public view returns(uint256) {
        uint256 balance = 0;
        uint256 user_index = stakes[_staker];
        for (uint256 si = 0; si < stakeholders[user_index].stakes.length; si += 1){
            balance += stakeholders[user_index].stakes[si].amount;
        }
        return balance;
    }

    function refund(uint256 amount) public onlyOwner nonReentrant returns(uint256) {
        require(_settings.platformWallet != address(0), "Staking: Platform wallet is unavailable");
        IERC20 token = IERC20(_settings.stakingToken);
        token.transfer(_settings.platformWallet, amount);
        return amount;
    }
    
}