// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;
import "./CrowdsaleBase.sol";
import "./SaferERC20.sol";
import "../node_modules/@openzeppelin/contracts/token/ERC20/ERC20.sol";
/**
 * @title DefaultCrowdsale
 * @dev This is an example of a fully fledged crowdsale.
 */
contract DefaultCrowdsalev1 is CrowdsaleBase {
    using SafeMath for uint256;
    using SaferERC20 for ERC20;

    address[] _supportedStablecoinsList;
    mapping(address => bool) _supportedStablecoins;
    mapping(address => mapping(address => uint256)) _stableCoinBalances;
    uint256 public stableCoinRate;
    uint256[] private stableCoinStageRates;

    constructor(
        address payable wallet,
        IERC20 token,
        uint256 numberOfStages_,
        uint256 softCapDenominator,
        address[] memory supportedStablecoins
    ) CrowdsaleBase(1, wallet, token, softCapDenominator, numberOfStages_) {
        stableCoinStageRates = new uint256[](numberOfStages_);
        for(uint index = 0; index < supportedStablecoins.length; index++){
            _supportedStablecoins[supportedStablecoins[index]] = true;
        }
        _supportedStablecoinsList = supportedStablecoins;
    }

    function startNextStage(StageSettings memory settings, uint256 stableCoinRate_) external onlyStageAdmins {
        super._startNextStage(settings);
        stableCoinRate = stableCoinRate_;
        stableCoinStageRates.push(stableCoinRate);
    }

    /**
     * This function allow exchange erc20 which has added rate is set
     * @param amount Amount of token to exchange
     */
    function exchangeTokens(address paymentToken, uint256 amount) public virtual nonReentrant saleInProgress {
        require(paymentToken != address(0), "Crowdsale: Payment Token is invalid");
        require(stableCoinRate > 0, "Crowdsale: Purchase unavailable");
        require(_supportedStablecoins[paymentToken] == true, "Crowdsale: Invalid token allowance");
        require(IERC20(paymentToken).allowance(_msgSender(), address(this)) >= amount, "Crowdsale: Invalid token allowance");
        address beneficiary = _msgSender();
        uint256 paymentCurrencyRate = calculateRate(paymentToken);
        uint256 crowdsaleTokensAmount = amount.div(paymentCurrencyRate).mul(10**18);
        require(_tokensSold.add(crowdsaleTokensAmount) <= hardCap, "Crowdsale: Hardcap exceeded");
        _addContributor(beneficiary);
        _preValidatePurchase(beneficiary, amount, crowdsaleTokensAmount);
        uint currentStageIndex = _nextStage - 1;
        // weiRaised is ommitted because token are purchasing by other currency than ether
        _contributions[beneficiary][currentStageIndex] = _contributions[beneficiary][currentStageIndex].add(crowdsaleTokensAmount);
        _tokensRaised[paymentToken] = _tokensRaised[paymentToken].add(amount);
        _stageTokensRaised[currentStageIndex][paymentToken] = _stageTokensRaised[currentStageIndex][paymentToken].add(amount);
        remainStageTokens = remainStageTokens.sub(crowdsaleTokensAmount);
        // Update investor balance - will be used to send tokens after sale end
        _investorTokenBalance[beneficiary] = _investorTokenBalance[beneficiary].add(crowdsaleTokensAmount);
        _tokensSold = _tokensSold.add(crowdsaleTokensAmount);
        // Save how much USDT tokens someone invested
        _stableCoinBalances[_msgSender()][paymentToken] = _stableCoinBalances[_msgSender()][paymentToken].add(amount);
        emit TokensPurchased(beneficiary, beneficiary, amount, crowdsaleTokensAmount);
        ERC20(paymentToken).safeTransferFrom(_msgSender(), amount);
    }

    function calculateRate(address paymentToken) internal view returns(uint256) {
        uint256 paymentTokenDecimals = uint256(ERC20(paymentToken).safeDecimals());
        require(18 - paymentTokenDecimals >= 0, "Crowdsale: Invalid decimals");
        return stableCoinRate.div((10**(18-paymentTokenDecimals)));
    }
    
    function refund() public virtual override saleEnded isContributor(_msgSender()) {
        super.refund();
        // WITHDRAW STABLECOINS
        for(uint index = 0; index < _supportedStablecoinsList.length; index++){
            uint256 stableCoinBalance = _stableCoinBalances[_msgSender()][_supportedStablecoinsList[index]];
            if (stableCoinBalance > 0) {
                ERC20(_supportedStablecoinsList[index]).safeTransfer(_msgSender(), stableCoinBalance);
                emit StableCoinInvestmentsReturn(_supportedStablecoinsList[index], _msgSender(), stableCoinBalance);
            }
        }
    }

    function withdrawFunds() public virtual override saleEnded softCapReached {
        super.withdrawFunds();
        // WITHDRAW STABLECOINS
        for(uint index = 0; index < _supportedStablecoinsList.length; index++){
            address stableCoinAddress = _supportedStablecoinsList[index];
            uint256 stableCoinBalance = IERC20(stableCoinAddress).balanceOf(address(this));
            if (stableCoinBalance > 0) {
                ERC20(stableCoinAddress).safeTransfer(_wallet, stableCoinBalance);
                emit FundsWithdrawal(_wallet, stableCoinAddress, stableCoinBalance);
            }
        }
    }
}
