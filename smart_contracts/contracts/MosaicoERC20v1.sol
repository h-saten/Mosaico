// SPDX-License-Identifier: MIT
// OpenZeppelin Contracts v4.3.2 (utils/MosaicoERC20.sol)
pragma solidity ^0.8.9;

import "../node_modules/@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "../node_modules/@openzeppelin/contracts/token/ERC20/extensions/ERC20Burnable.sol";
import "../node_modules/@openzeppelin/contracts/security/Pausable.sol";
import "../node_modules/@openzeppelin/contracts/access/Ownable.sol";
import "../node_modules/@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "../node_modules/@openzeppelin/contracts/token/ERC20/extensions/draft-ERC20Permit.sol";
import "./MultiOwnable.sol";
import "./Library.sol";

contract MosaicoERC20v1 is ERC20, ERC20Permit, Pausable, MultiOwnable, ERC20Burnable {
    address internal _minter = _msgSender();
    MosaicoStructs.ERC20Settings internal _settings;

    constructor(MosaicoStructs.ERC20Settings memory settings) 
        ERC20(settings.name, settings.symbol) 
        MultiOwnable(_msgSender())
        Pausable()
        ERC20Permit(settings.name)
    {
        _settings = settings;

        if(_settings.walletAddress != address(0)){
            _minter = _settings.walletAddress;
            if(_settings.walletAddress != _msgSender()){
                _setOwner(_settings.walletAddress);
            }
        }

        _mint(_minter, _settings.initialSupply);
    }

    function transfer(address to, uint256 value) public override whenNotPaused returns (bool) {
        return super.transfer(to, value);
    }

    function transferFrom(address from, address to, uint256 value) public override whenNotPaused returns (bool) {
        return super.transferFrom(from, to, value);
    }

    function pause() public onlyOwner {
        super._pause();
    }

    function unpause() public onlyOwner {
        super._unpause();
    }

    function mint(uint256 amount, address walletAddress) public onlyOwner returns(bool success) {
        require(_settings.isMintable == true, "Contract was not configured to be mintable");
        require(walletAddress != address(0), "Wallet address hasn't been properly configured");
        super._mint(walletAddress, amount);
        return true;
    }

    function burn(uint256 amount) public virtual override {
        require(_settings.isBurnable == true, "Contract was not configured to be burnable");
        super.burn(amount);
    }

    function batchTransferFrom(address from, address[] memory recipients, uint256[] memory amounts) public onlyOwner {
        require(recipients.length == amounts.length, "ERC1155: to and amounts length mismatch");
        require(from != address(0), "ERC1155: transfer to the zero address");

        for (uint256 i = 0; i < recipients.length; ++i) {
            address to = recipients[i];
            uint256 amount = amounts[i];
            uint256 fromBalance = balanceOf(from);
            require(fromBalance >= amount, "ERC1155: insufficient balance for transfer");
            _transfer(from, to, amount);
        }
    }
} 