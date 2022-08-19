const { assert } = require('chai');
const { type } = require('chai/lib/chai/utils');

const MosaicoERC20v1 = artifacts.require("MosaicoERC20v1");
const Crowdsale = artifacts.require("DefaultCrowdsalev1");
const TetherToken = artifacts.require("TetherToken");

var utils = web3.utils;

require('truffle-test-utils').init();

const transactionTool = require("./../test/helpers/ethTransactionHelper");
asBN = (value) => utils.toBN(value);
asEther = (value, decimals = 18) => asBN(value).mul(asBN(10).pow(asBN(decimals)));
asEtherString = (value, decimals = 18) => asEther(value, decimals).toString();

require('chai').should();

contract('Crowdsale', accounts => {

    describe('debug contract', function() {
    
        it('debug', async () => {
            const crowdsaleAddress = '0xd40942ba74ca40bdea2bb523cd686f8cb0985efc';
            const crowdsale = await Crowdsale.at(crowdsaleAddress);
            const tetherAddress = '0xdcf769530b212312c6e665d389c6cddd62f3f80d';
            const tether = await TetherToken.at(tetherAddress);

            const hardCap = await crowdsale.hardCap();
            const _tokensSold = await crowdsale._tokensSold();
            const stableCoinRate = await crowdsale.stableCoinRate();
            //const currentStage = await crowdsale._currentStage();
            
            const cap = await crowdsale.cap();
            const minIndividualCap = await crowdsale.minIndividualCap();
            const maxIndividualCap = await crowdsale.maxIndividualCap();
            const rate = await crowdsale.getRate();

            console.log(">> hardCap ", hardCap.toString());
            console.log(">> _tokensSold ", _tokensSold.toString());
            console.log(">> stableCoinRate ", stableCoinRate.toString());
            console.log(">> cap ", cap.toString());
            console.log(">> minIndividualCap ", minIndividualCap.toString());
            console.log(">> maxIndividualCap ", maxIndividualCap.toString());
            console.log(">> rate ", rate.toString());
            var receive = await crowdsale.userReceive(asEther(48, 6), rate);
            console.log(">> should receive ", receive.toString());

            const userBalance = await crowdsale.balanceOf('0xD9b26063e884266FADE2E9449A3194c2e5EE299f');
            console.log(">> userBalance ", userBalance.toString());
            
            console.log("===================");
            const usdtDecimals = await crowdsale.safeDecimals(tetherAddress);
            console.log("usdt decimals", usdtDecimals.toString());
        });

    });
});