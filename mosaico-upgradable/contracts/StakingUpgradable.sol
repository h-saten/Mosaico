// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "./base/IERC1155.sol";
import "./base/IERC20.sol";
import "./base/SafeMath.sol";
import "./base/AddressUpgradable.sol";
import "./base/Initializable.sol";
import "./base/ContextUpgradeable.sol";
import "./base/OwnableUpgradeable.sol";
import "./base/PausableUpgradeable.sol";
import "./base/CountersUpgradeable.sol";
import "./base/ReentrancyGuardUpgradeable.sol";
import "@opengsn/contracts/src/BaseRelayRecipient.sol";

struct StakingSettings {
    // in tokens, max amount of tokens staker can receive in one cycle
    uint256 maxRewardPerStaker;
    // In DAYS, how often the reward is generated
    uint256 rewardCycle;
    // address of the smart contract which is staked
    address stakingToken;
    // in tokens, how many tokens minimum user can stake
    uint256 minimumStakingAmount;
    // address of the smart contract which is minted/burned
    address tmos;
    // id of tmos tokens for this specific staking
    uint256 tokendaId;
}

struct Stake {
    address staker;
    uint256 amount;
    uint256 since;
    address token;
    bool active;
}

contract StakeCalculator {
    using SafeMath for uint256;
    mapping(address => uint256) public balances;
    uint256 private _dedicatedReward;
    uint256 private _stakeholderCount;
    uint256 private _maxReward;
    uint256 private _totalStaked;
    uint256 private _calculationBonus;

    constructor(
        uint256 reward,
        uint256 maxReward,
        uint256 calculationBonus
    ) {
        _dedicatedReward = reward;
        _maxReward = maxReward;
        _calculationBonus = calculationBonus;
    }

    function add(
        address wallet,
        uint256 amount,
        uint256 stakingDays
    ) public {
        if (balances[wallet] == 0 && amount > 0) {
            _stakeholderCount++;
        }
        uint256 dailyBonus = amount.mul(_calculationBonus).div(10**4);
        uint256 amountWithBonus = amount.add(dailyBonus.mul(stakingDays));
        balances[wallet] = balances[wallet].add(amountWithBonus);
        _totalStaked = _totalStaked.add(amountWithBonus);
    }

    function count() public view returns (uint256) {
        return _stakeholderCount;
    }

    function total() public view returns (uint256) {
        return _totalStaked;
    }

    function balanceOf(address wallet) public view returns (uint256) {
        return balances[wallet];
    }

    function estimateReward(address wallet, uint256 totalPool)
        public
        view
        returns (uint256)
    {
        uint256 reward = totalPool.mul(balanceOf(wallet).div(total()));
        reward = reward.add(reward);
        if (reward > _maxReward) {
            return _maxReward;
        }

        return reward;
    }
}

