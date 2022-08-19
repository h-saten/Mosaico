// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "./base/IERC20.sol";
import "./base/SafeMath.sol";
import "./base/AddressUpgradable.sol";
import "./base/Initializable.sol";
import "./base/ContextUpgradeable.sol";
import "./base/OwnableUpgradeable.sol";
import "./base/PausableUpgradeable.sol";
import "./base/CountersUpgradeable.sol";
import "./base/IERC1271Upgradeable.sol";
import "./base/SignatureCheckerUpgradeable.sol";
import "./base/StringsUpgradeable.sol";
import "./base/ECDSAUpgradeable.sol";
import "./base/EIP712Upgradeable.sol";

contract MosaicoUpgradable is Initializable, ContextUpgradeable, IERC20, OwnableUpgradeable, PausableUpgradeable, EIP712Upgradeable {
    using SafeMath for uint256;
    using AddressUpgradeable for address;
    using CountersUpgradeable for CountersUpgradeable.Counter;
    bytes32 private constant _PERMIT_TYPEHASH =
        keccak256("Permit(address owner,address spender,uint256 value,uint256 nonce,uint256 deadline)");
    bytes32 private _PERMIT_TYPEHASH_DEPRECATED_SLOT;

    mapping(address => CountersUpgradeable.Counter) private _nonces;
    mapping(address => uint256) private balances;
    mapping(address => mapping(address => uint256)) private _allowances;
    mapping(address => bool) private _isExcludedFromFee;
    mapping(address => bool) private _banned;

    uint256 private constant MAX = 280000000 * 10 ** 18;
    uint256 private _totalSupply;
    uint256 private _tFeeTotal;

    string private _name;
    string private _symbol;
    uint8 private _decimals;

    address public feeManager;

    uint256 public _burnFee;

    uint256 public _bountyFee;
    address public bountyWallet;

    uint256 public _ventureFee;
    address public ventureWallet;

    uint256 public _maxTxAmount;

    modifier onlyFeeManager{
        require(_msgSender() == feeManager, "Only manager can change fee");
        _;
    }

     modifier onlyNotBanned{
        require(_banned[_msgSender()] == false, "Only not banned wallets");
        _;
    }

    function initialize(address _bountyWallet, address _ventureWallet, address _tokenOwner) public initializer {
        _name = "Mosaico";
        _symbol = "MOS";
        _decimals = 18;
        _burnFee = 1.5 * 10 ** 2;
        _bountyFee = 1 * 10 ** 2;
        _ventureFee = 0.5 * 10 ** 2;
        _maxTxAmount = 180000000 * 10 ** _decimals;
        uint256 initialSupply = 180000000 * 10 ** _decimals;
        __ERC20_init(_bountyWallet, _ventureWallet, _tokenOwner);
        _mint(_tokenOwner, initialSupply);
    }

    function __ERC20_init(address _bountyWallet, address _ventureWallet, address _tokenOwner) internal onlyInitializing {
        __Context_init();
        __Ownable_init();
        __Pausable_init();
        __EIP712_init_unchained(_name, "1");
        __ERC20_init_unchained(_bountyWallet, _ventureWallet, _tokenOwner);
    }

    function __ERC20_init_unchained(address _bountyWallet, address _ventureWallet, address _tokenOwner) internal onlyInitializing {
        feeManager = _msgSender();
        ventureWallet = _ventureWallet;
        bountyWallet = _bountyWallet;
        _isExcludedFromFee[_tokenOwner] = true;
        _isExcludedFromFee[address(this)] = true;  
    }

    function name() public view returns (string memory) {
        return _name;
    }

    function symbol() public view returns (string memory) {
        return _symbol;
    }

    function decimals() public view returns (uint8) {
        return _decimals;
    }

    function totalSupply() public view override returns (uint256) {
        return _totalSupply;
    }

    function balanceOf(address account) public view override returns (uint256) {
        return balances[account];
    }

    function transfer(address recipient, uint256 amount) public onlyNotBanned override returns (bool) {
        _transfer(_msgSender(), recipient, amount);
        return true;
    }

    function allowance(address owner, address spender) public onlyNotBanned view override returns (uint256) {
        return _allowances[owner][spender];
    }

    function approve(address spender, uint256 amount) public onlyNotBanned override returns (bool) {
        _approve(_msgSender(), spender, amount);
        return true;
    }

    function transferFrom(address sender, address recipient, uint256 amount) public onlyNotBanned override returns (bool) {
        _transfer(sender, recipient, amount);
        uint256 currentAllowance = _allowances[sender][_msgSender()];
        require(currentAllowance >= amount, "ERC20: transfer amount exceeds allowance");
        unchecked {
            _approve(sender, _msgSender(), currentAllowance - amount);
        }

        return true;
    }

    function increaseAllowance(address spender, uint256 addedValue) public onlyNotBanned virtual returns (bool) {
        _approve(_msgSender(), spender, _allowances[_msgSender()][spender].add(addedValue));
        return true;
    }

    function decreaseAllowance(address spender, uint256 subtractedValue) public onlyNotBanned virtual returns (bool) {
        _approve(_msgSender(), spender, _allowances[_msgSender()][spender].sub(subtractedValue, "ERC20: decreased allowance below zero"));
        return true;
    }

    function totalFees() public view returns (uint256) {
        return _tFeeTotal;
    }

    function excludeFromFee(address account) public onlyFeeManager() {
        _isExcludedFromFee[account] = true;
    }

    function includeInFee(address account) public onlyFeeManager() {
        _isExcludedFromFee[account] = false;
    }

    function setBountyFee(uint256 bountyFee) external onlyFeeManager() {
        _bountyFee = bountyFee;
    }

    function setBountyWallet(address _wallet) external onlyOwner() {
        bountyWallet = _wallet;
    }

    function setVentureFee(uint256 ventureFee) external onlyFeeManager() {
        _ventureFee = ventureFee;
    }

    function setVentureWallet(address _wallet) external onlyOwner() {
        ventureWallet = _wallet;
    }

    function setFeeManager(address _feeManager) external onlyOwner() {
        feeManager = _feeManager;
    }

    function setMaxTxPercent(uint256 maxTxPercent) external onlyOwner() {
        _maxTxAmount = _totalSupply.mul(maxTxPercent).div(
            10 ** 2
        );
    }

    function ban(address wallet) external onlyOwner() {
        _banned[wallet] = true;
    }

    function unban(address wallet) external onlyOwner() {
        _banned[wallet] = false;
    }

    function isBanned(address wallet) public view returns (bool) {
        return _banned[wallet];
    }

    function isExcludedFromFee(address account) public view returns (bool) {
        return _isExcludedFromFee[account];
    }

    function _approve(address owner, address spender, uint256 amount) private {
        require(owner != address(0), "ERC20: approve from the zero address");
        require(spender != address(0), "ERC20: approve to the zero address");

        _allowances[owner][spender] = amount;
        emit Approval(owner, spender, amount);
    }

    function _transfer(address from, address to, uint256 amount) private {
        require(from != address(0), "ERC20: transfer from the zero address");
        require(to != address(0), "ERC20: transfer to the zero address");
        require(amount > 0, "Transfer amount must be greater than zero");

        if (from != owner() && to != owner())
            require(amount <= _maxTxAmount, "Transfer amount exceeds the maxTxAmount.");

        //if any account belongs to _isExcludedFromFee account then remove the fee
        if (_isExcludedFromFee[from] || _isExcludedFromFee[to]) {
            _transferStandard(from, to, amount);
        }
        else {
            _transferWithFee(from, to, amount);
        }

        emit Transfer(from, to, amount);
    }

    function _transferStandard(address sender, address recipient, uint256 amount) private {
        uint256 senderBalance = balances[sender];
        require(senderBalance >= amount, "ERC20: transfer amount exceeds balance");
        unchecked {
            balances[sender] = senderBalance.sub(amount);
        }
        balances[recipient] = balances[recipient].add(amount);
    }

    function _calculateBurnAmount(uint256 _amount) private view returns (uint256) {
        return _amount.mul(_burnFee).div(
            10 ** 4
        );
    }

    function _calculateBountyAmount(uint256 _amount) private view returns (uint256) {
        return _amount.mul(_bountyFee).div(
            10 ** 4
        );
    }

    function _calculateVentureAmount(uint256 _amount) private view returns (uint256) {
        return _amount.mul(_ventureFee).div(
            10 ** 4
        );
    }

    function _transferWithFee(address sender, address recipient, uint256 amount) private {
        uint256 burnAmount = _calculateBurnAmount(amount);
        uint256 bountyAmount = _calculateBountyAmount(amount);
        uint256 ventureAmount = _calculateVentureAmount(amount);

        _transferStandard(sender, recipient, amount.sub(burnAmount).sub(bountyAmount).sub(ventureAmount));
        _transferStandard(sender, address(0), burnAmount);
        _totalSupply = _totalSupply.sub(burnAmount);
        _transferStandard(sender, bountyWallet, bountyAmount);
        _transferStandard(sender, ventureWallet, ventureAmount);
        emit TransferFee(sender, recipient, burnAmount + bountyAmount + ventureAmount);
    }

    /** @dev Creates `amount` tokens and assigns them to `account`, increasing
     * the total supply.
     *
     * Emits a {Transfer} event with `from` set to the zero address.
     *
     * Requirements:
     *
     * - `account` cannot be the zero address.
     */
    function _mint(address account, uint256 amount) internal virtual {
        require(account != address(0), "ERC20: mint to the zero address");
        _totalSupply = _totalSupply.add(amount);
        balances[account] = balances[account].add(amount);
        emit Transfer(address(0), account, amount);
    }

    /**
     * @dev Destroys `amount` tokens from `account`, reducing the
     * total supply.
     *
     * Emits a {Transfer} event with `to` set to the zero address.
     *
     * Requirements:
     *
     * - `account` cannot be the zero address.
     * - `account` must have at least `amount` tokens.
     */
    function _burn(address account, uint256 amount) internal virtual {
        require(account != address(0), "ERC20: burn from the zero address");
        uint256 accountBalance = balances[account];
        require(accountBalance >= amount, "ERC20: burn amount exceeds balance");
        unchecked {
            balances[account] = accountBalance.sub(amount);
        }
        _totalSupply = _totalSupply.sub(amount);
        emit Transfer(account, address(0), amount);
    }

    function mint(uint256 amount, address walletAddress) public onlyOwner returns(bool success) {
        require(_totalSupply + amount <= MAX, "Contract was not configured to be mintable");
        require(walletAddress != address(0), "Wallet address hasn't been properly configured");
        _mint(walletAddress, amount);
        return true;
    }

    function burn(uint256 amount) public virtual {
        _burn(_msgSender(), amount);
    }

    /**
     * @dev See {IERC1155-balanceOfBatch}.
     *
     * Requirements:
     *
     * - `accounts` and `ids` must have the same length.
     */
    function balanceOfBatch(address[] memory accounts, uint256[] memory ids)
        public
        view
        virtual
        returns (uint256[] memory)
    {
        require(accounts.length == ids.length, "ERC1155: accounts and ids length mismatch");

        uint256[] memory batchBalances = new uint256[](accounts.length);

        for (uint256 i = 0; i < accounts.length; ++i) {
            batchBalances[i] = balanceOf(accounts[i]);
        }

        return batchBalances;
    }

    /**
     * @dev Triggers stopped state.
     *
     * Requirements:
     *
     * - The contract must not be paused.
     */
    function pause() public whenNotPaused onlyOwner {
        _pause();
    }

    /**
     * @dev Returns to normal state.
     *
     * Requirements:
     *
     * - The contract must be paused.
     */
    function unpause() public whenPaused onlyOwner {
        _unpause();
    }

    function permit(
        address owner,
        address spender,
        uint256 value,
        uint256 deadline,
        uint8 v,
        bytes32 r,
        bytes32 s
    ) public virtual {
        require(block.timestamp <= deadline, "ERC20Permit: expired deadline");

        bytes32 structHash = keccak256(abi.encode(_PERMIT_TYPEHASH, owner, spender, value, _useNonce(owner), deadline));

        bytes32 hash = _hashTypedDataV4(structHash);

        address signer = ECDSAUpgradeable.recover(hash, v, r, s);
        require(signer == owner, "ERC20Permit: invalid signature");

        _approve(owner, spender, value);
    }

    /**
     * @dev See {IERC20Permit-nonces}.
     */
    function nonces(address owner) public view virtual returns (uint256) {
        return _nonces[owner].current();
    }

    /**
     * @dev See {IERC20Permit-DOMAIN_SEPARATOR}.
     */
    // solhint-disable-next-line func-name-mixedcase
    function DOMAIN_SEPARATOR() external view returns (bytes32) {
        return _domainSeparatorV4();
    }

    /**
     * @dev "Consume a nonce": return the current value and increment.
     *
     * _Available since v4.1._
     */
    function _useNonce(address owner) internal virtual returns (uint256 current) {
        CountersUpgradeable.Counter storage nonce = _nonces[owner];
        current = nonce.current();
        nonce.increment();
    }

    /**
     * @dev This empty reserved space is put in place to allow future versions to add new
     * variables without shifting down storage in the inheritance chain.
     * See https://docs.openzeppelin.com/contracts/4.x/upgradeable#storage_gaps
     */
    uint256[49] private __gap;
}
