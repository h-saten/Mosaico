import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthService, ConfigService, HubServiceBase } from 'mosaico-base';

export interface TokenCreated {
    tokenId: string;
    tokenAddress: string;
}

@Injectable({
    providedIn: 'root'
})
export class WalletHubService extends HubServiceBase {
    public deployed$ = new BehaviorSubject<TokenCreated>(null);
    public failed$ = new BehaviorSubject<string>(null);
    public tokenMinted$ = new BehaviorSubject<string>(null);
    public tokenMintingFailed$ = new BehaviorSubject<string>(null);
    public tokenBurned$ = new BehaviorSubject<string>(null);
    public tokenBurningFailed$ = new BehaviorSubject<string>(null);
    public sendTransactionSucceeded$ = new BehaviorSubject<string>(null);
    public sendTransactionFailed$ = new BehaviorSubject<string>(null);
    public airdropWithdrawn$ = new BehaviorSubject<string>(null);
    public airdropWithdrawalFailed$ = new BehaviorSubject<string>(null);
    public stakeSucceeded$ = new BehaviorSubject<string>(null);
    public stakeFailed$ = new BehaviorSubject<string>(null);
    public stakeWithdrawalSucceeded$ = new BehaviorSubject<string>(null);
    public stakeWithdrawalFailed$ = new BehaviorSubject<string>(null);
    public stakeRewardClaimFailed$ = new BehaviorSubject<string>(null);
    public stakeRewardClaimedSuccessfully$ = new BehaviorSubject<string>(null);
    public stakeDistributed$ = new BehaviorSubject<string>(null);
    public stakeDistributionFailed$ = new BehaviorSubject<string>(null);

    constructor(http: HttpClient, configService: ConfigService, authClient: AuthService) {
        super(http, configService, authClient);
    }

    public addMintingListeners(): void {
        this.hubConnection.on('tokenMinted', (data: string) => {
            this.tokenMinted$.next(data);
        });
        this.hubConnection.on('tokenMintingFailed', (data: string) => {
            this.tokenMintingFailed$.next(data);
        });
    }

    public addStakeListeners(): void {
        this.hubConnection.on('stakeSucceeded', (data: string) => {
            this.stakeSucceeded$.next(data);
        });
        this.hubConnection.on('stakeFailed', (data: string) => {
            this.stakeFailed$.next(data);
        });
    }

    public addBurningListeners(): void {
        this.hubConnection.on('tokenBurned', (data: string) => {
            this.tokenBurned$.next(data);
        });
        this.hubConnection.on('tokenBurningFailed', (data: string) => {
            this.tokenBurningFailed$.next(data);
        });
    }

    public addListener(): void {
        this.hubConnection.on('tokenCreated', (data: TokenCreated) => {
            this.deployed$.next(data);
        });
        this.hubConnection.on('tokenCreationFailed', (data: string) => {
            this.failed$.next(data);
        });
    }

    public addWalletListeners(): void {
        this.hubConnection.on('sentCurrencySucceeded', (data: string) => {
            this.sendTransactionSucceeded$.next(data);
        });
        this.hubConnection.on('sentCurrencyFailed', (data: string) => {
            this.sendTransactionFailed$.next(data);
        });
    }

    public addAirdropListeners(): void {
        this.hubConnection.on('airdropDispatched', (data: string) => {
            this.airdropWithdrawn$.next(data);
        });
        this.hubConnection.on('airdropDispatchFailed', (data: string) => {
            this.airdropWithdrawalFailed$.next(data);
        });
    }

    public addStakeWithdrawalListeners(): void {
        this.hubConnection.on('stakeWithdrawalSucceeded', (data: string) => {
            this.stakeWithdrawalSucceeded$.next(data);
        });
        this.hubConnection.on('stakeWithdrawalFailed', (data: string) => {
            this.stakeWithdrawalFailed$.next(data);
        });
    }

    public addStakeRewardListeners(): void {
        this.hubConnection.on('stakeRewardClaimedSuccessfully', (data: string) => {
            this.stakeRewardClaimedSuccessfully$.next(data);
        });
        this.hubConnection.on('stakeRewardClaimFailed', (data: string) => {
            this.stakeRewardClaimFailed$.next(data);
        });
    }

    public addStakeDistributeListeners(): void {
        this.hubConnection.on('stakeDistributed', (data: string) => {
            this.stakeDistributed$.next(data);
        });
        this.hubConnection.on('stakeDistributionFailed', (data: string) => {
            this.stakeDistributionFailed$.next(data);
        });
    }

    getHubName(): string {
        return 'wallet';
    }

    public resetObjects(): void {
        this.deployed$ = new BehaviorSubject<TokenCreated>(null);
        this.failed$ = new BehaviorSubject<string>(null);
        this.tokenMinted$ = new BehaviorSubject<string>(null);
        this.tokenMintingFailed$ = new BehaviorSubject<string>(null);
        this.tokenBurned$ = new BehaviorSubject<string>(null);
        this.tokenBurningFailed$ = new BehaviorSubject<string>(null);
        this.sendTransactionSucceeded$ = new BehaviorSubject<string>(null);
        this.sendTransactionFailed$ = new BehaviorSubject<string>(null);
        this.airdropWithdrawn$ = new BehaviorSubject<string>(null);
        this.airdropWithdrawalFailed$ = new BehaviorSubject<string>(null);
        this.stakeSucceeded$ = new BehaviorSubject<string>(null);
        this.stakeFailed$ = new BehaviorSubject<string>(null);
        this.stakeWithdrawalSucceeded$ = new BehaviorSubject<string>(null);
        this.stakeWithdrawalFailed$ = new BehaviorSubject<string>(null);
        this.stakeRewardClaimFailed$ = new BehaviorSubject<string>(null);
        this.stakeRewardClaimedSuccessfully$ = new BehaviorSubject<string>(null);
        this.stakeDistributed$ = new BehaviorSubject<string>(null);
        this.stakeDistributionFailed$ = new BehaviorSubject<string>(null);
    }
}