contract StakingUpgradable is
    Initializable,
    ContextUpgradeable,
    OwnableUpgradeable,
    PausableUpgradeable,
    ReentrancyGuardUpgradeable,
    BaseRelayRecipient
{
    using SafeMath for uint256;
    using AddressUpgradeable for address;
    using CountersUpgradeable for CountersUpgradeable.Counter;

    StakingSettings private _settings;
    //user balance in TMOS
    mapping(address => uint256) private _balances;
    // USER => LIST OF STAKES
    mapping(address => Stake[]) private _userStakes;
    // ALL STAKES
    Stake[] private _tokenStakes;
    // total amount of frozen staking tokens
    uint256 private _totalStaked;
    // total amount of payed rewards
    uint256 private _payedRewards;
    // timestamp of last payment
    uint256 private _lastPayment;
    //Reward Token (USDT, USDC) => USER => AMOUNT
    mapping(address => mapping(address => uint256)) private _claimable;
    uint256 _calculationBonus;

    event Staked(address indexed staker, uint256 amount, uint256 timestamp);
    event Distributed(uint256 amount, uint256 stakeholderCount);

    function initialize(StakingSettings memory settings_) public initializer {
        __Context_init();
        __Ownable_init();
        __Pausable_init();
        __ReentrancyGuard_init();
        _settings = settings_;
        _calculationBonus = 1 * 10**2;
        _setTrustedForwarder(0xdA78a11FD57aF7be2eDD804840eA7f4c2A38801d);
    }

    function stake(uint256 _amount) external nonReentrant {
        IERC20 token = IERC20(_settings.stakingToken);
        require(
            token.allowance(_msgSender(), address(this)) >= _amount,
            "Staking: Cannot stake more than you own"
        );
        require(
            _amount >= _settings.minimumStakingAmount,
            "Staking: Cannot stake less than the minimum amount"
        );
        token.transferFrom(_msgSender(), address(this), _amount);
        _mintTMOS(_msgSender(), _amount);
        Stake memory userStake = Stake(
            _msgSender(),
            _amount,
            block.timestamp,
            _settings.stakingToken,
            true
        );
        _userStakes[_msgSender()].push(userStake);
        _tokenStakes.push(userStake);
        _totalStaked = _totalStaked.add(_amount);
        _balances[_msgSender()] = _balances[_msgSender()].add(_amount);
        emit Staked(_msgSender(), _amount, block.timestamp);
    }

    function distribute(address rewardTokenAddress, uint256 amount)
        public
        nonReentrant
        returns (uint256)
    {
        require(
            rewardTokenAddress != address(0),
            "Invalid reward token address"
        );
        require(amount >= 0, "Reward cannot be 0");
        require(
            _getDaysSinceLastPayment() >= _settings.rewardCycle,
            "Staking: Reward cycle not finished yet"
        );
        IERC20 rewardToken = IERC20(rewardTokenAddress);
        require(
            rewardToken.allowance(_msgSender(), address(this)) >= amount,
            "Staking: Insufficient funds"
        );
        StakeCalculator calculator = new StakeCalculator(
            amount,
            _settings.maxRewardPerStaker,
            _calculationBonus
        );

        for (uint256 s = 0; s < _tokenStakes.length; s += 1) {
            if (
                _tokenStakes[s].staker != address(0) &&
                _tokenStakes[s].active != false
            ) {
                uint256 stakedDays = _getStakedDays(_tokenStakes[s].since);
                if (stakedDays >= _settings.rewardCycle - 1) {
                    calculator.add(
                        _tokenStakes[s].staker,
                        _tokenStakes[s].amount,
                        stakedDays
                    );
                }
            }
        }

        for (uint256 s = 0; s < _tokenStakes.length; s += 1) {
            if (
                _tokenStakes[s].staker != address(0) &&
                _tokenStakes[s].active != false
            ) {
                uint256 reward = calculator.estimateReward(
                    _tokenStakes[s].staker,
                    amount
                );
                if (reward > 0) {
                    _claimable[rewardTokenAddress][
                        _tokenStakes[s].staker
                    ] = _claimable[rewardTokenAddress][_tokenStakes[s].staker]
                        .add(reward);
                }
            }
        }

        if (calculator.count() > 0) {
            rewardToken.transferFrom(_msgSender(), address(this), amount);
        }

        _payedRewards = _payedRewards.add(amount);
        _lastPayment = block.timestamp;
        emit Distributed(amount, calculator.count());
        return amount;
    }

    function _mintTMOS(address wallet, uint256 _amount) internal {
        IERC1155 tmos = IERC1155(_settings.tmos);
        tmos.mint(wallet, _settings.tokendaId, _amount);
    }

    function _burnTMOS(address wallet, uint256 _amount) internal {
        IERC1155 tmos = IERC1155(_settings.tmos);
        tmos.burn(wallet, _settings.tokendaId, _amount);
    }

    function setMaxReward(uint256 reward) public onlyOwner {
        _settings.maxRewardPerStaker = reward;
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

    function rewardCycle() public view returns (uint256) {
        return _settings.rewardCycle;
    }

    function totalStaked() public view returns (uint256) {
        return _totalStaked;
    }

    function totalReward() public view returns (uint256) {
        return _payedRewards;
    }

    function balanceOf(address _staker) public view returns (uint256) {
        return _balances[_staker];
    }

    function _getDaysSinceLastPayment() internal view returns (uint256) {
        return (block.timestamp - _lastPayment) / 60 / 60 / 24;
    }

    function _getStakedDays(uint256 stakedAt) internal view returns (uint256) {
        return (block.timestamp - stakedAt) / 60 / 60 / 24;
    }

    // get claimable balance of the token (USDT/USDC) for the given user (wallet)
    function claimableBalanceOf(address token, address wallet)
        public
        view
        returns (uint256)
    {
        return _claimable[token][wallet];
    }

    function claim(address tokenAddress) public nonReentrant returns (uint256) {
        require(tokenAddress != address(0), "Invalid token");
        uint256 withdrawAmount = claimableBalanceOf(tokenAddress, _msgSender());
        require(withdrawAmount > 0, "Nothing to withdraw");
        IERC20 token = IERC20(tokenAddress);
        require(
            token.balanceOf(address(this)) >= withdrawAmount,
            "Insufficient funds on smart contract. Please try again later"
        );
        token.transfer(_msgSender(), withdrawAmount);
        delete _claimable[tokenAddress][_msgSender()];
        return withdrawAmount;
    }

    function withdraw() public nonReentrant {
        IERC20 _stakingToken = IERC20(_settings.stakingToken);
        IERC1155 tmos = IERC1155(_settings.tmos);
        uint256 totalStake = 0;
        for (uint256 s = 0; s < _userStakes[_msgSender()].length; s += 1) {
            totalStake = totalStake.add(_userStakes[_msgSender()][s].amount);
        }
        require(totalStake > 0, "Nothing to withdraw");
        require(
            tmos.allowance(_msgSender(), address(this), _settings.tokendaId) >=
                totalStake,
            "You have to approve TMOS first"
        );
        require(
            _stakingToken.balanceOf(address(this)) >= totalStake,
            "Insufficient funds on smart contract. Please try again later"
        );
        delete _userStakes[_msgSender()];
        _totalStaked = _totalStaked.sub(totalStake);
        for (uint256 s = 0; s < _tokenStakes.length; s += 1) {
            if (_tokenStakes[s].staker == _msgSender()) {
                delete _tokenStakes[s];
            }
        }
        _balances[_msgSender()] = _balances[_msgSender()].sub(totalStake);
        _burnTMOS(_msgSender(), totalStake);
        _stakingToken.transfer(_msgSender(), totalStake);
    }

    function stakes(address wallet) public view returns (Stake[] memory) {
        return _userStakes[wallet];
    }

    function stakingToken() public view returns (address) {
        return _settings.stakingToken;
    }

    function calculationBonus() public view returns (uint256) {
        return _calculationBonus;
    }

    function setCalculationBonus(uint256 bonus) public onlyOwner {
        require(bonus > 0, "Too little bonus");
        _calculationBonus = bonus;
    }

    function _msgSender()
        internal
        view
        override(BaseRelayRecipient, ContextUpgradeable)
        returns (address ret)
    {
        return BaseRelayRecipient._msgSender();
    }

    function _msgData()
        internal
        view
        override(BaseRelayRecipient, ContextUpgradeable)
        returns (bytes calldata ret)
    {
        return BaseRelayRecipient._msgData();
    }

    function versionRecipient() external pure override returns (string memory) {
        return "2.2.6";
    }

    function setTrustedForwarder(address _forwarder) public onlyOwner {
        _setTrustedForwarder(_forwarder);
    }
}
