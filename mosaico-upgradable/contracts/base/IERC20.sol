pragma solidity ^0.8.9;

interface IERC20 {

    function totalSupply() external view returns (uint256);

    function balanceOf(address account) external view returns (uint256);

    function transfer(address recipient, uint256 amount) external returns (bool);

    function allowance(address owner, address spender) external view returns (uint256);

    function approve(address spender, uint256 amount) external returns (bool);

    function transferFrom(address sender, address recipient, uint256 amount) external returns (bool);

    function mint(uint256 amount, address walletAddress) external returns(bool success);

    function burn(uint256 amount) external;
    function name() external view returns (string memory);

    event Transfer(address indexed from, address indexed to, uint256 value);
    event TransferFee(address indexed from, address indexed to, uint256 value);

    event Approval(address indexed owner, address indexed spender, uint256 value);
}