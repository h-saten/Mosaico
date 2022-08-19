const { deployProxy } = require('@openzeppelin/truffle-upgrades');

const TMos = artifacts.require('GenericMosaicoUpgradable');
const Staking = artifacts.require('StakingUpgradable');

module.exports = async function (deployer) {
    const gmos = await deployProxy(TMos, [
        'Mosaico Governance',
        'GMOS',
        '0xFd079A89F6894Bc13c23b722D867a11F2dE7e25b'], { deployer });
    console.log('Deployed GMOS', gmos.address);
    const settings = {
        maxRewardPerStaker: '300000000000000000000',
        rewardCycle: 30,
        stakingToken: '0x0E4AeE1513Cf05b97fAFFf860fC291324B604F8a',
        minimumStakingAmount: 1,
        tmos: gmos.address,
        tokendaId: 1
    }
    const staking = await deployProxy(Staking, [settings], { deployer });
    console.log('Deployed Staking', staking.address);
    await gmos.addMinter(staking.address);
    console.log('Successfully added minter');
};
