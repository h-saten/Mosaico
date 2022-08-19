pragma solidity ^0.8.9;

import "./base/IERC1155.sol";
import "./base/SafeMath.sol";
import "./base/AddressUpgradable.sol";
import "./base/Initializable.sol";
import "./base/ContextUpgradeable.sol";
import "./base/OwnableUpgradeable.sol";
import "./base/PausableUpgradeable.sol";
import "./base/CountersUpgradeable.sol";

contract GenericMosaicoUpgradable is
    Initializable,
    ContextUpgradeable,
    IERC1155,
    OwnableUpgradeable,
    PausableUpgradeable
{
    using SafeMath for uint256;
    using AddressUpgradeable for address;
    using CountersUpgradeable for CountersUpgradeable.Counter;

    string private _name;
    string private _symbol;
    uint8 private _decimals;
    mapping(uint256 => uint256) private _totalSupply;

    mapping(uint256 => mapping(address => uint256)) private _balances;
    mapping(uint256 => mapping(address => mapping(address => uint256))) private _allowances;
    mapping(address => bool) private _minters;

    modifier onlyMinter() {
        require(_minters[_msgSender()] == true, "Ownable: caller is not the owner");
        _;
    }

    function initialize(string memory name_, string memory symbol_, address _tokenOwner) public initializer {
        _name = name_;
        _symbol = symbol_;
        _decimals = 18;
        __ERC20_init(_tokenOwner);
    }

    function __ERC20_init(address _tokenOwner) internal onlyInitializing {
        __Context_init();
        __Ownable_init();
        __Pausable_init();
        __ERC20_init_unchained(_tokenOwner);
    }

    function __ERC20_init_unchained(address _tokenOwner) internal onlyInitializing {
        _minters[_tokenOwner] = true;
        _minters[_msgSender()] = true;
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

    function totalSupply(uint256 id) public view override returns (uint256) {
        return _totalSupply[id];
    }

    function balanceOf(address account, uint256 id) public view override returns (uint256) {
        return _balances[id][account];
    }

    function allowance(address owner, address spender, uint256 id) public view override returns (uint256) {
        return _allowances[id][owner][spender];
    }

    function approve(address spender, uint256 amount, uint256 id) public whenNotPaused override returns (bool) {
        _approve(_msgSender(), spender, amount, id);
        return true;
    }

    function _approve(address owner, address spender, uint256 amount, uint256 id) private {
        require(owner != address(0), "ERC20: approve from the zero address");
        require(spender != address(0), "ERC20: approve to the zero address");
        _allowances[id][owner][spender] = amount;
    }

    function addMinter(address walletAddress) public onlyOwner {
        require(walletAddress != address(0), "Wallet address hasn't been properly configured");
        require(_minters[walletAddress] != true, "Wallet address is already a minter");
        _minters[walletAddress] = true;
    }

    function balanceOfBatch(address[] memory accounts, uint256[] calldata ids)
        public
        view
        virtual
        returns (uint256[] memory)
    {
        require(accounts.length == ids.length, "ERC1155: accounts and ids length mismatch");

        uint256[] memory batchBalances = new uint256[](accounts.length);

        for (uint256 i = 0; i < accounts.length; ++i) {
            batchBalances[i] = balanceOf(accounts[i], ids[i]);
        }

        return batchBalances;
    }

    function safeTransferFrom(
        address from,
        address to,
        uint256 id,
        uint256 amount
    ) public whenNotPaused virtual {
        require(to != address(0), "ERC1155: transfer to the zero address");
        require(allowance(from, _msgSender(), id) > amount || from == _msgSender(), "ERC1155: transfer caller is not owner nor approved");
        address operator = _msgSender();
        uint256 fromBalance = _balances[id][from];
        require(fromBalance >= amount, "ERC1155: insufficient balance for transfer");
        unchecked {
            _balances[id][from] = fromBalance - amount;
        }
        _balances[id][to] += amount;
        emit TransferSingle(operator, from, to, id, amount);
    }

    function safeBatchTransferFrom(
        address from,
        address to,
        uint256[] memory ids,
        uint256[] memory amounts
    ) public whenNotPaused virtual {
        require(ids.length == amounts.length, "ERC1155: ids and amounts length mismatch");
        require(to != address(0), "ERC1155: transfer to the zero address");

        for (uint256 i = 0; i < ids.length; ++i) {
            uint256 id = ids[i];
            uint256 amount = amounts[i];
            require(allowance(from, _msgSender(), id) >= amount || from == _msgSender(), "ERC1155: transfer caller is not owner nor approved");

            uint256 fromBalance = _balances[id][from];
            require(fromBalance >= amount, "ERC1155: insufficient balance for transfer");
            unchecked {
                _balances[id][from] = fromBalance - amount;
            }
            _balances[id][to] += amount;
        }
    }

    function pause() public whenNotPaused onlyOwner {
        _pause();
    }

    function unpause() public whenPaused onlyOwner {
        _unpause();
    }

    function mint(
        address account,
        uint256 id,
        uint256 amount
    ) public onlyMinter whenNotPaused virtual returns(bool success) {
        require(account != address(0), "ERC1155: mint to the zero address");
        address operator = _msgSender();
        _balances[id][account] += amount;
        emit TransferSingle(operator, address(0), account, id, amount);
        return true;
    }

    function burn(
        address account,
        uint256 id,
        uint256 amount
    ) public virtual returns(bool success) {
        require(account != address(0), "ERC1155: burn from the zero address");
        require(allowance(account, _msgSender(), id) >= amount || account == _msgSender(), "ERC1155: transfer caller is not owner nor approved");
        address operator = _msgSender();
        uint256 accountBalance = _balances[id][account];
        require(accountBalance >= amount, "ERC1155: burn amount exceeds balance");
        unchecked {
            _balances[id][account] = accountBalance - amount;
        }

        emit TransferSingle(operator, account, address(0), id, amount);
        return true;
    }

    /**
     * @dev This empty reserved space is put in place to allow future versions to add new
     * variables without shifting down storage in the inheritance chain.
     * See https://docs.openzeppelin.com/contracts/4.x/upgradeable#storage_gaps
     */
    uint256[49] private __gap;
}
