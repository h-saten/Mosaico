// SPDX-License-Identifier: MIT
// OpenZeppelin Contracts v4.3.2 (utils/MosaicoERC20.sol)
pragma solidity ^0.8.9;

library MosaicoStructs {
    struct ERC20Settings {
        bool isMintable;
        bool isBurnable;
        string name;
        string symbol;
        uint256 initialSupply;
        address walletAddress;
        bool isGovernance;
    }
}