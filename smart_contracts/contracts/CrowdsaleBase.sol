// SPDX-License-Identifier: MIT
// OpenZeppelin Contracts v4.3.2 (utils/CrowdsaleBase.sol)
pragma solidity ^0.8.9;

import "../node_modules/@openzeppelin/contracts/utils/Strings.sol";
import "../node_modules/@openzeppelin/contracts/utils/Context.sol";
import "../node_modules/@openzeppelin/contracts/token/ERC20/IERC20.sol";
import "../node_modules/@openzeppelin/contracts/utils/math/SafeMath.sol";
import "../node_modules/@openzeppelin/contracts/token/ERC20/utils/SafeERC20.sol";
import "../node_modules/@openzeppelin/contracts/security/ReentrancyGuard.sol";
import "../node_modules/@openzeppelin/contracts/security/Pausable.sol";
import "../node_modules/@openzeppelin/contracts/utils/math/SafeMath.sol";
import "./WhitelistedRole.sol";
import "./StageAdminRole.sol";

/**
 * @title Crowdsale
 * @dev Crowdsale is a base contract for managing a token crowdsale,
 * allowing investors to purchase tokens with ether. This contract implements
 * such functionality in its most fundamental form and can be extended to provide additional
 * functionality and/or custom behavior.
 * The external interface represents the basic interface for purchasing tokens, and conforms
 * the base architecture for crowdsales. It is *not* intended to be modified / overridden.
 * The internal interface conforms the extensible and modifiable surface of crowdsales. Override
 * the methods to add functionality. Consider using 'super' where appropriate to concatenate
 * behavior.
 */
