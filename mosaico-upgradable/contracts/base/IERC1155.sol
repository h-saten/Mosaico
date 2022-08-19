pragma solidity ^0.8.9;

interface IERC1155 {
    event TransferSingle(address indexed operator, address indexed from, address indexed to, uint256 id, uint256 value);

    function totalSupply(uint256 id) external view returns (uint256);
    
    function balanceOf(address account, uint256 id) external view returns (uint256);
    
    function balanceOfBatch(address[] calldata accounts, uint256[] calldata ids)
        external
        view
        returns (uint256[] memory);
    
    
    function allowance(address owner, address spender, uint256 id) external view returns (uint256);
    
    function approve(address spender, uint256 amount, uint256 id) external returns (bool);
        
    function safeTransferFrom(
        address from,
        address to,
        uint256 id,
        uint256 amount
    ) external;

    function safeBatchTransferFrom(
        address from,
        address to,
        uint256[] calldata ids,
        uint256[] calldata amounts
    ) external;

    function burn(address account, uint256 id, uint256 amount) external returns(bool success);

    function mint(address account, uint256 id, uint256 amount) external returns(bool success);
}