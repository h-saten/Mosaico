const BigNumber = web3.BigNumber;
const TetherToken = artifacts.require("TetherToken");
require('truffle-test-utils').init();
var utils = web3.utils;
require('chai').use(require('chai-bignumber')(BigNumber)).should();

asBN = (value) => utils.toBN(value);

contract('TetherToken', accounts => {
    const settings = {
        name: "Tether",
        symbol: "USDT",
        initialSupply: 1000000,
        walletAddress: accounts[0]
    };
    const _decimals = 18;

    let contract;

    beforeEach(async function () {
        contract = await TetherToken.new(asBN(settings.initialSupply), settings.name, settings.symbol, _decimals);
    });

    describe('ERC20 Attributes', function () {
        it('has the correct name', async function () {
            //arrange
            //act
            let tokenName = await contract.name();
            //assert
            tokenName.should.equal(settings.name);
        });

        it('has correct symbol', async function () {
            //arrange
            //act
            let tokenSymbol = await contract.symbol();
            //assert
            tokenSymbol.should.equal(settings.symbol);
        });

        it('has correct decimals', async function () {
            //arrange
            //act
            let tokenDecimals = await contract.decimals();
            //assert
            tokenDecimals.toNumber().should.equal(_decimals);
        });

        it('has correct total supply', async function () {
            //arrange
            //act
            let totalSupply = await contract.totalSupply();
            //assert
            totalSupply.toNumber().should.equal(settings.initialSupply);
        });

        it('creator has all tokens', async function () {
            //arrange
            //act
            let balance = await contract.balanceOf(accounts[0]);
            //assert
            balance.toNumber().should.equal(settings.initialSupply);
        });

    });

    describe('ERC20 Transfer', function () {
        
        it('should not change total supply', async function () {
            //arrange
            const owner = accounts[0];
            const countToSend = 100;
            const accountToReceive = accounts[1];
            //act
            await contract.transfer(accountToReceive, countToSend);
            //assert
            let balance = await contract.balanceOf(accountToReceive);
            balance.toNumber().should.equal(countToSend);
            let ownerBalance = await contract.balanceOf(owner);
            ownerBalance.toNumber().should.equal(settings.initialSupply - countToSend);
            let totalSupply = await contract.totalSupply();
            totalSupply.toNumber().should.equal(settings.initialSupply);
        });

        it('should send tokens to another account', async function () {
            //arrange
            const countToSend = 100;
            const accountToReceive = accounts[1];
            //act
            await contract.transfer(accountToReceive, countToSend);
            //assert
            let balance = await contract.balanceOf(accountToReceive);
            balance.toNumber().should.equal(countToSend);
        });

        it('should not send tokens if no allowance for owner', async function () {
            //arrange
            const countToSend = 100;
            const owner = accounts[0];
            const accountToReceive = accounts[2];
            //act
            try {
                await contract.transferFrom(owner, accountToReceive, countToSend);
                throw "succeeded";
            }
            catch (error) {
                //assert
                error.should.not.equal("succeeded");
            }
        });

        it('should not send tokens if no allowance', async function () {
            //arrange
            const countToSend = 100;
            const accountToSend = accounts[1];
            const accountToReceive = accounts[2];
            //act
            try {
                await contract.transferFrom(accountToSend, accountToReceive, countToSend);
                throw "succeeded";
            }
            catch (error) {
                //assert
                error.should.not.equal("succeeded");
            }
        });

        it('should be transferable from owner', async function () {
            //arrange
            const countToSend = 100;
            const owner = accounts[0];
            const accountToReceive = accounts[2];
            //act
            await contract.approve(owner, countToSend);
            await contract.transferFrom(owner, accountToReceive, countToSend);
            //assert
            let balance = await contract.balanceOf(accountToReceive);
            balance.toNumber().should.equal(countToSend);
        });

        it('should send tokens if allowance', async function () {
            //arrange
            const countToSend = 100;
            const accountToSend = accounts[1];
            const accountToReceive = accounts[2];
            await contract.approve(accountToSend, countToSend);
            //act
            await contract.transferFrom(accounts[0], accountToReceive, countToSend, { from: accountToSend });
            //assert
            let balance = await contract.balanceOf(accountToReceive);
            balance.toNumber().should.equal(countToSend);
        });
        

        it('should not increase allowance when allowance is not zero', async function () {
            //arrange
            const countToSend = 100;
            const accountToSend = accounts[1];
            //act
            await contract.approve(accountToSend, countToSend);

            try {
                await contract.approve(accountToSend, countToSend);
                throw "succeeded";
            }
            catch (error) {
                //assert
                error.should.not.equal("succeeded");
            }
        });

        it('should increase allowance when allowance is decreased to zero', async function () {
            //arrange
            const countToSend = 100;
            const accountToSend = accounts[1];
            //act
            await contract.approve(accountToSend, countToSend);
            await contract.approve(accountToSend, 0);
            await contract.approve(accountToSend, countToSend);
            //assert
            let allowanceBalance = await contract.allowance(accounts[0], accounts[1]);
            allowanceBalance.toNumber().should.equal(countToSend);
        });

    });

});