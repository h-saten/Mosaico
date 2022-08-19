// Source: https://medium.com/edgefund/time-travelling-truffle-tests-f581c1964687

advanceTimeAndBlock = async (blocks) => {
    await advanceTime(blocks * 13.2);
    for(var i = 0; i < blocks; i++){
        await advanceBlock();
    }

    return Promise.resolve(web3.eth.getBlock('latest'));
}

advanceTime = (time) => {
    return new Promise((resolve, reject) => {
        web3.currentProvider.send({
            jsonrpc: "2.0",
            method: "evm_increaseTime",
            params: [time],
            id: new Date().getTime()
        }, (err, result) => {
            if (err) { return reject(err); }
            return resolve(result);
        });
    });
}

advanceBlock = () => {
    return new Promise((resolve, reject) => {
        web3.currentProvider.send({
            jsonrpc: "2.0",
            method: "evm_mine",
            id: new Date().getTime()
        }, (err, result) => {
            if (err) { return reject(err); }
            const newBlockHash = web3.eth.getBlock('latest').hash;

            return resolve(newBlockHash)
        });
    });
}

advanceInDays = async (daysAmount) => {
    const blocks = 86400 * parseInt(daysAmount);
    await advanceTime(blocks);
    await advanceBlock();
    return Promise.resolve(web3.eth.getBlock('latest'));
}

module.exports = {
    advanceTime,
    advanceBlock,
    advanceTimeAndBlock,
    advanceInDays
}