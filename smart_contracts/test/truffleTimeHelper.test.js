const { assert } = require('chai');
const blockchainTime = require("./helpers/truffleTimeHelper");

require('truffle-test-utils').init();

contract('TimeHelper', () => {

    describe("Testing Helper Functions", () => {
        it("should advance the blockchain forward a block", async () =>{
            const originalBlockHash = await web3.eth.getBlock('latest').then(result => result.hash);
            let newBlockHash = await web3.eth.getBlock('latest').then(result => result.hash);
            assert.equal(originalBlockHash, newBlockHash);

            newBlockHash = await blockchainTime.advanceBlock().then(result => result);
    
            assert.notEqual(originalBlockHash, newBlockHash);
        });
    
        it("should be able to advance time and block together", async () => {
            const advancement = 600;
            
            const originalBlock = await web3.eth.getBlock('latest').then(result => result);
            const newBlock = await blockchainTime.advanceTimeAndBlock(advancement).then(result => result);

            const timeDiff = newBlock.timestamp - originalBlock.timestamp;
    
            assert.isTrue(timeDiff >= advancement);
        });
    });

});