const { assert } = require("chai");

const MosaicoERC20v1 = artifacts.require("MosaicoERC20v1");
const Crowdsale = artifacts.require("DefaultCrowdsalev1");

var utils = web3.utils;

require("truffle-test-utils").init();

require("chai").should();

asBN = (value) => utils.toBN(value);
asEther = (value, decimals = 18) =>
  asBN(value).mul(asBN(10).pow(asBN(decimals)));
asEtherString = (value, decimals = 18) => asEther(value, decimals).toString();


contract("DefaultCrowdsalev1", (accounts) => {
  const usdtIssuer = accounts[1];
  const companyWallet = accounts[2];

  const tokenSettings = {
    name: "Mosaico ERC20 Test Token",
    symbol: "MOSTT",
    initialSupply: asEtherString(10000000),
    isMintable: false,
    isBurnable: false,
    isPaused: false,
    walletAddress: companyWallet,
    isGovernance: false,
  };

  const usdtTokenSettings = {
    name: "Tether",
    symbol: "USDT",
    initialSupply: asEtherString(10000000),
    isMintable: true,
    isBurnable: true,
    isPaused: false,
    walletAddress: usdtIssuer,
    isGovernance: false,
  };

  const usdcTokenSettings = {
    name: "USD Coin",
    symbol: "USDC",
    initialSupply: asEtherString(10000000),
    isMintable: true,
    isBurnable: true,
    isPaused: false,
    walletAddress: usdtIssuer,
    isGovernance: false,
  };

  const crowdsaleStages = [
    {
      name: "Private Sale",
      isPrivate: false,
      cap: asEtherString(1000),
      minIndividualCap: asEtherString(10),
      maxIndividualCap: asEtherString(500),
      whitelist: [],
      rate: asEtherString(20),
    },
    {
      name: "Pre Sale",
      isPrivate: false,
      cap: asEtherString(10000),
      minIndividualCap: asEtherString(10),
      maxIndividualCap: asEtherString(10000),
      whitelist: [],
      rate: 40,
    },
    {
      name: "Public Sale",
      isPrivate: false,
      cap: asEtherString(1000),
      minIndividualCap: asEtherString(10),
      maxIndividualCap: asEtherString(1000),
      whitelist: [],
      rate: 60,
    },
  ];

  const crowdsaleStagesStableCoinRates = [2, 4, 6];

  const numberOfStages = crowdsaleStages.length;
  let token;
  let usdtToken;
  let usdcToken;
  let crowdsale;
  let currentStage = crowdsaleStages[0];

  const crowdsaleHardCap = crowdsaleStages
    .map((stageEntry) => stageEntry.cap * stageEntry.rate)
    .reduce((summary, stageSummary) => summary + stageSummary);
  const crowdsaleSoftCap = (crowdsaleHardCap * 10) / 100;

  beforeEach(async function () {
    token = await MosaicoERC20v1.new(tokenSettings);
    usdtToken = await MosaicoERC20v1.new(usdtTokenSettings);
    usdcToken = await MosaicoERC20v1.new(usdcTokenSettings);
    crowdsale = await Crowdsale.new(
      companyWallet,
      token.address,
      numberOfStages,
      10,
      [usdtToken.address, usdcToken.address]
    );

    await crowdsale.startNextStage(
      currentStage,
      crowdsaleStagesStableCoinRates[0]
    );
    await token.transfer(crowdsale.address, currentStage.cap, {
      from: companyWallet,
    });
  });

  describe("Crowdsale deploy", function () {
    it("crowdsale contract should be allowed to transfer contract in stage", async () => {
      //arrange
      const crowdsaleAddress = crowdsale.address;
      //act
      const crowdsaleTokensBalance = await token.balanceOf(crowdsaleAddress);
      //assert
      assert.isTrue(crowdsaleTokensBalance.eq(asBN(currentStage.cap)));
    });

    it("first stage should be Private sale", async () => {
      const stageName = await crowdsale.stage();
      stageName.should.equal("Private Sale");
    });

    it("first cap should be 1000", async () => {
      const cap = await crowdsale.cap();
      assert.isTrue(asBN(cap).eq(asBN(currentStage.cap)));
    });

    it("should initial wallet balance of users except crowdsale contract be 0", async () => {
      //arrange
      const beneficiary = accounts[1];
      //act
      const beneficiaryBalance = await token.balanceOf(beneficiary);
      //assert
      assert.isTrue(beneficiaryBalance.eq(asBN(0)));
    });
  });

  describe("Crowdsale rate", function () {
    it("initial crowdsale rate is first array value", async function () {
      //act
      const rate = await crowdsale.getRate();
      //assert
      assert.isTrue(rate.eq(asBN(currentStage.rate)));
    });

    it("should change rate to the next one", async function () {
      //arrange
      const nextStage = crowdsaleStages[1];
      const stageStablecoinsRate = crowdsaleStagesStableCoinRates[1];
      //act
      await crowdsale.startNextStage(nextStage, stageStablecoinsRate);
      await token.transfer(crowdsale.address, nextStage.cap, {
        from: companyWallet,
      });
      //assert
      const rate = await crowdsale.getRate();
      assert.isTrue(rate.eq(asBN(nextStage.rate)));
    });

    it("should abort next rate change for other address than owner", async () => {
      //arrange
      const account = accounts[1];
      const nextStage = crowdsaleStages[1];
      const stageStablecoinsRate = crowdsaleStagesStableCoinRates[1];
      //act
      try {
        await crowdsale.startNextStage(nextStage, stageStablecoinsRate, {
          from: account,
        });
        throw "succeeded";
      } catch (error) {
        //assert
        error.should.not.equal("succeeded");
      }
    });
  });

  describe("Crowdsale purchase", function () {
    it("should transfer 20 tokens to beneficiary", async () => {
      //arrange
      const beneficiary = accounts[3];
      // const quantity = asBN(1);
      // const expected = quantity.mul(asBN(currentStage.rate));

      const quantity = asEther(400, 18);
      const expected = asEther(quantity).div(asBN(currentStage.rate));
      //act
      await crowdsale.buyTokens(beneficiary, { value: quantity });
      var beneficiaryBalance = await crowdsale.balanceOf(beneficiary);
      //asser
      assert.isTrue(beneficiaryBalance.eq(expected));
    });

    it("should transfer 20 tokens via fallback function execution", async () => {
      //arrange
      const beneficiary = accounts[3];
      const quantity = asEther(400, 18);
      const expected = asEther(quantity).div(asBN(currentStage.rate));
      //act
      await web3.eth
        .sendTransaction({
          from: beneficiary,
          to: crowdsale.address,
          value: quantity,
          gas: "1000000",
        })
        .on("receipt", function () {});

      const beneficiaryBalance = await crowdsale.balanceOf(beneficiary);

      //assert
      assert.isTrue(beneficiaryBalance.eq(expected));
    });

    it("transfer should fail if private sale and not whitelisted", async () => {
      //arrange
      const stage = {
        name: "Private Sale",
        isPrivate: true,
        cap: 1000,
        minIndividualCap: 10,
        maxIndividualCap: 1000,
        whitelist: [],
        rate: 20,
      };
      //crowdsale = await Crowdsale.new(companyWallet, token.address, numberOfStages, 10, [usdtToken.address, usdcToken.address]);

      const privateSaleContract = await Crowdsale.new(
        companyWallet,
        token.address,
        1,
        10,
        [usdtToken.address, usdcToken.address]
      );
      await privateSaleContract.startNextStage(stage, 2);
      await token.transfer(crowdsale.address, stage.cap, {
        from: companyWallet,
      });

      const buyer = accounts[3];
      const quantity = asBN(200);

      //act
      try {
        let isWhitelisted = await privateSaleContract.isWhitelisted(buyer);
        isWhitelisted.should.equal(false);
        await crowdsale.buyTokens(buyer, { value: quantity }, { from: buyer });
        throw "succeeded";
      } catch (error) {
        error.should.not.equal("succeeded");
      }
    });

    it("should trasfer if private sale and whitelisted stage", async () => {
      //arrange
      const buyer = accounts[3];
      const stage = {
        name: "Private Sale",
        isPrivate: true,
        cap: 1000,
        minIndividualCap: 10,
        maxIndividualCap: 1000,
        whitelist: [buyer],
        rate: 20,
      };
      const privateSaleContract = await Crowdsale.new(
        companyWallet,
        token.address,
        numberOfStages,
        10,
        [usdtToken.address, usdcToken.address]
      );
      await privateSaleContract.startNextStage(stage, 2);
      await token.transfer(crowdsale.address, stage.cap, {
        from: companyWallet,
      });

      const quantity = asEther(400, 18);
      const expected = asEther(quantity).div(asBN(currentStage.rate));

      //act
      await crowdsale.buyTokens(buyer, { from: buyer, value: quantity });

      //assert
      const beneficiaryBalance = await crowdsale.balanceOf(buyer);
      assert.isTrue(beneficiaryBalance.eq(expected));
    });
  });

  describe("Crowdsale whitelisting", function () {
    it("owner should be whitelisted admin", async () => {
      //arrange
      const owner = accounts[0];
      //act
      const ownerResult = await crowdsale.isWhitelistAdmin(owner);
      //assert
      ownerResult.should.equal(true);
    });

    it("whitelisted admin should add to whitelist", async () => {
      //arrange
      let someAccount = accounts[1];
      //act
      await crowdsale.addWhitelisted([someAccount]);
      //assert
      let isWhitelisted = await crowdsale.isWhitelisted(someAccount);
      isWhitelisted.should.equal(true);
    });

    it("not a whitelisted admin should not add to whitelist", async () => {
      //arrange
      const owner = accounts[0];
      const someAccount = accounts[1];
      //act
      try {
        await crowdsale.addWhitelisted([owner], { from: someAccount });
        throw "succeeded";
      } catch (error) {
        //assert
        error.should.not.equal("succeeded");
      }
    });
  });

  describe("Crowdsale cap", function () {
    it("should not buy if cap reached", async () => {
      //arrange
      const beneficiary = accounts[3];
      const quantity = asBN(10020);
      //act
      try {
        await crowdsale.buyTokens(beneficiary, {
          value: quantity,
          from: beneficiary,
        });
        throw "succeeded";
      } catch (error) {
        error.should.not.equal("succeeded");
      }
    });
  });

  describe("Crowdsale pausable", function () {
    it("should be unpaused when created", async () => {
      //arrange
      //act
      let isPaused = await crowdsale.paused();
      //assert
      isPaused.should.be.equal(false);
    });

    it("should be paused by owner", async () => {
      //arrange
      //act
      await crowdsale.pause();
      //assert
      let isPaused = await crowdsale.paused();
      isPaused.should.equal(true);
    });

    it("should not be paused by someone", async () => {
      //arrange
      let someone = accounts[1];
      //act
      try {
        await crowdsale.pause({ from: someone });
        throw "succeeded";
      } catch (error) {
        //assert
        error.should.not.equal("succeeded");
      }
      //assert
      let isPaused = await crowdsale.paused();
      isPaused.should.equal(false);
    });

    it("should not be paused if already paused", async () => {
      //arrange
      await crowdsale.pause();
      //act
      try {
        await crowdsale.pause();
        throw "succeeded";
      } catch (error) {
        //assert
        error.should.not.equal("succeeded");
      }
      //assert
      let isPaused = await crowdsale.paused();
      isPaused.should.equal(true);
    });

    it("should not be unpaused when next stage", async () => {
      //arrange
      const stage = crowdsaleStages[1];
      await crowdsale.pause();
      const stageStablecoinsRate = crowdsaleStagesStableCoinRates[1];
      //act
      await crowdsale.startNextStage(stage, stageStablecoinsRate);
      await token.transfer(crowdsale.address, stage.cap, {
        from: companyWallet,
      });
      //assert
      let isPaused = await crowdsale.paused();
      isPaused.should.equal(false);
    });
  });
});
