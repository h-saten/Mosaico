const { assert } = require('chai');
const { type } = require('chai/lib/chai/utils');

const MosaicoERC20v1 = artifacts.require("MosaicoERC20v1");
const Crowdsale = artifacts.require("DefaultCrowdsalev1");
const TetherToken = artifacts.require("TetherToken");

var utils = web3.utils;

require('truffle-test-utils').init();

require('chai').should();
asBN = (value) => utils.toBN(value);
asEther = (value, decimals = 18) => asBN(value).mul(asBN(10).pow(asBN(decimals)));
asEtherString = (value, decimals = 18) => asEther(value, decimals).toString();

contract('Crowdsale', accounts => {
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
        url: 'mosaico.ai',
        isGovernance: false
    };

    const usdtTokenSettings = {
        name: "Tether", 
        symbol: "USDT", 
        initialSupply: asEtherString(10000000), 
        isMintable: true,
        isBurnable: true,
        isPaused: false,
        walletAddress: usdtIssuer,
        url: 'mosaico.ai',
        isGovernance: false
    };

    const usdcTokenSettings = {
        name: "USD Coin", 
        symbol: "USDC", 
        initialSupply: asEtherString(10000000), 
        isMintable: true,
        isBurnable: true,
        isPaused: false,
        walletAddress: usdtIssuer,
        url: 'mosaico.ai',
        isGovernance: false
    };

    const crowdsaleStages = [
        {
            name: 'Sale',
            isPrivate: false,
            cap: asEtherString(10000),
            minIndividualCap: asEtherString(10),
            maxIndividualCap: asEtherString(5000),
            whitelist: [],
            rate: asEtherString(5, 14) // 1ETH = $4000
        }
    ];

    const crowdsaleStagesStableCoinRates = [asEther(2)];

    const numberOfStages = crowdsaleStages.length;
    let token;
    let usdtToken;
    let usdcToken;
    let crowdsale;
    let currentStage = crowdsaleStages[0];
    
    const softCapDenominator = 30;

    beforeEach(async function () {
        token = await MosaicoERC20v1.new(tokenSettings);
        usdtToken = await TetherToken.new(usdtTokenSettings.initialSupply, usdtTokenSettings.name, usdtTokenSettings.symbol, 6, {from: usdtIssuer});
        crowdsale = await Crowdsale.new(companyWallet, token.address, numberOfStages, softCapDenominator, [usdtToken.address]);

        await crowdsale.startNextStage(currentStage, crowdsaleStagesStableCoinRates[0]);
        await token.increaseAllowance(crowdsale.address, currentStage.cap, {from: companyWallet});
    });

    describe('Crowdsale with exchange', function() {

        it('should transfer valid tokens amount to user and deposit usdt on contract address', async () => {           
            //arrange
            const paymentCurrency = usdtToken;
            const buyer = accounts[0];
            
            var usdtAmount = asEther(4000, 6);
            // top up buyer wallet
            var bal = await usdtToken.balanceOf(usdtIssuer);
            
            await usdtToken.transfer(buyer, usdtAmount, {from: usdtIssuer});
            
            const currentAllowance = await usdtToken.allowance(buyer, crowdsale.address);
            if (currentAllowance.cmp(asBN(0)) === 1) {
                await usdtToken.approve(crowdsale.address, 0, {from: buyer});
            }
            // allow crowdsale contract to manage user usdt - payment transaction purpose
            await usdtToken.approve(crowdsale.address, usdtAmount, {from: buyer});

            //act
            await crowdsale.exchangeTokens(paymentCurrency.address, usdtAmount.toString(), {from: buyer, gas: 350000});
            
            const contractUsdtBalance = await usdtToken.balanceOf(crowdsale.address);
            const expectedBuyerTokenBalance = asEther(2000);
            const buyerTokenBalance = await crowdsale.balanceOf(buyer);

            //assert
            assert.isTrue(buyerTokenBalance.eq(expectedBuyerTokenBalance));
            assert.isTrue(contractUsdtBalance.eq(usdtAmount));
            /*
            */
        });
        
    });
});