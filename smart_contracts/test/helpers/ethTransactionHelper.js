
var utils = web3.utils;

transactionCost = async (transaction) => {
    const buyTxn = transaction;
    const buyTxnDetails = await web3.eth.getTransaction(buyTxn.tx);
    const gasPrice = utils.toBN(parseInt(buyTxnDetails.gasPrice));
    const txnFee = utils.toBN(buyTxn.receipt.gasUsed).mul(gasPrice);

    return Promise.resolve(txnFee);
}

module.exports = {
    transactionCost
}