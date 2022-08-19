// SPDX-License-Identifier: MIT
// OpenZeppelin Contracts v4.3.2 (utils/DefaultCrowdsale.sol)
pragma solidity ^0.8.9;
//import "./DefaultCrowdsalev1.sol";

/**
 * @title VestingCrowdsalev1
 * @dev This is an example of a fully fledged crowdsale.
 */
// contract VestingCrowdsalev1 is DefaultCrowdsalev1  {
//     address[] internal _fundWallets;
//     uint256[] internal _releaseTimes;
//     mapping(address => uint256) public contributions;
//     bool internal _canWithdrawEarly;
//     address public privateSaleTimeLock;

//     constructor(
//         address payable wallet,
//         IERC20 token,
//         string[] memory names, 
//         uint256[] memory rates,
//         uint256[] memory caps, 
//         bool[] memory privateSales,
//         address[] memory whitelist,
//         uint[] memory minIndividualCaps,
//         uint[] memory maxIndividualCaps,
//         address[] memory fundWallets,
//         uint256[] memory releaseTimes,
//         bool canWithdrawEarly
//     ) DefaultCrowdsalev1(wallet, token, names, rates, caps, privateSales, whitelist, minIndividualCaps, maxIndividualCaps) {
//         _fundWallets = fundWallets;
//         _releaseTimes = releaseTimes;
//         _canWithdrawEarly = canWithdrawEarly;
//     }
// }