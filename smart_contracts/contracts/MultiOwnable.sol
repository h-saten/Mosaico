// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "../node_modules/@openzeppelin/contracts/utils/Context.sol";
import "../node_modules/@openzeppelin/contracts/utils/math/Math.sol";

abstract contract MultiOwnable is Context {
    using Math for uint256;

    mapping (address => bool) private _isOwner;
    address[] public _owners;

    event OwnerAdded(address indexed newOwner);
    event OwnerRemoved(address indexed oldOwner);

    constructor(address owner) {
        _setOwner(owner);
    }

    function _setOwner(address newOwner) internal {
        require(_isOwner[newOwner] == false, "MultiOwnable: already an owner");
        _isOwner[newOwner] = true;
        _owners.push(newOwner);
        emit OwnerAdded(newOwner);
    }

    modifier onlyOwner() {
        require(isOwner(_msgSender()), "MultiOwnable: caller is not the owner");
        _;
    }

    function getOwners() public view virtual returns (address[] memory) {
        return _owners;
    }

    /**
     * @dev Returns the address of the current owner.
     */
    function isOwner(address account) public view virtual returns (bool) {
        return _isOwner[account] == true;
    }

    function _removeOwner(address account) internal {
        require(isOwner(account), "MultiOwnable: Account is not an owner");
        _isOwner[account] = false;
        for (uint i=0; i < _owners.length - 1; i++)
            if (_owners[i] == account) {
                _owners[i] = _owners[_owners.length - 1];
                break;
            }
        _owners.pop();
        emit OwnerRemoved(account);
    }
}
