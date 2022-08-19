// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "../../node_modules/@openzeppelin/contracts/utils/Context.sol";
import "../MosaicoERC20v1.sol";
import "../Library.sol";

abstract contract TokenManagerv1 is Context {
    mapping(address => bool) private _isTokenManaged;
    mapping(address => bool) private _isVotingToken;
    address[] private _tokens;

    function isManagedToken(address token) public view virtual returns (bool) {
        return _isTokenManaged[token] == true;
    }

    function isVotingToken(address token) public view virtual returns (bool) {
        return _isVotingToken[token] == true;
    }

    modifier onlyManaged(address token) {
        require(isManagedToken(token) == true, "Token Manager: Must be managed token");
        _;
    }

    event TokenAdded(address tokenAddress, bool isVoting);
    
    function _addToken(address token, bool isVoting) internal {
        require(_isTokenManaged[token] == false, "Token Manager: Token already added");
        _isTokenManaged[token] = true;
        _isVotingToken[token] = isVoting;
        _tokens.push(token);
        emit TokenAdded(token, isVoting);
    }

    function getTokens() public virtual view returns(address[] memory) {
        return _tokens;
    }

    function getWeight(address account, address tokenAddress) public virtual view returns (uint256) {
        require(isManagedToken(tokenAddress) == true, "Token Manager: Unmanaged token");
        MosaicoERC20v1 token = MosaicoERC20v1(tokenAddress);
        uint256 weight = token.balanceOf(account);
        return weight;
    }

    function _mint(address tokenAddress, uint256 amount, address receiver) internal {
        require(isManagedToken(tokenAddress) == true, "Token Manager: Unmanaged token");
        MosaicoERC20v1 token = MosaicoERC20v1(tokenAddress);
        token.mint(amount, receiver);
    }

    function _burn(address tokenAddress, uint256 amount) internal {
        require(isManagedToken(tokenAddress) == true, "Token Manager: Unmanaged token");
        MosaicoERC20v1 token = MosaicoERC20v1(tokenAddress);
        token.burn(amount);
    }

    function _pause(address tokenAddress) internal {
        require(isManagedToken(tokenAddress) == true, "Token Manager: Unmanaged token");
        MosaicoERC20v1 token = MosaicoERC20v1(tokenAddress);
        token.pause();
    }

    function _unpause(address tokenAddress) internal {
        require(isManagedToken(tokenAddress) == true, "Token Manager: Unmanaged token");
        MosaicoERC20v1 token = MosaicoERC20v1(tokenAddress);
        token.unpause();
    }

    function _create(MosaicoStructs.ERC20Settings memory contractSettings) internal virtual returns(address){
        MosaicoERC20v1 token = new MosaicoERC20v1(contractSettings);
        _addToken(address(token), contractSettings.isGovernance);
        return address(token);
    }
}