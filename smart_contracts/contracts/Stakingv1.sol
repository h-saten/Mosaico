// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "../node_modules/@openzeppelin/contracts/token/ERC20/IERC20.sol";
import "../node_modules/@openzeppelin/contracts/utils/math/SafeMath.sol";
import "../node_modules/@openzeppelin/contracts/utils/Context.sol";
import "../node_modules/@openzeppelin/contracts/security/Pausable.sol";
import "./MultiOwnable.sol";
import "../node_modules/@openzeppelin/contracts/security/ReentrancyGuard.sol";

contract Stakingv1 is Context, Pausable, MultiOwnable, ReentrancyGuard {
    using SafeMath for uint256;

    struct StakingSettings {
        uint256 rewardPerToken;
        uint rewardPeriodInDays;
        address stakingToken;
        address rewardToken;
        uint256 minimumStakingAmount;
        bool userShouldDeclarePeriod;
        uint256 punishmentFee;
        address platformWallet;
        uint256 withdrawThreshold;
    }

    struct Stake {
        address staker;
        uint256 amount;
        uint256 since;
        uint256 declaredPeriod;
    }

    struct Stakeholder {
        address staker;
        uint256 lastClaim;
        Stake[] stakes;
    }

    struct StakingSummary {
        uint256 total_amount;
        uint256 total_reward_amount;
        uint256 total_punishment;
        Stake[] stakes;
    }

    event Staked(address indexed staker, uint256 amount, uint256 index, uint256 timestamp);
    uint256 internal _totalStaked;
    uint256 internal _totalReward;
    Stakeholder[] internal stakeholders;

    /**
    * @notice 
    * stakes is used to keep track of the INDEX for the stakers in the stakes array
     */
    mapping(address => uint256) internal stakes;

    StakingSettings internal _settings;

    constructor(StakingSettings memory settings) MultiOwnable(_msgSender()) {
        _settings = settings;
        require(_settings.punishmentFee >= 0 && _settings.punishmentFee <= 1, "Staking: Invalid punishment fee. Should be between 0 and 1");
        stakeholders.push(); // empty first array value for not make misunderstading in checking if index exist 
    }

    function _addStakeholder(address staker) internal returns (uint256){
        stakeholders.push();
        uint256 userIndex = stakeholders.length - 1;
        stakeholders[userIndex].staker = staker;
        stakes[staker] = userIndex;
        return userIndex; 
    }

    /**
    * @notice
    * _Stake is used to make a stake for an sender. It will remove the amount staked from the stakers account and place those tokens inside a stake container
    * StakeID 
    */
    function _stake(uint256 _amount, uint256 period) internal returns(uint256) {
        // Simple check so that user does not stake 0 
        require(_amount > 0, "Staking: Cannot stake nothing");
        require(period > 0 || _settings.userShouldDeclarePeriod == false, "Staking: Should declare staking period");
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
        stakeholders[index].stakes.push(Stake(_msgSender(), _amount, block.timestamp, period));
        _totalStaked += _amount;
        return index;
    }

    function setDailyReward(uint256 reward) public onlyOwner { 
        _settings.rewardPerToken = reward;
    }

    function setRewardToken(address token) public onlyOwner {
        _settings.rewardToken = token;
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

    function stake(uint256 _amount, uint256 period) external nonReentrant {
      IERC20 token = IERC20(_settings.stakingToken);
      require(_amount <= token.balanceOf(_msgSender()), "Staking: Cannot stake more than you own");
      require(_amount >= _settings.minimumStakingAmount, "Staking: Cannot stake less than the minimum amount");
      token.transferFrom(_msgSender(), address(this), _amount);
      uint256 stakerIndex = _stake(_amount, period);
      emit Staked(_msgSender(), _amount, stakerIndex, block.timestamp);
    }

    /**
    * @notice
    * calculateStakeReward is used to calculate how much a user should be rewarded for their stakes
    * and the duration the stake has been active
    */
    function _calculateStakeReward(Stake memory _current_stake, uint256 daysStaked) internal view returns(uint256){
        // First calculate how long the stake has been active
        // Use current seconds since epoch - the seconds since epoch the stake was made
        // The output will be duration in SECONDS ,
        // We will reward the user 0.1% per Hour So thats 0.1% per 3600 seconds
        // the alghoritm is  seconds = block.timestamp - stake seconds (block.timestap - _stake.since)
        // hours = Seconds / 3600 (seconds /3600) 3600 is an variable in Solidity names hours
        // we then multiply each token by the hours staked , then divide by the rewardPerHour rate
         // amount of days person staked tokens
        return (daysStaked / _settings.rewardPeriodInDays) * _current_stake.amount * _settings.rewardPerToken;
    }

    /**
    * @notice
    * withdrawStake takes in an amount and a index of the stake and will remove tokens from that stake
    * Notice index of the stake is the users stake counter, starting at 0 for the first stake
    * Will return the amount to MINT onto the acount
    * Will also calculateStakeReward and reset timer
    */
    function _estimateRewards(uint256 index) internal view returns(uint256, uint256){
        // Grab user_index which is the index to use to grab the Stake[]
        uint256 user_index = stakes[_msgSender()];
        Stake memory current_stake = stakeholders[user_index].stakes[index];
        // Calculate available Reward first before we start modifying data
        uint256 daysStaked = _getStakedDays(current_stake);
        uint256 stakerReward = _calculateStakeReward(current_stake, daysStaked);
        require(stakerReward >= 0, "Staking: Cannot withdraw more than you have earned");
        if(_settings.userShouldDeclarePeriod == true && current_stake.declaredPeriod > daysStaked) {
            return (stakerReward - (stakerReward * _settings.punishmentFee), stakerReward * _settings.punishmentFee);
        }
        return (stakerReward, 0);
    }
    
    /**
    * @notice checks if user can withdraw reward today
     */
    function canClaim() public view returns (bool) {
        return ((block.timestamp - stakeholders[stakes[_msgSender()]].lastClaim) / 60 / 60 / 24) >= _settings.withdrawThreshold;
    }

    /**
    * @notice withdrawStake is used to withdraw stakes from the account holder
     */
    function claimReward() public nonReentrant returns(uint256 amount) {
        require(canClaim() == true, "Staking: Cannot withdraw today");

        StakingSummary memory summary = hasStake(_msgSender());
        uint256 withdrawAmount = 0;
        uint256 feeAmount = 0;
        uint256 user_index = stakes[_msgSender()];

        for (uint256 s = 0; s < summary.stakes.length; s += 1){
            (uint256 reward, uint256 punishmentFee) = _estimateRewards(s);
            withdrawAmount += reward;
            feeAmount += punishmentFee;
            delete stakeholders[user_index].stakes[s];
        }

        require(withdrawAmount > 0, "Staking: Nothing to claim");

        stakeholders[stakes[_msgSender()]].lastClaim = block.timestamp;
        IERC20 token = IERC20(_settings.rewardToken);
        // Return staked tokens to user
        token.transfer(_msgSender(), withdrawAmount);
        // pay punishment fee if withdraw earlier
        if(feeAmount > 0) {
            token.transfer(_settings.platformWallet, feeAmount);
        }
        _totalReward += withdrawAmount;
        return withdrawAmount;
    }

     /**
    * @notice
    * withdraw
    */
    function withdraw() public {
        StakingSummary memory summary = hasStake(_msgSender());
        require(summary.total_amount > 0, "Staking: Nothing to withdraw");
        IERC20 token = IERC20(_settings.stakingToken);
        token.transfer(_msgSender(), summary.total_amount);
        uint256 user_index = stakes[_msgSender()];
        for (uint256 s = 0; s < stakeholders[user_index].stakes.length; s += 1){
            delete stakeholders[user_index].stakes[s];
        }
        _totalStaked -= summary.total_amount;
    }

    /**
    * @notice
    * returns amount of days stake exists
    */
    function _getStakedDays(Stake memory st) internal view returns(uint256) {
        return (block.timestamp - st.since) / 60 / 60 / 24;
    }

    /**
    * @notice
    * hasStake is used to check if a account has stakes and the total amount along with all the seperate stakes
    */
    function hasStake(address _staker) public view returns(StakingSummary memory){
        // Keep a summary in memory since we need to calculate this
        StakingSummary memory summary = StakingSummary(0, 0, 0, stakeholders[stakes[_staker]].stakes);
        // Itterate all stakes and grab amount of stakes
        for (uint256 s = 0; s < summary.stakes.length; s += 1) {
            (uint256 reward, uint256 punishmentFee) = _estimateRewards(s);
            summary.total_reward_amount += reward;
            summary.total_amount += summary.stakes[s].amount;
            summary.total_punishment += punishmentFee;
        }
        return summary;
    }
    
}