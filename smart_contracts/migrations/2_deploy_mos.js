const { deployProxy } = require('@openzeppelin/truffle-upgrades');

const MosaicoUpgradable = artifacts.require('MosaicoUpgradable');

module.exports = async function (deployer) {
    const instance = await deployProxy(MosaicoUpgradable, [
        '0x552F7E532c87aC4FD2dC9041F9B84a55F90B047B', 
        '0xd36c83eeD46754B9014089cB2E68f525a681C69f', 
        '0xFd079A89F6894Bc13c23b722D867a11F2dE7e25b'], { deployer });
    console.log('Deployed', instance.address);
  };
