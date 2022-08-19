const MosaicoERC20v1 = artifacts.require("MosaicoERC20v1");
const Staking = artifacts.require("Stakingv1");

const chai = require('chai');
chai.use(require('chai-bignumber')());
const assert = chai.assert;
chai.should();

require('truffle-test-utils').init();

var utils = web3.utils;

const blockchainTime = require("./helpers/truffleTimeHelper");

asBN = (value) => utils.toBN(value);


contract('Staking', accounts => {
    const initialSupply = web3.utils.toWei("1000000", "ether" );
    const settings = {
        name: "Mosaico ERC20 Test Token",
        symbol: "MOSTT",
        initialSupply: initialSupply.toString(),
        isMintable: false,
        isBurnable: false,
        isPaused: false,
        walletAddress: accounts[0],
        isGovernance: false
    };

    const rewardTokenSettings = {
        name: "W Mosaico ERC20",
        symbol: "WMOSTT",
        initialSupply: initialSupply.toString(),
        isMintable: false,
        isBurnable: false,
        isPaused: false,
        walletAddress: accounts[0],
        isGovernance: true
    };

    const stakingSettings = {
        rewardPerToken: 10,
        rewardPeriodInDays: 1,
        minimumStakingAmount: 0,
        userShouldDeclarePeriod: false,
        punishmentFee: 0,
        platformWallet: accounts[0],
        withdrawThreshold: 0
    };

    let tokenContract;
    let rewardContract;
    let stakingContract;

    const doStake = async (value = web3.utils.toWei("1", "ether" ), address = accounts[0]) => {
        await tokenContract.approve(stakingContract.address, value, {from: address});
        await stakingContract.stake(value, 0, {from: address});
    };

    beforeEach(async function () {
        tokenContract = await MosaicoERC20v1.new(settings);
        rewardContract = await MosaicoERC20v1.new(rewardTokenSettings);
        const localStakingSettings = {
            ...stakingSettings,
            stakingToken: tokenContract.address,
            rewardToken: rewardContract.address
        }
        stakingContract = await Staking.new(localStakingSettings);
        await tokenContract.transfer(stakingContract.address, web3.utils.toWei("200000", "ether" ));
        await rewardContract.transfer(stakingContract.address, web3.utils.toWei("200000", "ether" ));
    });

    describe('Staking', () => {
        it("cant withdraw bigger amount than current stake", async() => {
            //arrange
            let owner = accounts[0];
            //act
            await doStake();
            try {
                await stakingContract.claimReward({from:owner});
                throw "success";
            } catch(error){
                //assert
                error.should.not.equal("success");
            }
        });

        it("summary return 50 tokens as staked amount", async() => {
            //arrange
            let owner = accounts[0];
            let stakeAmount =  web3.utils.toWei("50", "ether" );

            //act
            await doStake(stakeAmount);
            const stake = await stakingContract.hasStake(owner);
            // assert
            assert.equal(1, stake.stakes.length);
            assert.equal(web3.utils.fromWei(stake.total_amount), 50);
            assert.equal(web3.utils.fromWei(stake.stakes[0].amount), 50);
        });

        it("stake reward is 0 when staked while ago", async () => {
            //arrange
            let owner = accounts[0];
            //act
            await doStake();
            const stake = await stakingContract.hasStake(owner);
            //assert
            assert.equal(0, stake.total_reward_amount);
        });

        it("withdraw tokens from one stake", async() => {
            //arrange
            let owner = accounts[0];
            let stakeAmount =  web3.utils.toWei("50", "ether" );
            const initialBalance = await tokenContract.balanceOf(owner) / 1e18;
            //act
            await doStake(stakeAmount);
            await stakingContract.withdraw({from:owner});
            const balanceAfter = await tokenContract.balanceOf(owner) / 1e18;
            const stake = await stakingContract.hasStake(owner);
            assert.equal(0, stake.total_amount);
            assert.equal(initialBalance, balanceAfter);
        });

        it("reward bigger than 0 after block mining", async () => {
            //arrange
            let owner = accounts[0];
            const stakeAmount =  web3.utils.toWei("100", "ether" );
            //act
            await doStake(stakeAmount);
            await blockchainTime.advanceInDays(1);
            const stake = await stakingContract.hasStake(owner);
            //assert
            assert.isAtLeast(parseInt(stake.total_reward_amount), 10);
        });

        it("summary return do stakes info", async () => {
            //arrange
            let owner = accounts[0];
            const stake1Amount =  web3.utils.toWei("100", "ether" );
            const stake2Amount =  web3.utils.toWei("300", "ether" );
            //act
            await doStake(stake1Amount);
            await blockchainTime.advanceInDays(1);
            await doStake(stake2Amount);
            await blockchainTime.advanceInDays(1);
            const stake = await stakingContract.hasStake(owner);
            //assert
            assert.equal(web3.utils.fromWei(stake.total_amount), 400);
            assert.equal(web3.utils.fromWei(stake.total_reward_amount), 5000);
            assert.equal(web3.utils.fromWei(stake.total_punishment), 0);
        });

        it("summary return do stakes info with 2 days reward period", async () => {
            //arrange
            stakingContract = await Staking.new({
                ...stakingSettings,
                stakingToken: tokenContract.address,
                rewardToken: rewardContract.address,
                rewardPeriodInDays: 2
            });
            let owner = accounts[0];
            const stakeAmount = web3.utils.toWei("100", "ether" );
            //act
            await doStake(stakeAmount);
            await blockchainTime.advanceInDays(2);
            const stake = await stakingContract.hasStake(owner);
            //assert
            assert.equal(web3.utils.fromWei(stake.total_amount), 100);
            assert.equal(web3.utils.fromWei(stake.total_reward_amount), 1000);
            assert.equal(web3.utils.fromWei(stake.total_punishment), 0);
        });

        it("two stakers and return valid summary for second one", async () => {
            //arrange
            let anotherStaker = accounts[1];
            const stakeAmount = web3.utils.toWei("100", "ether" );
            await tokenContract.transfer(anotherStaker, stakeAmount);
            await tokenContract.approve(stakingContract.address, stakeAmount, {from: anotherStaker})
            //act
            await doStake(stakeAmount);
            await doStake(stakeAmount, anotherStaker);
            await blockchainTime.advanceInDays(1);
            const stake = await stakingContract.hasStake(anotherStaker);
            //assert
            assert.equal(stake.stakes.length, 1);
        });

        it("withdraw reward", async() => {
            //arrange
            let owner = accounts[0];
            let stake1Amount = web3.utils.toWei("20", "ether" );
            let stake2Amount = web3.utils.toWei("10", "ether" );
            let stake3Amount = web3.utils.toWei("30", "ether" );
            await tokenContract.approve(stakingContract.address, web3.utils.toWei("60", "ether" ));
            //act
            await doStake(stake1Amount);
            await doStake(stake2Amount);
            await blockchainTime.advanceInDays(1);
            await doStake(stake3Amount);
            await blockchainTime.advanceInDays(1);
            const beforeRewardBalance = web3.utils.fromWei(await rewardContract.balanceOf(owner));
            console.log(beforeRewardBalance.toString())
            //assert
            const stakeBeforeWithdrawal = await stakingContract.hasStake(owner);
            assert.equal(web3.utils.fromWei(stakeBeforeWithdrawal.total_amount), 60);
            assert.equal(web3.utils.fromWei(stakeBeforeWithdrawal.total_reward_amount), 900);
            assert.equal(web3.utils.fromWei(stakeBeforeWithdrawal.total_punishment), 0);

            await stakingContract.claimReward();
            const stakeAfterWithdrawal = await stakingContract.hasStake(owner);
            assert.equal(0, stakeAfterWithdrawal.total_amount);

            const rewardBalance = web3.utils.fromWei(await rewardContract.balanceOf(owner));
            console.log(rewardBalance.toString())
            assert.equal((rewardBalance - beforeRewardBalance).toString(), 900);
        });

    });

});