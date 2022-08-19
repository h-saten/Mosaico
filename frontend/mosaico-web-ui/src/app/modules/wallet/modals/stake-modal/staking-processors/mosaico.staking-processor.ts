import { Injectable } from "@angular/core";
import { IStakingProcessor } from "./istaking-processor";
import { StakingPair, StakingService, WalletStake } from 'mosaico-wallet';
import { ErrorHandlingService } from 'mosaico-base';

@Injectable({
    providedIn: 'root'
})
export class MosaicoStakingProcessor implements IStakingProcessor {
    constructor(private _stakingService: StakingService, private errorHandler: ErrorHandlingService) {

    }

    withdraw(stake: WalletStake): Promise<string | boolean> {
        return new Promise<boolean>((resolve, reject) => {
            this._stakingService.withdraw(stake.id).subscribe((res) => {
                resolve(true);
            }, (error) => {
                reject(error);
            });
        });
    }

    stake(stakingPair: StakingPair, balance: number = 0, days: number = 0): Promise<string | boolean> {
        return new Promise<boolean>((resolve, reject) => {
            this._stakingService.stake(stakingPair.id, { balance, days, stakingPairId: stakingPair.id }).subscribe((res) => {
                resolve(true);
            }, (error) => {
                reject(error);
            });
        });
    }

    async approve(stakingPair: StakingPair, balance: number = 0, days: number = 0): Promise<string | boolean> {
        return true;
    }
}