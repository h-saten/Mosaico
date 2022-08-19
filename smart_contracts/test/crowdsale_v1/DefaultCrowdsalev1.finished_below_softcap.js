const { assert } = require('chai');
const { type } = require('chai/lib/chai/utils');

const MosaicoERC20v1 = artifacts.require("MosaicoERC20v1");
const Crowdsale = artifacts.require("DefaultCrowdsalev1");
const TetherToken = artifacts.require("TetherToken");

var utils = web3.utils;

require('truffle-test-utils').init();

const transactionTool = require("./../helpers/ethTransactionHelper");
asBN = (value) => utils.toBN(value);
asEther = (value, decimals = 18) => asBN(value).mul(asBN(10).pow(asBN(decimals)));
asEtherString = (value, decimals = 18) => asEther(value, decimals).toString();

require('chai').should();

const tetherApprove = async (token, owner, recipient, amount)  => {
    const currentAllowance = await token.allowance(owner, recipient);
    if (currentAllowance.cmp(asBN(0)) === 1) {
        await token.approve(recipient, 0, {from: owner});
    }
    // allow crowdsale contract to manage user usdt - payment transaction purpose
    await token.approve(recipient, amount, {from: owner});
};

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
        isGovernance: false
    };

    const crowdsaleStages = [
        {
            name: 'Private Sale',
            isPrivate: false,
            cap: asEtherString(1000),
            minIndividualCap: asEtherString(10),
            maxIndividualCap: asEtherString(500),
            whitelist: [],
            rate: asEtherString(5, 14),
            stableCoinRate: asEtherString(2, 6)
        }
    ];

    const crowdsaleStagesStableCoinRates = [asEther(2)];
    const numberOfStages = crowdsaleStages.length;
    let token;
    let usdtToken;
    let usdcToken;
    let crowdsale;
    let currentStage = crowdsaleStages[0];

    beforeEach(async function () {
        token = await MosaicoERC20v1.new(tokenSettings);
        usdtToken = await TetherToken.new(usdtTokenSettings.initialSupply, usdtTokenSettings.name, usdtTokenSettings.symbol, 6, {from: usdtIssuer});
        usdcToken = await MosaicoERC20v1.new(usdcTokenSettings);
        crowdsale = await Crowdsale.new(companyWallet, token.address, numberOfStages, 10, [usdtToken.address, usdcToken.address]);
        
        await crowdsale.startNextStage(currentStage, crowdsaleStagesStableCoinRates[0]);
        await token.increaseAllowance(crowdsale.address, currentStage.cap, {from: companyWallet});
    });

    describe('Finished crowdsale without softcap reached', function() {
    
        it('should return invested stablecoins to user when sale ended below softcap', async () => {
            //arrange
            const paymentToken = usdtToken;
            const stagesIndex = 0;
            const buyer = accounts[3];
            const stage = crowdsaleStages[stagesIndex];

            await token.increaseAllowance(crowdsale.address, stage.cap, {from: companyWallet});
            const paymentTokenAmount = asEther(40, 6);
            // top up buyer wallet
            await paymentToken.transfer(buyer, paymentTokenAmount, {from: usdtIssuer});
            // allow crowdsale contract to manage user usdt - payment transaction purpose

            await tetherApprove(usdtToken, buyer, crowdsale.address, paymentTokenAmount);

            const initialBuyerBalance = await paymentToken.balanceOf(buyer);
            assert.isTrue(initialBuyerBalance.eq(asBN(paymentTokenAmount)));

            //act
            await crowdsale.exchangeTokens(paymentToken.address, paymentTokenAmount, {from: buyer});

            let buyerPaymentTokenBalance = await paymentToken.balanceOf(buyer);
            const contractUsdtBalance = await paymentToken.balanceOf(crowdsale.address);
            const buyerTokenBalance = await crowdsale.balanceOf(buyer);

            //assert
            assert.isTrue(buyerPaymentTokenBalance.eq(asBN(0)));
            assert.isTrue(buyerTokenBalance.eq(asEther(20)));
            assert.isTrue(contractUsdtBalance.eq(paymentTokenAmount));

            //finish crowdsale and return funds
            
            const crowdSaleOwner = accounts[0];
            await crowdsale.closeSale({from: crowdSaleOwner});
            
            assert.isTrue(await crowdsale._saleEnded());

            await crowdsale.refund({from: buyer});

            // //wait crowdale
            buyerPaymentTokenBalance = await paymentToken.balanceOf(buyer);

            assert.isTrue(buyerPaymentTokenBalance.eq(paymentTokenAmount));
        });
        
        it('should refund invested native currency', async () => {
            //arrange
            const buyer = accounts[3];
            const paymentAmount = asEther(1, 16);

            const initialEthBalance = await web3.eth.getBalance(buyer);  
            const buyTxn = await crowdsale.buyTokens(buyer, {value: paymentAmount, from: buyer, gas: 350000});
            const txnFee = await transactionTool.transactionCost(buyTxn);

            let crowdSaleEthBalance = await web3.eth.getBalance(crowdsale.address);  

            assert.isTrue(asBN(crowdSaleEthBalance).eq(paymentAmount));
            
            const crowdSaleOwner = accounts[0];
            await crowdsale.closeSale({from: crowdSaleOwner});
            
            assert.isTrue(await crowdsale._saleEnded());
            
            // act 
            const refundTx = await crowdsale.refund({from: buyer});
        
            const refundTxnFee = await transactionTool.transactionCost(refundTx);
            buyerEthBalance = await web3.eth.getBalance(buyer);
            const expectedEthBalance = asBN(initialEthBalance).sub(txnFee).sub(refundTxnFee);

            // assert
            assert.isTrue(asBN(buyerEthBalance).eq(expectedEthBalance));
        });

        it('should abort funds refund when softcap reached', async () => {
            //arrange
            const buyer = accounts[3];

            try {
                await crowdsale.closeSale({from: buyer});
                throw "success";
            }
            catch (error) {
                error.reason.should.equal("StageAdminRole: caller does not have the StageAdminRole role");
            }
        });        

        it('should abort funds refund when softcap reached', async () => {
            //arrange
            const paymentToken = usdtToken;
            const stagesIndex = 0;
            const buyer = accounts[3];
            const buyer2 = accounts[4];
            const stage = crowdsaleStages[stagesIndex];
            await token.increaseAllowance(crowdsale.address, stage.cap, {from: companyWallet});
            
            const paymentTokenAmount = asEther(1000, 6);
            // top up buyer wallet
            await paymentToken.transfer(buyer, paymentTokenAmount, {from: usdtIssuer});
            await paymentToken.transfer(buyer2, paymentTokenAmount, {from: usdtIssuer});

            // allow crowdsale contract to manage user usdt - payment transaction purpose
            await tetherApprove(usdtToken, buyer, crowdsale.address, paymentTokenAmount);
            await tetherApprove(usdtToken, buyer2, crowdsale.address, paymentTokenAmount);

            //act
            await crowdsale.exchangeTokens(paymentToken.address, paymentTokenAmount, {from: buyer});
            await crowdsale.exchangeTokens(paymentToken.address, paymentTokenAmount, {from: buyer2});
            
            const crowdSaleOwner = accounts[0];
            await crowdsale.closeSale({from: crowdSaleOwner});
            
            assert.isTrue(await crowdsale._saleEnded());

            try {
                await crowdsale.refund({from: buyer});
                await crowdsale.refund({from: buyer2});
            } catch (error) {
                error.reason.should.equal("Crowdsale: SoftCap reached");
            }
        });

        const closeSaleAfterInvestment = async (buyer, paymentToken, nativeCurrencyPaymentAmount, paymentTokenAmount) => {
            const stage = crowdsaleStages[0];
            await token.increaseAllowance(crowdsale.address, stage.cap, {from: companyWallet});

            if (nativeCurrencyPaymentAmount && nativeCurrencyPaymentAmount > 0) {
                // BUY USING ETH
                await crowdsale.buyTokens(buyer, {value: nativeCurrencyPaymentAmount, from: buyer, gas: 350000 });
            }

            if (paymentTokenAmount && paymentTokenAmount > 0) {
                // BUY USING STABLECOIN
                await paymentToken.transfer(buyer, paymentTokenAmount, {from: usdtIssuer});
                await tetherApprove(paymentToken, buyer, crowdsale.address, paymentTokenAmount);
                await crowdsale.exchangeTokens(paymentToken.address, paymentTokenAmount, {from: buyer, gas: 350000});
            }
            
            const crowdSaleOwner = accounts[0];
            await crowdsale.closeSale({from: crowdSaleOwner});
        }

        it('should transfer crowdsale funds to specified wallet after successfully ended crowdsale', async () => {
            //arrange
            const buyer = accounts[3];
            const paymentAmount = asEther(1, 16);
            const paymentTokenAmount = asEther(400, 6);
            const paymentToken = usdtToken;

            await closeSaleAfterInvestment(buyer, paymentToken, paymentAmount, paymentTokenAmount);

            const companyWalletBalanceBeforeWithdrawal = asBN(await web3.eth.getBalance(companyWallet));

            // act
            await crowdsale.withdrawFunds();

            const currentCompanyWalletBalance = asBN(await web3.eth.getBalance(companyWallet));
            const expectedCompanyWalletBalance = companyWalletBalanceBeforeWithdrawal.add(asBN(paymentAmount));
            const expectedCompanyWallettStablecoinBalance = asBN(await paymentToken.balanceOf(companyWallet));

            // assert
            assert.isTrue(asBN(currentCompanyWalletBalance).eq(expectedCompanyWalletBalance));
            assert.isTrue(expectedCompanyWallettStablecoinBalance.eq(asBN(paymentTokenAmount)));

        });

        it('should transfer bought tokens to users wallet', async () => {
            // arrange
            const buyer = accounts[3];
            const stage = crowdsaleStages[0];
            const paymentAmount = asEther(1, 16);
            const paymentToken = usdtToken;
            const paymentTokenAmount = asEther(400, 6);

            await closeSaleAfterInvestment(buyer, paymentToken, paymentAmount, paymentTokenAmount);
            
            const expectedUserTokensBalance = asEther(220);//paymentTokenAmount.div(asBN(stage.stableCoinRate)).add(paymentAmount.div(asBN(stage.rate)));

            // act
            await crowdsale.tokensDistribution();

            var buyerTokensBalance = await token.balanceOf(buyer);

            // assert
            assert.isTrue(buyerTokensBalance.eq(asBN(expectedUserTokensBalance)));
        });
 
   
      
        it('should transfer bought tokens when claimed', async () => {
            // arrange
            const buyer = accounts[3];
            const stage = crowdsaleStages[0];
            const paymentAmount = asEther(1, 16);
            const paymentToken = usdtToken;
            const paymentTokenAmount = asEther(400, 6);

            await closeSaleAfterInvestment(buyer, paymentToken, paymentAmount, paymentTokenAmount);
            
            const expectedUserTokensBalance = asEther(220);

            // act
            await crowdsale.claimTokens(buyer);

            var buyerTokensBalance = await token.balanceOf(buyer);

            // assert
            assert.isTrue(buyerTokensBalance.eq(asBN(expectedUserTokensBalance)));
        });
  
        it('should abort when tokens already claimed', async () => {
            // arrange
            const buyer = accounts[3];
            const paymentAmount = asEther(1, 16);
            const paymentToken = usdtToken;
            const paymentTokenAmount = asEther(400, 6);

            await closeSaleAfterInvestment(buyer, paymentToken, paymentAmount, paymentTokenAmount);
            await crowdsale.claimTokens(buyer);

            try {
                // act
                await crowdsale.claimTokens(buyer);
            } catch (error) {
                error.reason.should.equal("Crowdsale: Already claimed");
            }
        });

    });
});