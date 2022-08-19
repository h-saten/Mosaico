const BigNumber = web3.BigNumber;
const Mosaico = artifacts.require('Mosaico');
require('truffle-test-utils').init();
var utils = web3.utils;
require('chai').use(require('chai-bignumber')(BigNumber)).should();

contract('Mosaico', (accounts) => {
  let contract;
  const _decimals = utils.toBN(18);
  let settings = {
    name: 'Mosaico',
    symbol: 'MOS',
    initialSupply: utils.toBN(180000000),
  };
  const initialSupplyBigInt = BigInt(settings.initialSupply) * 10n ** 18n;

  beforeEach(async function () {
    contract = await Mosaico.new(accounts[1], accounts[2], accounts[0]);
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
      tokenDecimals.should.be.bignumber.eql(_decimals);
    });

    it('has correct total supply', async function () {
      //arrange
      //act
      let totalSupply = await contract.totalSupply();
      //assert
      totalSupply.should.be.bignumber.eql(
        settings.initialSupply.mul(utils.toBN(10).pow(_decimals))
      );
    });

    it('creator has all tokens', async function () {
      //arrange
      //act
      let balance = await contract.balanceOf(accounts[0]);
      //assert
      balance.should.be.bignumber.eql(
        settings.initialSupply.mul(utils.toBN(10).pow(_decimals))
      );
    });
  });

  describe('ERC20 Transfer', function () {
    it('should not change total supply', async function () {
      //arrange
      const owner = accounts[0];
      const countToSend = utils.toBN(100);
      const accountToReceive = accounts[1];
      //act
      await contract.transfer(accountToReceive, countToSend);
      //assert
      let balance = await contract.balanceOf(accountToReceive);
      balance.should.be.bignumber.eql(countToSend);
      let ownerBalance = await contract.balanceOf(owner);
      ownerBalance.should.be.bignumber.eql(
        utils.toBN(
          (initialSupplyBigInt - BigInt(countToSend.toNumber())).toString()
        )
      );
      let totalSupply = await contract.totalSupply();
      totalSupply.should.be.bignumber.eql(
        utils.toBN(initialSupplyBigInt.toString())
      );
    });

    it('should send tokens to another account', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const accountToReceive = accounts[1];
      //act
      await contract.transfer(accountToReceive, countToSend);
      //assert
      let balance = await contract.balanceOf(accountToReceive);
      balance.should.be.bignumber.eql(countToSend);
    });

    it('should not send tokens if no allowance for owner', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const owner = accounts[0];
      const accountToReceive = accounts[2];
      //act
      try {
        await contract.transferFrom(owner, accountToReceive, countToSend);
        throw 'succeeded';
      } catch (error) {
        //assert
        error.should.not.equal('succeeded');
      }
    });

    it('should not send tokens if no allowance', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const accountToSend = accounts[1];
      const accountToReceive = accounts[2];
      //act
      try {
        await contract.transferFrom(
          accountToSend,
          accountToReceive,
          countToSend
        );
        throw 'succeeded';
      } catch (error) {
        //assert
        error.should.not.equal('succeeded');
      }
    });

    it('should be transferable from owner', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const owner = accounts[0];
      const accountToReceive = accounts[2];
      //act
      await contract.approve(owner, countToSend);
      await contract.transferFrom(owner, accountToReceive, countToSend);
      //assert
      let balance = await contract.balanceOf(accountToReceive);
      balance.should.be.bignumber.eql(countToSend);
    });

    it('should send tokens if allowance', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const accountToSend = accounts[1];
      const accountToReceive = accounts[2];
      await contract.approve(accountToSend, countToSend);
      //act
      await contract.transferFrom(accounts[0], accountToReceive, countToSend, {
        from: accountToSend,
      });
      //assert
      let balance = await contract.balanceOf(accountToReceive);
      balance.should.be.bignumber.eql(countToSend);
    });

    it('should increase allowance', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const accountToSend = accounts[1];
      const accountToReceive = accounts[2];
      await contract.increaseAllowance(accountToSend, countToSend);
      //act
      await contract.transferFrom(accounts[0], accountToReceive, countToSend, {
        from: accountToSend,
      });
      //assert
      let balance = await contract.balanceOf(accountToReceive);
      balance.should.be.bignumber.eql(countToSend);
    });

    it('should fail if allowance not enough', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const accountToSend = accounts[1];
      const accountToReceive = accounts[2];
      await contract.increaseAllowance(accountToSend, countToSend);
      //act
      try {
        await contract.transferFrom(
          accounts[0],
          accountToReceive,
          countToSend.add(utils.toBN(1)),
          { from: accountToSend }
        );
        throw 'succeeded';
      } catch (error) {
        //assert
        error.should.not.equal('succeeded');
      }
    });

    it('should decrease allowance', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const decreaseBy = utils.toBN(20);
      console.log(accounts);

      const accountToSend = accounts[1];
      console.log(`Account to send: ${accountToSend}`);

      const accountToReceive = accounts[2];
      console.log(`Account to receive: ${accountToReceive}`);

      //on dev there are only 3 accounts
      const accountToSpend = accounts[0];
      console.log(`Account to spend: ${accountToSpend}`);

      await contract.transfer(accountToSend, countToSend);
      await contract.increaseAllowance(accountToSpend, countToSend, {
        from: accountToSend,
      });
      await contract.decreaseAllowance(accountToSpend, decreaseBy, {
        from: accountToSend,
      });
      //act
      try {
        const all = await contract.allowance(accountToSend, accountToSpend);
        console.log(`Current allowance - ${all.should.be.bignumber}`);
        await contract.transferFrom(
          accountToSend,
          accountToReceive,
          utils.toBN(countToSend),
          { from: accountToSpend }
        );
        throw 'succeeded';
      } catch (error) {
        //assert
        error.should.not.equal('succeeded');
      }
      const canSendCount = utils.toBN(80);
      await contract.transferFrom(
        accountToSend,
        accountToReceive,
        canSendCount,
        { from: accountToSpend }
      );
      let balance = await contract.balanceOf(accountToReceive);
      //79 because of fees
      balance.should.be.bignumber.eql(utils.toBN(79));
    });
  });

  describe('ERC20 Events', function () {
    it('should emit transfer event', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const accountToReceive = accounts[1];
      //act
      let result = await contract.transfer(accountToReceive, countToSend);
      assert.web3Event(
        result,
        {
          event: 'Transfer',
        },
        'The event is emitted'
      );
    });

    it('should emit approval event', async function () {
      //arrange
      const countToSend = utils.toBN(100);
      const accountToSend = accounts[1];
      await contract.transfer(accountToSend, countToSend);
      //act
      let result = await contract.approve(accountToSend, countToSend);
      //assert
      assert.web3Event(
        result,
        {
          event: 'Approval',
        },
        'The event is emitted'
      );
    });
  });

  describe('ERC20 Ownable', function () {
    it('should be owned by creator', async function () {
      //arrange
      const ownerAddress = accounts[0];
      //act
      let owner = await contract.owner();
      //assert
      owner.should.equal(ownerAddress);
    });
  });

  describe('ERC20 Mintable', function () {
    it('should be able to mint by owner', async function () {
      // Not passed
      //arrange
      const mintedAmount = utils.toBN(10000);
      const ownerAddress = accounts[0];
      //act
      await contract.mint(mintedAmount, ownerAddress);
      //assert
      let balance = await contract.balanceOf(ownerAddress);
      balance.should.be.bignumber.eql(
        utils.toBN(
          (initialSupplyBigInt + BigInt(mintedAmount.toNumber())).toString()
        )
      );
    });

    it('should not be able to mint by someone', async function () {
      //arrange
      const mintedAmount = utils.toBN(10000);
      const address = accounts[1];
      //act
      try {
        await contract.mint(mintedAmount, accounts[0], { from: address });
        throw 'succeeded';
      } catch (error) {
        error.should.not.equal('succeeded');
      }
    });
  });

  describe('ERC20 Burnable', function () {
    it('should be able to burn by owner', async function () {
      //arrange
      const burnedAmount = utils.toBN(10000);
      const ownerAddress = accounts[0];
      //act
      await contract.burn(utils.toBN(burnedAmount));
      //assert
      let balance = await contract.balanceOf(ownerAddress);
      balance.should.be.bignumber.eql(
        utils.toBN(
          (initialSupplyBigInt - BigInt(burnedAmount.toNumber())).toString()
        )
      );
    });

    it('should be able to burn by someone', async function () {
      //arrange
      const initialBudget = utils.toBN(10001);
      const burnedAmount = utils.toBN(10000);
      const address = accounts[1];
      await contract.transfer(address, initialBudget);
      //act
      await contract.burn(utils.toBN(burnedAmount), { from: address });
      //assert
      let balance = await contract.balanceOf(address);
      balance.should.be.bignumber.eql(
        utils.toBN(initialBudget.toNumber() - burnedAmount.toNumber())
      );
    });

    it('should not be able to burn more than balance', async function () {
      //arrange
      const initialBudget = utils.toBN(10001);
      const burnedAmount = utils.toBN(10002);
      const address = accounts[1];
      await contract.transfer(address, initialBudget);
      //act
      try {
        await contract.burn(burnedAmount, { from: address });
        throw 'succeeded';
      } catch (error) {
        //assert
        error.should.not.equal('succeeded');
      }
    });
  });
});