abstract contract CrowdsaleBase is
    Context,
    ReentrancyGuard,
    StageAdminRole,
    Pausable
{
    using SafeMath for uint256;

    struct StageExchangeRate {
        address token;
        uint256 rate;
    }

    struct StageSettings {
        string name;
        bool isPrivate;
        uint256 cap;
        uint256 minIndividualCap;
        uint256 maxIndividualCap;
        address[] whitelist;
        uint256 rate;
    }

    uint256 internal _nextStage;
    uint256 public numberOfStages;
    StageSettings[] stages;
    StageSettings _currentStage;

    uint256[] internal _stageWeiRaised;

    mapping(uint256 => mapping(address => uint256)) _stageTokensRaised;
    mapping(address => uint256) internal _tokensRaised;

    mapping(address => bool) internal _refunds;
    mapping(address => bool) internal _claimed;
    bool private fundsWithdrawn;

    // The token being sold
    IERC20 public _token;

    // Address where funds are collected
    address payable public _wallet;

    // How many token units a buyer gets per wei.
    // The rate is the conversion between wei and the smallest and indivisible token unit.
    // So, if you are using a rate of 1 with a ERC20Detailed token with 3 decimals called TOK
    // 1 wei will give you 1 unit, or 0.001 TOK.
    uint256 public _rate;

    mapping(address => uint256[]) internal _contributions;

    // Amount of wei raised
    uint256 public _weiRaised;
    uint256 public _tokensSold;
    uint256 public remainStageTokens;
    uint256 private _softCapDenominator;

    mapping(address => uint256) public _nativeCurrencyInvestments;
    uint256 public hardCap;
    uint256 public softCap;
    bool public _saleEnded;
    mapping(address => uint256) internal _investorTokenBalance;

    address[] internal _beneficiaries;

    /**
     * Event when crowdsale end
     */
    event CrowdsaleFinalized();

    /**
     * Event for token purchase logging
     * @param purchaser who paid for the tokens
     * @param beneficiary who got the tokens
     * @param value weis paid for purchase
     * @param amount amount of tokens purchased
     */
    event TokensPurchased(
        address indexed purchaser,
        address indexed beneficiary,
        uint256 value,
        uint256 amount
    );
    event NextStage(StageSettings stage);
    event StableCoinInvestmentsReturn(
        address indexed currency,
        address indexed investor,
        uint256 amount
    );
    event NativeCurrencyInvestmentsReturn(
        address indexed investor,
        uint256 amount
    );
    event FundsWithdrawal(
        address indexed wallet,
        address indexed stablecoin,
        uint256 amount
    );
    event NativeCurrencyWithdrawal(address indexed wallet, uint256 amount);

    /**
     * @param rate Number of token units a buyer gets per wei
     * @dev The rate is the conversion between wei and the smallest and indivisible
     * token unit. So, if you are using a rate of 1 with a ERC20Detailed token
     * with 3 decimals called TOK, 1 wei will give you 1 unit, or 0.001 TOK.
     * @param wallet Address where collected funds will be forwarded to
     * @param token Address of the token being sold
     */
    constructor(
        uint256 rate,
        address payable wallet,
        IERC20 token,
        uint256 softCapDenominator,
        uint256 numberOfStages_
    ) {
        require(rate > 0, "Crowdsale: rate is 0");
        require(
            softCapDenominator > 0 && softCapDenominator <= 100,
            "Crowdsale: soft cap denominator is 0"
        );
        require(wallet != address(0), "Crowdsale: wallet is the zero address");
        require(
            address(token) != address(0),
            "Crowdsale: token is the zero address"
        );
        _softCapDenominator = softCapDenominator;
        _rate = rate;
        _wallet = wallet;
        _token = token;
        _nextStage = 0;
        numberOfStages = numberOfStages_;
    }

    modifier saleInProgress() {
        require(_saleEnded == false, "Crowdsale: Crowdsale ended");
        _;
    }

    modifier saleEnded() {
        require(_saleEnded == true, "Crowdsale: Crowdsale in progress");
        _;
    }

    modifier softCapReached() {
        require(_tokensSold >= softCap, "Crowdsale: SoftCap not reached");
        _;
    }

    modifier isContributor(address wallet) {
        require(
            _contributions[wallet].length > 0,
            "Crowdsale: Only investors permitted"
        );
        _;
    }

    /**
     * @dev fallback function ***DO NOT OVERRIDE***
     * Note that other contracts will transfer funds with a base gas stipend
     * of 2300, which is not enough to call buyTokens. Consider calling
     * buyTokens directly when purchasing tokens from a contract.
     */
    fallback() external payable {
        buyTokens(_msgSender());
    }

    receive() external payable {
        buyTokens(_msgSender());
    }

    /**
     * @dev Change funding to next rate value
     */
    function _startNextStage(StageSettings memory settings)
        internal
        virtual
        onlyStageAdmins
    {
        require(
            _nextStage <= numberOfStages,
            "Crowdsale: Stage exceeds stage limit"
        );
        _updateRate(settings.rate);
        stages.push(settings);
        _stageWeiRaised.push(0);
        _currentStage = settings;
        _nextStage += 1;
        _removeAllWhitelisted();
        addWhitelisted(settings.whitelist);

        remainStageTokens = settings.cap;

        if (super.paused()) {
            super._unpause();
        }

        hardCap = 0;
        for (uint256 i = 0; i < stages.length; i++) {
            hardCap += stages[i].cap;
        }
        softCap = hardCap.mul(_softCapDenominator).div(100);

        emit NextStage(settings);
    }

    /**
     * @return the token being sold.
     */
    function getToken() public view returns (IERC20) {
        return _token;
    }

    /**
     * @return the address where funds are collected.
     */
    function getWallet() public view returns (address payable) {
        return _wallet;
    }

    /**
     * @return the number of token units a buyer gets per wei.
     */
    function getRate() public view returns (uint256) {
        return _rate;
    }

    /**
     * @return the amount of wei raised.
     */
    function getWeiRaised() public view returns (uint256) {
        return _weiRaised;
    }

    /**
     * @return the amount of purchased tokens.
     */
    function balanceOf(address wallet) public view returns (uint256) {
        return _investorTokenBalance[wallet];
    }

    /**
     * @dev Validation of an executed purchase. Observe state and use revert statements to undo rollback when valid
     * conditions are not met.
     * @param beneficiary Address performing the token purchase
     * @param weiAmount Value in wei involved in the purchase
     */
    function _postValidatePurchase(address beneficiary, uint256 weiAmount)
        internal
        view
        virtual
    {
        // solhint-disable-previous-line no-empty-blocks
    }

    /**
     * @dev Source of tokens. Override this method to modify the way in which the crowdsale ultimately gets and sends
     * its tokens.
     * @param beneficiary Address performing the token purchase
     * @param tokenAmount Number of tokens to be emitted
     */
    function _deliverTokens(address beneficiary, uint256 tokenAmount)
        internal
        virtual
    {
        //_token.safeTransfer(beneficiary, tokenAmount);
        _token.transferFrom(_wallet, beneficiary, tokenAmount);
    }

    /**
     * @dev Executed when a purchase has been validated and is ready to be executed. Doesn't necessarily emit/send
     * tokens.
     * @param beneficiary Address receiving the tokens
     * @param tokenAmount Number of tokens to be purchased
     */
    function _processPurchase(address beneficiary, uint256 tokenAmount)
        internal
        virtual
    {
        _deliverTokens(beneficiary, tokenAmount);
    }

    /**
     * @dev Override to extend the way in which ether is converted to tokens.
     * @param weiAmount Value in wei to be converted into tokens
     * @return Number of tokens that can be purchased with the specified _weiAmount
     */
    function _getTokenAmount(uint256 weiAmount)
        internal
        view
        returns (uint256)
    {
        // return weiAmount.mul(_rate);
        return weiAmount.mul(10**18).div(_rate);
    }

    /**
     * @dev Determines how ETH is stored/forwarded on purchases.
     */
    function _forwardFunds() internal {
        _wallet.transfer(msg.value);
    }

    /**
     * @dev Determines how ETH is stored/forwarded on purchases.
     */
    function _updateRate(uint256 value) internal {
        _rate = value;
    }

    function pause() public virtual onlyStageAdmins whenNotPaused {
        super._pause();
    }

    function stage() public view returns (string memory) {
        return _currentStage.name;
    }

    function minIndividualCap() public view returns (uint256) {
        return _currentStage.minIndividualCap;
    }

    function maxIndividualCap() public view returns (uint256) {
        return _currentStage.maxIndividualCap;
    }

    function cap() public view returns (uint256) {
        return _currentStage.cap;
    }

    function _addContributor(address beneficiary) internal virtual {
        if (_contributions[beneficiary].length == 0) {
            _contributions[beneficiary] = new uint256[](numberOfStages);
            _beneficiaries.push(beneficiary);
        }
    }

    /**
     * @dev Validation of an incoming purchase. Use require statements to revert state when conditions are not met.
     * Use `super` in contracts that inherit from Crowdsale to extend their validations.
     * Example from CappedCrowdsale.sol's _preValidatePurchase method:
     *     super._preValidatePurchase(beneficiary, weiAmount);
     *     require(weiRaised().add(weiAmount) <= cap);
     * @param beneficiary Address performing the token purchase
     * @param weiAmount Value in wei involved in the purchase
     * @param tokenAmount Value in number of tokens to be purchased
     */
    function _preValidatePurchase(
        address beneficiary,
        uint256 weiAmount,
        uint256 tokenAmount
    ) internal view virtual whenNotPaused {
        require(
            beneficiary != address(0),
            "Crowdsale: beneficiary is the zero address"
        );
        require(weiAmount != 0, "Crowdsale: weiAmount is 0");
        require(tokenAmount != 0, "Crowdsale: tokenAmount is 0");
        require(
            _currentStage.rate >= 0 && _nextStage > 0,
            "Crowdsale: Stage is not active"
        );
        require(
            _currentStage.isPrivate != true || isWhitelisted(beneficiary),
            "Crowdsale: Ongoing private sale: benefeciary is not white listed"
        );
        require(tokenAmount <= remainStageTokens, "Crowdsale: cap exceeded");
        require(
            _contributions[beneficiary][_nextStage - 1].add(tokenAmount) >=
                _currentStage.minIndividualCap &&
                _contributions[beneficiary][_nextStage - 1].add(tokenAmount) <=
                _currentStage.maxIndividualCap,
            "Crowdsale: Individual cap limit was not met"
        );
    }

    /**
     * @dev Override for extensions that require an internal state to check for validity (current user contributions,
     * etc.)
     * @param beneficiary Address receiving the tokens
     * @param weiAmount Value in wei involved in the purchase
     */
    function _updatePurchasingState(
        address beneficiary,
        uint256 weiAmount,
        uint256 tokenAmount
    ) internal {
        _contributions[beneficiary][_nextStage - 1] = _contributions[
            beneficiary
        ][_nextStage - 1].add(tokenAmount);
        _stageWeiRaised[_nextStage - 1] = _stageWeiRaised[_nextStage - 1].add(
            weiAmount
        );
        remainStageTokens = remainStageTokens.sub(tokenAmount);
    }

    function buyTokens(address beneficiary)
        public
        payable
        virtual
        nonReentrant
        saleInProgress
    {
        require(_rate > 0, "Crowdsale: Purchase unavailable");

        uint256 weiAmount = msg.value;

        // calculate token amount to be created
        uint256 tokens = _getTokenAmount(weiAmount);
        require(_tokensSold + tokens <= hardCap, "Crowdsale: Hardcap exceeded");

        _addContributor(beneficiary);
        _preValidatePurchase(beneficiary, weiAmount, tokens);

        // update state
        _weiRaised = _weiRaised.add(weiAmount);
        _tokensSold = _tokensSold.add(tokens);
        _nativeCurrencyInvestments[_msgSender()] = _nativeCurrencyInvestments[
            _msgSender()
        ].add(weiAmount); 
        _investorTokenBalance[beneficiary] = _investorTokenBalance[beneficiary]
            .add(tokens);
        //_processPurchase(beneficiary, tokens); // Token transfers after when ended
        emit TokensPurchased(_msgSender(), beneficiary, weiAmount, tokens);

        _updatePurchasingState(beneficiary, weiAmount, tokens);

        // //_forwardFunds();
        _postValidatePurchase(beneficiary, weiAmount);
    }

    // TODO separate test for double refund attempt
    function refund() public virtual saleEnded isContributor(_msgSender()) {
        require(_msgSender() != address(0), "Crowdsale: Invalid address");
        require(_tokensSold < softCap, "Crowdsale: SoftCap reached");
        require(_refunds[_msgSender()] == false, "Crowdsale: Already refunded");

        _refunds[_msgSender()] = true;

        // WITHDRAW ETHER
        uint256 nativeCurrencyContribution = _nativeCurrencyInvestments[
            _msgSender()
        ];
        if (nativeCurrencyContribution > 0) {
            (bool nativeCurrencyTransferSuccess, ) = payable(_msgSender()).call{
                value: nativeCurrencyContribution
            }("");
            require(
                nativeCurrencyTransferSuccess,
                "Crowdsale: Failed to refund Ether"
            );
            emit NativeCurrencyInvestmentsReturn(
                _msgSender(),
                nativeCurrencyContribution
            );
        }
    }

    function withdrawFunds() public virtual saleEnded softCapReached {
        require(!fundsWithdrawn, "Crowdsale: Funds withdrawn");

        // WITHDRAW ETHER
        if (_weiRaised > 0) {
            fundsWithdrawn = true;
            (bool nativeCurrencyTransferSuccess, ) = payable(_wallet).call{
                value: _weiRaised
            }("");
            require(
                nativeCurrencyTransferSuccess,
                "Crowdsale: Failed to send Ether"
            );
            emit NativeCurrencyWithdrawal(_wallet, _weiRaised);
        }
    }

    // TODO to remove?
    // TODO probably should be function executed by specified user to claim
    // TODO test checking double claim
    function tokensDistribution() external saleEnded softCapReached {
        for (uint256 index = 0; index < _beneficiaries.length; index++) {
            require(
                _claimed[_beneficiaries[index]] == false,
                "Crowdsale: Already claimed"
            );
            _claimed[_beneficiaries[index]] = true;
            IERC20(_token).transferFrom(
                _wallet,
                _beneficiaries[index],
                balanceOf(_beneficiaries[index])
            );
        }
    }

    function claimTokens(address wallet)
        external
        saleEnded
        softCapReached
        isContributor(wallet)
    {
        require(_claimed[wallet] == false, "Crowdsale: Already claimed");
        require(
            _contributions[wallet].length > 0,
            "Crowdsale: Only investors permitted"
        );
        _claimed[wallet] = true;
        IERC20(_token).transferFrom(_wallet, wallet, balanceOf(wallet));
    }

    function closeSale() external onlyStageAdmins {
        require(_saleEnded == false, "Crowdsale: Crowdsale ended");
        _saleEnded = true;
        emit CrowdsaleFinalized();
    }
}
