import { Injectable } from "@angular/core";
import { MosaicoRelayMilkyService, StakingPair, StakingService, WalletStake } from 'mosaico-wallet';
import { BlockchainService, SuccessResponse, ErrorHandlingService } from 'mosaico-base';
import { firstValueFrom } from "rxjs";
import { ToastrService } from "ngx-toastr";
import { IStakingProcessor } from "./istaking-processor";

@Injectable({
    providedIn: 'root'
})
export class MilkyMetamaskStakingProcessor implements IStakingProcessor {

    constructor(private _relayService: MosaicoRelayMilkyService, private _blockchainService: BlockchainService, private toastr: ToastrService, private errorHandler: ErrorHandlingService,
        private stakingService: StakingService) {
    }

    approve(stakingPair: StakingPair, balance: number = 0, days: number = 0): Promise<string|boolean> {
        return new Promise<string|boolean>(async (resolve, reject) => {
            const tokenAddress = '0x296480179b8b79c9C9588b275E69d58f0d1BCa67';
            const recipient = '0x60696c5423A6484bf9Ff1BA7e4f1aD0b037bab2a';
            try {
                const tokenAbiResponse: SuccessResponse<any> = await firstValueFrom(this._relayService.getMilkyTokenAbi());
                const tokenAbi = tokenAbiResponse?.data;
                const web3 = await this._blockchainService.getWeb3();
                await this._blockchainService.authenticateToMetamask();
                const owner = await this._blockchainService.getCurrentWallet();
                const value = web3.utils.toWei(balance.toString());
                const contract = new web3.eth.Contract(tokenAbi, tokenAddress);
                const currentBalance = await contract.methods.balanceOf(owner).call();
                if (web3.utils.toBN(value).gt(web3.utils.toBN(currentBalance))) {
                    throw new Error('Your request exceeds the balance');
                }
                const currentAllowance = await contract.methods.allowance(owner, recipient).call();
                if (web3.utils.toBN(value).gt(web3.utils.toBN(currentAllowance))) {
                    let requiredGas = await contract.methods.approve(recipient, value).estimateGas({ from: owner });
                    requiredGas = Math.ceil(requiredGas + requiredGas * 0.2);
                    let gasPrice = web3.utils.toBN(await web3.eth.getGasPrice());
                    gasPrice = gasPrice.add(gasPrice.mul(web3.utils.toBN(20)).div(web3.utils.toBN(100)));
                    await contract.methods.approve(recipient, value).send({ from: owner, gas: requiredGas, gasPrice })
                        .on('receipt', (p) => resolve(p.transactionHash))
                        .on('error', (error) => {
                            reject(error)
                        });
                }
                else{
                    resolve(true);
                }
            }
            catch(error) {
                reject(error);
            }
        });
    }

    public async stake(stakingPair: StakingPair, balance: number = 0, days: number = 0): Promise<string|boolean> {
        return new Promise<string|boolean>(async (resolve, reject) => {
            try {
                const contractAddress = '0x60696c5423A6484bf9Ff1BA7e4f1aD0b037bab2a';
                const dividendAbiResponse: SuccessResponse<any> = await firstValueFrom(this._relayService.getMilkyDividendAbi());
                const dividendAbi = dividendAbiResponse?.data;
                this.toastr.success('You successfully approved MIC tokens on Staking Smart contract. Now let\'s actually stake.');
                const web3 = await this._blockchainService.getWeb3();
                const owner = await this._blockchainService.getCurrentWallet();
                const value = web3.utils.toWei(balance.toString());
                const contract = new web3.eth.Contract(dividendAbi, contractAddress);
                let requiredGas = await contract.methods.stake(value).estimateGas({ from: owner });
                requiredGas = Math.ceil(requiredGas + requiredGas * 0.2);
                let gasPrice = web3.utils.toBN(await web3.eth.getGasPrice());
                gasPrice = gasPrice.add(gasPrice.mul(web3.utils.toBN(20)).div(web3.utils.toBN(100)));
                await contract.methods.stake(value).send({ from: owner, gas: requiredGas, gasPrice })
                    .on('transactionHash', async (hash) => {
                        await this.handleSuccessMetamaskStake(stakingPair, balance, days, hash);
                        resolve(hash);
                    })
                    .on('error', (error) => {
                        reject(error);
                    });
            }
            catch (e) {
                reject(e);
            }
        });
    }

    public async withdraw(stake: WalletStake): Promise<string|boolean> {
        return new Promise<string|boolean>(async (resolve, reject) => {
            try {
                const dividendContractAddress = '0x60696c5423A6484bf9Ff1BA7e4f1aD0b037bab2a';
                const dividendAbiResponse = await firstValueFrom(this._relayService.getMilkyDividendAbi());
                const dividendAbi = dividendAbiResponse?.data;
                const web3 = await this._blockchainService.getWeb3();
                await this._blockchainService.authenticateToMetamask();
                const owner = await this._blockchainService.getCurrentWallet();
                const contract = new web3.eth.Contract(dividendAbi, dividendContractAddress);
                let requiredGas = await contract.methods.withdraw().estimateGas({ from: owner });
                requiredGas = Math.ceil(requiredGas + requiredGas * 0.2);
                const gasPrice = await web3.eth.getGasPrice();
                await contract.methods.withdraw().send({ from: owner, gas: requiredGas, gasPrice })
                    .on('transactionHash', async (hash) => {
                        await this.handleSuccessMetamaskWithdraw(stake.id, owner, hash);
                        resolve(hash);
                    })
                    .on('error', (error) => {
                        reject(error);
                    });
              }
              catch (e) {
                reject(e);
              }
        });
    }

    private async handleSuccessMetamaskStake(stakingPair: StakingPair, balance: number, days: number, transactionHash: string): Promise<void> {
        const owner = await this._blockchainService.getCurrentWallet();
        const response = await firstValueFrom(this.stakingService.stakeMetamask(stakingPair.id, {
            balance,
            days,
            stakingPairId: stakingPair.id,
            transactionHash,
            wallet: owner
        }));
    }

    private async handleSuccessMetamaskWithdraw(id: string, wallet: string, transactionHash: string): Promise<void> {
        const owner = await this._blockchainService.getCurrentWallet();
        const response = await firstValueFrom(this.stakingService.withdrawMetamask(id, wallet, transactionHash));
    }
}