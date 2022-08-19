// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "../node_modules/@openzeppelin/contracts/utils/Context.sol";
import "./Roles.sol";
import "./WhitelistedRole.sol";

/**
 * @title StageAdminRole
 * @dev StageAdminRole are responsible for submitting new stages.
 */
abstract contract StageAdminRole is Context, WhitelistedRole {
    using Roles for Roles.Role;

    event StageAdminRoleAdded(address indexed account);
    event StageAdminRoleRemoved(address indexed account);

    Roles.Role private _stageAdmins;

    constructor () {
        _addStageAdmin(_msgSender());
    }

    modifier onlyStageAdmins() {
        require(isStageAdmin(_msgSender()), "StageAdminRole: caller does not have the StageAdminRole role");
        _;
    }

    function isStageAdmin(address account) public view returns (bool) {
        return _stageAdmins.has(account);
    }

    function addStageAdmin(address account) public onlyWhitelistAdmin {
        _addWhitelistAdmin(account);
    }

    function removeStageAdmin(address account) public onlyWhitelistAdmin {
        _removeStageAdmin(account);
    }

    function renounceStageAdmin() public {
        _removeStageAdmin(_msgSender());
    }

    function _addStageAdmin(address account) internal {
        _stageAdmins.add(account);
        emit StageAdminRoleAdded(account);
    }

    function _removeStageAdmin(address account) internal {
        _stageAdmins.remove(account);
        emit StageAdminRoleRemoved(account);
    }
}