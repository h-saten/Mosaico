const { assert } = require("chai");
const { type } = require("chai/lib/chai/utils");

const MosaicoERC20v1 = artifacts.require("MosaicoERC20v1");
const Crowdsale = artifacts.require("DefaultCrowdsalev1");
const TetherToken = artifacts.require("TetherToken");

var utils = web3.utils;

require("truffle-test-utils").init();

require("chai").should();
asBN = (value) => utils.toBN(value);
asEther = (value, decimals = 18) =>
  asBN(value).mul(asBN(10).pow(asBN(decimals)));
asEtherString = (value, decimals = 18) => asEther(value, decimals).toString();

contract("Crowdsale", (accounts) => {
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
      rate: asEtherString(40),
    },
    {
      name: "Public Sale",
      isPrivate: false,
      cap: asEtherString(1000),
      minIndividualCap: asEtherString(10),
      maxIndividualCap: asEtherString(1000),
      whitelist: [],
      rate: asEtherString(60),
    },
  ];

  const crowdsaleStagesStableCoinRates = [asEther(2), asEther(4), asEther(6)];

  const numberOfStages = crowdsaleStages.length;
  let token;
  let usdtToken;
  let usdcToken;
  let crowdsale;
  let currentStage = crowdsaleStages[0];

  const softCapDenominator = 30;

  beforeEach(async function () {
    token = await MosaicoERC20v1.new(tokenSettings);
    usdtToken = await MosaicoERC20v1.new(usdtTokenSettings);
    usdcToken = await MosaicoERC20v1.new(usdcTokenSettings);
    crowdsale = await Crowdsale.new(
      companyWallet,
      token.address,
      numberOfStages,
      softCapDenominator,
      [usdtToken.address]
    );

    await crowdsale.startNextStage(
      currentStage,
      crowdsaleStagesStableCoinRates[0]
    );
    await token.increaseAllowance(crowdsale.address, currentStage.cap, {
      from: companyWallet,
    });
  });

  describe("Crowdsale with exchange", function () {
    it("should transfer valid tokens amount to user and deposit usdt on contract address", async () => {
      //arrange
      const paymentCurrency = usdtToken;
      const stageIndex = 1;
      const buyer = accounts[stageIndex];
      await crowdsale.pause();
      const stage = crowdsaleStages[stageIndex];
      const stablecoinExchangeRate = crowdsaleStagesStableCoinRates[stageIndex];

      await crowdsale.startNextStage(stage, stablecoinExchangeRate);
      await token.increaseAllowance(crowdsale.address, stage.cap, {
        from: companyWallet,
      });

      var usdtAmount = asEther(40);
      // top up buyer wallet
      var bal = await usdtToken.balanceOf(usdtIssuer);
      console.log("iss balance: ", bal.toString());
      console.log(
        "total supply balance: ",
        (await usdtToken.totalSupply()).toString()
      );
      await usdtToken.transfer(buyer, usdtAmount, { from: usdtIssuer });

      // allow crowdsale contract to manage user usdt - payment transaction purpose
      await usdtToken.increaseAllowance(crowdsale.address, usdtAmount, {
        from: buyer,
      });

      //act
      await crowdsale.exchangeTokens(paymentCurrency.address, usdtAmount, {
        from: buyer,
      });

      const contractUsdtBalance = await usdtToken.balanceOf(crowdsale.address);
      const expectedBuyerTokenBalance = asEther(
        usdtAmount.div(stablecoinExchangeRate)
      );
      const buyerTokenBalance = await crowdsale.balanceOf(buyer);

      //assert
      assert.isTrue(buyerTokenBalance.eq(expectedBuyerTokenBalance));
      assert.isTrue(contractUsdtBalance.eq(asBN(usdtAmount)));
    });

    it("should revert when exchange tokens amount is bigger than stage tokens cap", async () => {
      //arrange
      const paymentCurrency = usdtToken;
      const buyer = accounts[1];
      await crowdsale.pause();
      const stageIndex = 1;
      const stage = crowdsaleStages[stageIndex];
      const stablecoinExchangeRate = crowdsaleStagesStableCoinRates[stageIndex];
      await crowdsale.startNextStage(stage, stablecoinExchangeRate);
      const crowdsaleTokenOwnerBalance = await token.balanceOf(companyWallet);

      var usdtAmount = asBN(stage.cap)
        .mul(stablecoinExchangeRate)
        .div(asBN(10).pow(asBN(18)))
        .add(stablecoinExchangeRate);
        
      await usdtToken.transfer(buyer, usdtAmount, { from: usdtIssuer });
      await usdtToken.increaseAllowance(crowdsale.address, usdtAmount, {
        from: buyer,
      });

      assert.isTrue(
        crowdsaleTokenOwnerBalance.eq(asBN(tokenSettings.initialSupply))
      );

      // act
      try {
        await crowdsale.exchangeTokens(paymentCurrency.address, usdtAmount, {
          from: buyer,
        });
        throw "success";
      } catch (error) {
        error.reason.should.equal("Crowdsale: cap exceeded");
      }
    });

    it("should revert when exchange tokens amount is bigger than maximum stage cap per user", async () => {
      //arrange
      const paymentCurrency = usdtToken;
      const buyer = accounts[1];
      const stageIndex = 0;
      const stage = crowdsaleStages[stageIndex];
      const stablecoinExchangeRate = crowdsaleStagesStableCoinRates[stageIndex];

      var usdtAmount = asBN(stage.maxIndividualCap)
        .mul(stablecoinExchangeRate)
        .div(asBN(10).pow(asBN(18)))
        .add(stablecoinExchangeRate);
      await usdtToken.transfer(buyer, usdtAmount, { from: usdtIssuer });
      await usdtToken.increaseAllowance(crowdsale.address, usdtAmount, {
        from: buyer,
      });
      const crowdsaleTokenOwnerBalance = await token.balanceOf(companyWallet);

      assert.isTrue(
        crowdsaleTokenOwnerBalance.eq(asBN(tokenSettings.initialSupply))
      );

      // act
      try {
        await crowdsale.exchangeTokens(paymentCurrency.address, usdtAmount, {
          from: buyer,
        });
        throw "success";
      } catch (error) {
        error.reason.should.equal(
          "Crowdsale: Individual cap limit was not met"
        );
      }
    });
  });
});
