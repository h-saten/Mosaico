import { Injectable } from '@angular/core';
import WalletConnectProvider from "@walletconnect/web3-provider";
import Web3 from 'web3';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class WalletConnectService {
    provider: WalletConnectProvider;
    currentAddress$ = new BehaviorSubject<string>(null);
    onDisconnected$ = new BehaviorSubject<boolean>(false);

    constructor() {
        this.provider = new WalletConnectProvider({
            rpc: {
                137: "https://polygon-rpc.com"
            },
            chainId: 137
        });
    }

    public isConnected(): boolean {
        return this.provider?.connected;
    }

    public async getWeb3(): Promise<Web3> {
        if (this.provider) {
            await this.provider.enable();
            this.onDisconnected$.next(false);
            this.provider.on("accountsChanged", (accounts: string[]) => {
                if (accounts && accounts.length > 0) {
                    this.currentAddress$.next(accounts[0]);
                }
            });
            this.provider.on("disconnect", (code: number, reason: string) => {
                this.currentAddress$.next(null);
                this.onDisconnected$.next(true);
            });
            const web3 = new Web3(<any>this.provider);
            return web3;
        }
    }

    public async cleanup(): Promise<any> {
        await this.provider?.disconnect();
        this.currentAddress$.next(null);
    }


    public async getCurrentWallets(): Promise<string[]> {
        if (window.ethereum) {
            const web3 = await this.getWeb3();
            return await web3.eth.getAccounts();
        }
        return [];
    }

    public async getCurrentWallet(): Promise<string> {
        if (window.ethereum) {
            const web3 = await this.getWeb3();
            const accounts = await web3.eth.getAccounts();
            if (accounts && accounts.length > 0) {
                return accounts[0];
            }
        }
        return null;
    }
}