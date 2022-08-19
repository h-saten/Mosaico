// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "@opengsn/contracts/src/BaseRelayRecipient.sol";

contract GasStationTest is BaseRelayRecipient  {
    address[] public _callers;

    constructor(){
        _setTrustedForwarder(0xdA78a11FD57aF7be2eDD804840eA7f4c2A38801d);
    }

    function call() public returns (uint256) {
        _callers.push(_msgSender());
        return _callers.length;
    }

    function _msgSender() internal override view returns (address ret) {
        return BaseRelayRecipient._msgSender();
    }

    function _msgData() internal override view returns (bytes calldata ret) {
        return BaseRelayRecipient._msgData();
    }

    function versionRecipient() external override pure returns (string memory) {
        return "2.2.6";
    }
}