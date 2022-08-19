const BigNumber = web3.BigNumber;
const MosaicoERC20v1 = artifacts.require("MosaicoERC20v1");
require('truffle-test-utils').init();
var utils = web3.utils;
require('chai').use(require('chai-bignumber')(BigNumber)).should();

contract('MosaicoERC20v1', accounts => {
    const settings = {
        name: "Mosaico ERC20 Test Token",
        symbol: "MOSTT",
        initialSupply: 1000000,
        isMintable: false,
        isBurnable: false,
        isPaused: false,
        walletAddress: accounts[0],
        isGovernance: false
    };
    const _decimals = 18;

    let contract;

    beforeEach(async function () {
        contract = await MosaicoERC20v1.new(settings);
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

        it('should increase allowance', async function () {
            //arrange
            const countToSend = 100;
            const accountToSend = accounts[1];
            const accountToReceive = accounts[2];
            await contract.increaseAllowance(accountToSend, countToSend);
            //act
            await contract.transferFrom(accounts[0], accountToReceive, countToSend, { from: accountToSend });
            //assert
            let balance = await contract.balanceOf(accountToReceive);
            balance.toNumber().should.equal(countToSend);
        });

        it('should fail if allowance not enough', async function () {
            //arrange
            const countToSend = 100;
            const accountToSend = accounts[1];
            const accountToReceive = accounts[2];
            await contract.increaseAllowance(accountToSend, countToSend);
            //act
            try {
                await contract.transferFrom(accounts[0], accountToReceive, countToSend + 1, { from: accountToSend });
                throw "succeeded";
            }
            catch (error) {
                //assert
                error.should.not.equal("succeeded")
            }
        });

        it('should decrease allowance', async function () {
            //arrange
            const countToSend = 100;
            const decreaseBy = 20;
            console.log(accounts);

            const accountToSend = accounts[1];
            console.log(`Account to send: ${accountToSend}`);

            const accountToReceive = accounts[2];
            console.log(`Account to receive: ${accountToReceive}`);

            //on dev there are only 3 accounts
            const accountToSpend = accounts[0];
            console.log(`Account to spend: ${accountToSpend}`);

            await contract.transfer(accountToSend, countToSend);
            await contract.increaseAllowance(accountToSpend, countToSend, { from: accountToSend });
            await contract.decreaseAllowance(accountToSpend, decreaseBy, { from: accountToSend });
            //act
            try {
                const all = await contract.allowance(accountToSend, accountToSpend);
                console.log(`Current allowance - ${all.toNumber()}`);
                await contract.transferFrom(accountToSend, accountToReceive, countToSend, { from: accountToSpend });
                throw "succeeded";
            }
            catch (error) {
                //assert
                error.should.not.equal("succeeded")
            }
            await contract.transferFrom(accountToSend, accountToReceive, countToSend - decreaseBy, { from: accountToSpend });
            let balance = await contract.balanceOf(accountToReceive);
            balance.toNumber().should.equal(countToSend - decreaseBy);
        });

    });

    describe('ERC20 Events', function () {
        it('should emit transfer event', async function () {
            //arrange
            const countToSend = 100;
            const accountToReceive = accounts[1];
            //act
            let result = await contract.transfer(accountToReceive, countToSend);
            assert.web3Event(result, {
                event: 'Transfer'
            }, 'The event is emitted'
            );
        });

        it('should emit approval event', async function () {
            //arrange
            const countToSend = 100;
            const accountToSend = accounts[1];
            await contract.transfer(accountToSend, countToSend);
            //act
            let result = await contract.approve(accountToSend, countToSend);
            //assert
            assert.web3Event(result, {
                event: 'Approval'
            }, 'The event is emitted'
            );
        });
    });

    describe('ERC20 Pausable', function () {
        it('should be unpaused', async function () {
            //arrange
            //act
            let isPaused = await contract.paused();
            //assert
            isPaused.should.equal(false);
        });

        it('should be paused by owner', async function () {
            //arrange
            //act
            try {
                await contract.pause();
                throw "succeeded";
            }
            catch (error) {
                error.should.equal("succeeded");
            }
            //assert
            let isPaused = await contract.paused();
            isPaused.should.equal(true);
        });

        it('should be not be paused again', async function () {
            //arrange
            await contract.pause();
            //act
            try {
                await contract.pause();
                throw "succeeded";
            }
            catch (error) {
                error.should.not.equal("succeeded");
            }
            //assert
            let isPaused = await contract.paused();
            isPaused.should.equal(true);
        });

        it('should be not be unpaused again', async function () {
            //arrange
            //act
            try {
                await contract.unpause();
                throw "succeeded";
            }
            catch (error) {
                error.should.not.equal("succeeded");
            }
            //assert
            let isPaused = await contract.paused();
            isPaused.should.equal(false);
        });

        it('should be not be paused by someone', async function () {
            //arrange
            const accountToPause = accounts[1];
            //act
            try {
                await contract.pause({ from: accountToPause });
                throw "succeeded";
            }
            catch (error) {
                error.should.not.equal("succeeded");
            }
            //assert
            let isPaused = await contract.paused();
            isPaused.should.equal(false);
        });

        it('should be unpaused by owner', async function () {
            //arrange
            await contract.pause();
            //act
            try {
                await contract.unpause();
                throw "succeeded";
            }
            catch (error) {
                error.should.equal("succeeded");
            }
            //assert
            let isPaused = await contract.paused();
            isPaused.should.equal(false);
        });

        it('should be not be unpaused by someone', async function () {
            //arrange
            const accountToPause = accounts[1];
            await contract.pause();
            //act
            try {
                await contract.unpause({ from: accountToPause });
                throw "succeeded";
            }
            catch (error) {
                error.should.not.equal("succeeded");
            }
            //assert
            let isPaused = await contract.paused();
            isPaused.should.equal(true);
        });

        it('should emit paused event', async function () {
            //arrange
            //act
            let result = await contract.pause();
            //assert
            assert.web3Event(result, {
                event: 'Paused'
            }, 'The event is emitted'
            );
        });

        it('should emit paused event', async function () {
            //arrange
            await contract.pause();
            //act
            let result = await contract.unpause();
            //assert
            assert.web3Event(result, {
                event: 'Unpaused'
            }, 'The event is emitted'
            );
        });

    });

    describe('ERC20 Ownable', function () {
        it('should be owned by creator', async function () {
            //arrange
            const ownerAddress = accounts[0];
            //act
            let owner = await contract.getOwners();
            //assert
            owner[0].should.equal(ownerAddress);
        });
    });

    describe('ERC20 Mintable', function () {
        it('should not be able to mint when disabled', async function () {
            //arrange
            let mintedAmount = 10000;
            //act
            try {
                await contract.mint(utils.toBN(mintedAmount), accounts[0]);
                throw "succeeded";
            }
            catch (error) {
                error.should.not.equal("succeeded");
            }
        });

        it('should be able to mint by owner', async function () {
            //arrange
            const mintedAmount = 10000;
            let mintableToken = await MosaicoERC20v1.new({ ...settings, isMintable: true });
            const ownerAddress = accounts[0];
            //act
            await mintableToken.mint(utils.toBN(mintedAmount), ownerAddress);
            //assert
            let balance = await mintableToken.balanceOf(ownerAddress);
            balance.toNumber().should.equal(settings.initialSupply + mintedAmount);
        });

        it('should not be able to mint by someone', async function () {
            //arrange
            const mintedAmount = 10000;
            let mintableToken = await MosaicoERC20v1.new({ ...settings, isMintable: true });
            const address = accounts[1];
            //act
            try {
                await mintableToken.mint(utils.toBN(mintedAmount), accounts[0], { from: address });
                throw "succeeded";
            }
            catch (error) {
                error.should.not.equal("succeeded");
            }
        });

        it('should be minted to sender account if account has not been specified', async function () {
            //arrange
            const address = accounts[0];
            const emptyAddress = '0x0000000000000000000000000000000000000000';

            //act
            let mintableToken = await MosaicoERC20v1.new({ ...settings, walletAddress: emptyAddress });
                
            //assert
            let balance = await mintableToken.balanceOf(address);
            balance.toNumber().should.equal(settings.initialSupply);
        });

        it('should be able to mint by a particular wallet address', async function () {
            //arrange
            const mintedAmount = 30000;
            const sourceAddress = accounts[0];
            const companyWalletAddress = accounts[2];
            
            //act
            let mintableToken = await MosaicoERC20v1.new({ ...settings, walletAddress: companyWalletAddress, isMintable: true });
            await mintableToken.mint(utils.toBN(mintedAmount), companyWalletAddress, { from: companyWalletAddress });
            
            //assert
            let balance = await mintableToken.balanceOf(companyWalletAddress);
            balance.toNumber().should.equal(settings.initialSupply + mintedAmount);
        });
    });

    describe('ERC20 Burnable', function () {
        it('should not be able to burn when disabled', async function () {
            //arrange
            let burnedAmount = 10000;
            //act
            try {
                await contract.mint(utils.toBN(burnedAmount), accounts[0]);
                throw "succeeded";
            }
            catch (error) {
                error.should.not.equal("succeeded");
            }
        });

        it('should be able to burn by owner', async function () {
            //arrange
            const burnedAmount = 10000;
            let burnableToken = await MosaicoERC20v1.new({ ...settings, isBurnable: true });
            const ownerAddress = accounts[0];
            //act
            await burnableToken.burn(utils.toBN(burnedAmount));
            //assert
            let balance = await burnableToken.balanceOf(ownerAddress);
            balance.toNumber().should.equal(settings.initialSupply - burnedAmount);
        });

        it('should be able to burn by someone', async function () {
            //arrange
            const initialBudget = 10001;
            const burnedAmount = 10000;
            let burnableToken = await MosaicoERC20v1.new({ ...settings, isBurnable: true });
            const address = accounts[1];
            await burnableToken.transfer(address, initialBudget);
            //act
            await burnableToken.burn(utils.toBN(burnedAmount), { from: address });
            //assert
            let balance = await burnableToken.balanceOf(address);
            balance.toNumber().should.equal(initialBudget - burnedAmount);
        });

        it('should not be able to burn more than balance', async function () {
            //arrange
            const initialBudget = 10001;
            const burnedAmount = 10002;
            let burnableToken = await MosaicoERC20v1.new({ ...settings, isBurnable: true });
            const address = accounts[1];
            await burnableToken.transfer(address, initialBudget);
            //act
            try {
                await burnableToken.burn(utils.toBN(burnedAmount), { from: address });
                throw "succeeded";
            }
            catch (error) {
                //assert
                error.should.not.equal("succeeded");
            }
        });

        it('should be able to burn if allowance', async function () {
            //arrange
            const initialBudget = 10001;
            const burnedAmount = 10000;
            let burnableToken = await MosaicoERC20v1.new({ ...settings, isBurnable: true });
            const address = accounts[1];
            const ownerAddress = accounts[0];
            await burnableToken.increaseAllowance(address, initialBudget);
            //act        
            await burnableToken.burnFrom(ownerAddress, utils.toBN(burnedAmount), { from: address });
            //assert
            let allowance = await burnableToken.allowance(ownerAddress, address);
            allowance.toNumber().should.equal(initialBudget - burnedAmount);
        });

        it('should not be able to burn if not allowance', async function () {
            //arrange
            const initialBudget = 10001;
            const burnedAmount = 10002;
            let burnableToken = await MosaicoERC20v1.new({ ...settings, isBurnable: true });
            const address = accounts[1];
            const ownerAddress = accounts[0];
            await burnableToken.increaseAllowance(address, initialBudget);
            //act
            try{
                await burnableToken.burnFrom(ownerAddress, utils.toBN(burnedAmount), { from: address });
                throw "succeeded";
            }
            catch(error){
                //assert
                error.should.not.equal("succeeded");
            }
        });
    });

    describe('ERC20 Batch Transfer', function () {
        it('should send to all', async function () {
            await contract.batchTransferFrom(accounts[0], [accounts[1], accounts[2]], [utils.toBN(10), utils.toBN(10)]);
            let balance = await contract.balanceOf(accounts[1]);
            //assert
            balance.toNumber().should.equal(10);
        });
    });
});