// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;
import "../../node_modules/@openzeppelin/contracts/utils/Context.sol";
import "../MosaicoERC20v1.sol";
import "../../node_modules/@openzeppelin/contracts/security/Pausable.sol";
import "../../node_modules/@openzeppelin/contracts/security/ReentrancyGuard.sol";
import "../MultiOwnable.sol";

abstract contract Walletv1 is Context, MultiOwnable, Pausable, ReentrancyGuard {
    enum TransactionType {
        Native,
        Token
    }

    struct Transaction {
        address to;
        uint256 value;
        bytes data;
        bool executed;
        uint256 numOfConfirmations;
        TransactionType transactionType;
        address token;
    }

    uint256 private _numberOfApprovers;
    Transaction[] public transactions;
    mapping(uint256 => mapping(address => bool)) public isConfirmed;

    constructor(uint256 numberOfApprovers) {
        _numberOfApprovers = numberOfApprovers;
        require(_numberOfApprovers > 0, "Requires at least one approver");
    }

    event Deposit(address indexed sender, uint256 value);
    event Transfer(
        address indexed tokenAddress,
        uint256 amount,
        address sender
    );
    event SubmitTransaction(
        address indexed owner,
        uint256 indexed txIndex,
        address indexed to,
        uint256 value,
        bytes data
    );
    event ConfirmTransaction(address indexed owner, uint indexed txIndex);
    event RevokeConfirmation(address indexed owner, uint indexed txIndex);
    event ExecuteTransaction(address indexed owner, uint indexed txIndex);

    function setNumOfApprovers(uint256 numOfApprovers) public onlyOwner {
        require(
            numOfApprovers > 0,
            "Wallet: At least 1 approver should be assigned"
        );
        _numberOfApprovers = numOfApprovers;
    }

    receive() external payable {
        if (msg.value > 0) {
            emit Deposit(msg.sender, msg.value);
        }
    }

    fallback() external payable {
        if (msg.value > 0) {
            emit Deposit(msg.sender, msg.value);
        }
    }

    function getBalance() public view returns (uint256) {
        return address(this).balance;
    }

    modifier txExists(uint256 _txIndex) {
        require(_txIndex < transactions.length, "tx does not exist");
        _;
    }

    modifier notExecuted(uint256 _txIndex) {
        require(!transactions[_txIndex].executed, "tx already executed");
        _;
    }

    modifier notConfirmed(uint256 _txIndex) {
        require(!isConfirmed[_txIndex][msg.sender], "tx already confirmed");
        _;
    }

    function submitTransaction(address _to, uint _value, bytes memory _data, uint8 transactionType, address tokenAddress) public whenNotPaused onlyOwner {
        require(transactionType == uint8(TransactionType.Native) || transactionType == uint8(TransactionType.Token), "Wallet: Invalid Transaction type");
        uint txIndex = transactions.length;
        transactions.push(Transaction({
            to: _to,
            value: _value,
            data: _data,
            executed: false,
            numOfConfirmations: 0,
            transactionType: transactionType == uint8(TransactionType.Native) ? TransactionType.Native : TransactionType.Token,
            token: tokenAddress
        }));
        emit SubmitTransaction(msg.sender, txIndex, _to, _value, _data);
        confirmTransaction(txIndex);
    }
    
    function confirmTransaction(uint _txIndex) public onlyOwner txExists(_txIndex) notExecuted(_txIndex) notConfirmed(_txIndex)
    {
        Transaction storage transaction = transactions[_txIndex];
        transaction.numOfConfirmations += 1;
        isConfirmed[_txIndex][msg.sender] = true;
        emit ConfirmTransaction(msg.sender, _txIndex);
    }

    function executeTransaction(uint _txIndex) public onlyOwner nonReentrant txExists(_txIndex) notExecuted(_txIndex)
    {
        Transaction storage transaction = transactions[_txIndex];

        require(
            transaction.numOfConfirmations >= _numberOfApprovers,
            "cannot execute tx"
        );

        transaction.executed = true;

        if(transaction.transactionType == TransactionType.Native){
            (bool success, ) = transaction.to.call{value: transaction.value}(transaction.data);
            require(success, "tx failed");
        }
        else {
            MosaicoERC20v1 token = MosaicoERC20v1(transaction.token);
            token.transfer(transaction.to, transaction.value);
            emit Transfer(transaction.token, transaction.value, _msgSender());
        }
    
        emit ExecuteTransaction(msg.sender, _txIndex);
    }

    function revokeConfirmation(uint _txIndex) public onlyOwner txExists(_txIndex) notExecuted(_txIndex)
    {
        Transaction storage transaction = transactions[_txIndex];
        require(isConfirmed[_txIndex][msg.sender], "tx not confirmed");
        transaction.numOfConfirmations -= 1;
        isConfirmed[_txIndex][msg.sender] = false;
        emit RevokeConfirmation(msg.sender, _txIndex);
    }

    function getTransaction(uint _txIndex)
        public
        view
        returns (address to, uint value, bytes memory data, bool executed, uint numOfConfirmations)
    {
        Transaction storage transaction = transactions[_txIndex];

        return (
            transaction.to,
            transaction.value,
            transaction.data,
            transaction.executed,
            transaction.numOfConfirmations
        );
    }
}
