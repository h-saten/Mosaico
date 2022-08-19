pragma solidity ^0.8.9;

import "../MultiOwnable.sol";
import "@openzeppelin/contracts/token/ERC20/utils/SafeERC20.sol";
import "@openzeppelin/contracts/utils/math/SafeMath.sol";
import "@openzeppelin/contracts/token/ERC20/IERC20.sol";

contract MosaicoVaultv1 is MultiOwnable {
    using SafeERC20 for IERC20;
    using SafeMath for uint256;

    struct Items {
        IERC20 token;
        address withdrawer;
        address creator;
        uint256 amount;
        uint256 unlockTimestamp;
        bool withdrawn;
        bool deposited;
    }

    mapping(address => mapping(address => uint256)) public walletTokenBalance;
    mapping (address => uint256[]) public depositsByTokenAddress;
    mapping (address => uint256[]) public depositsByWithdrawers;
    
    event Withdraw(uint256 id, address withdrawer, uint256 amount);
    event Canceled(uint256 id, address withdrawer, uint256 amount);
    event Sent(uint256 id, address withdrawer, address recipient, uint256 amount);

    mapping (uint256 => Items) public lockedToken;
    uint256 public depositsCount;

    constructor() MultiOwnable(_msgSender()) {
    }

    function deposit(IERC20 _token, address _withdrawer, uint256 _amount, uint256 _unlockTimestamp) external returns (uint256 _id) {
        require(
            _token.allowance(msg.sender, address(this)) >= _amount,
            "Approve tokens first!"
        );
        _token.safeTransferFrom(_msgSender(), address(this), _amount);
        walletTokenBalance[address(_token)][_msgSender()] = walletTokenBalance[address(_token)][_msgSender()].add(_amount);

        _id = ++depositsCount;
        lockedToken[_id].token = _token;
        lockedToken[_id].withdrawer = _withdrawer;
        lockedToken[_id].amount = _amount;
        lockedToken[_id].unlockTimestamp = _unlockTimestamp;
        lockedToken[_id].withdrawn = false;
        lockedToken[_id].deposited = true;
        lockedToken[_id].creator = _msgSender();

        depositsByTokenAddress[address(_token)].push(_id);
        depositsByWithdrawers[_withdrawer].push(_id);
        return _id;
    }

    function withdrawTokens(uint256 _id) external {
        require(block.timestamp >= lockedToken[_id].unlockTimestamp, 'Tokens are still locked!');
        require(_msgSender() == lockedToken[_id].withdrawer, 'You are not the withdrawer!');
        require(lockedToken[_id].deposited, 'Tokens are not yet deposited!');
        require(!lockedToken[_id].withdrawn, 'Tokens are already withdrawn!');
        lockedToken[_id].withdrawn = true;
        walletTokenBalance[address(lockedToken[_id].token)][lockedToken[_id].creator] = walletTokenBalance[address(lockedToken[_id].token)][lockedToken[_id].creator].sub(lockedToken[_id].amount);
        emit Withdraw(_id, _msgSender(), lockedToken[_id].amount);
        lockedToken[_id].token.safeTransfer(_msgSender(), lockedToken[_id].amount);
    }

    function cancelDeposit(uint256 _id) external {
        require(!lockedToken[_id].withdrawn, 'Tokens are already withdrawn!');
        require(lockedToken[_id].creator == _msgSender(), 'You are not creator!');
        lockedToken[_id].withdrawn = true;
        walletTokenBalance[address(lockedToken[_id].token)][_msgSender()] = walletTokenBalance[address(lockedToken[_id].token)][_msgSender()].sub(lockedToken[_id].amount);
        emit Canceled(_id, _msgSender(), lockedToken[_id].amount);
        lockedToken[_id].token.safeTransfer(_msgSender(), lockedToken[_id].amount);
    }

    function send(uint256 _id, address recipient, uint256 _amount) external {
        require(block.timestamp >= lockedToken[_id].unlockTimestamp, 'Tokens are still locked!');
        require(_msgSender() == lockedToken[_id].withdrawer, 'You are not the withdrawer!');
        require(lockedToken[_id].deposited, 'Tokens are not yet deposited!');
        require(!lockedToken[_id].withdrawn, 'Tokens are already withdrawn!');
        require(lockedToken[_id].amount >= _amount, 'Not enough tokens to send');
        walletTokenBalance[address(lockedToken[_id].token)][lockedToken[_id].creator] = walletTokenBalance[address(lockedToken[_id].token)][lockedToken[_id].creator].sub(_amount);
        lockedToken[_id].amount = lockedToken[_id].amount.sub(_amount);
        if(lockedToken[_id].amount <= 0) {
            lockedToken[_id].withdrawn = true;
        }
        emit Sent(_id, _msgSender(), recipient, _amount);
        lockedToken[_id].token.safeTransfer(recipient, _amount);
    }

    function getVaultById(uint256 _id) view external returns (Items memory) {
        return lockedToken[_id];
    }

    function getDepositsByTokenAddress(address _id) view external returns (uint256[] memory) {
        return depositsByTokenAddress[_id];
    }

    function getDepositsByWithdrawer(address _token, address _withdrawer) view external returns (uint256) {
        return walletTokenBalance[_token][_withdrawer];
    }

    function getVaultsByWithdrawer(address _withdrawer) view external returns (uint256[] memory) {
        return depositsByWithdrawers[_withdrawer];
    }
}
