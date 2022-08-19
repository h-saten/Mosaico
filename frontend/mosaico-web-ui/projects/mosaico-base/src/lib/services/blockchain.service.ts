import { Injectable } from "@angular/core";
import detectEthereumProvider from '@metamask/detect-provider';
import { RequestAccounts, AddSelectedToken } from '../utils/moralis-custom-methods';
import { SelectedTokenNetworkType } from '../models/selected-token-network-type';
import { BlockchainProtocolType } from "../models/blockchain-network-protocol";
import Web3 from 'web3';
import * as etherscanLink from '@metamask/etherscan-link';

@Injectable({
    providedIn: 'root'
})
export class BlockchainService {
    constructor() {
    }

    public isValidAddress(address: string): boolean {
        return Web3.utils.isAddress(address);
    }

    public async cleanup(): Promise<any> {
        if (window.ethereum) {
            const ethProvider = await detectEthereumProvider() as any;
            await ethProvider.request({
                method: "eth_requestAccounts",
                params: [
                    {
                        eth_accounts: {}
                    }
                ]
            });
        }
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

    public async getChainId(): Promise<number> {
        const web3 = await this.getWeb3();
        return web3.eth.getChainId();
    }

    public async getWeb3(): Promise<Web3> {
        const ethProvider = await detectEthereumProvider() as any;
        return new Web3(ethProvider);
    }

    public async authenticateToMetamask(callback?: any): Promise<string> {
        const web3 = await this.getWeb3();
        const accounts = await web3.eth.requestAccounts();
        if (accounts && accounts.length > 0) {
            return accounts[0];
        }
    }

    public async addSelectedTokenToMetamask(selectedTokenNetworkType: SelectedTokenNetworkType): Promise<boolean> {
        const ethProvider = await detectEthereumProvider() as any;
        await ethProvider.request({
            method: AddSelectedToken,
            params: {
                type: <BlockchainProtocolType>selectedTokenNetworkType.protocol || BlockchainProtocolType.ERC20,
                options: {
                    address: selectedTokenNetworkType.address,
                    symbol: selectedTokenNetworkType.symbol,
                    decimals: selectedTokenNetworkType.decimals,
                    image: selectedTokenNetworkType.image
                }
            }
        });
        return true;
    }

    public getAccountLink(address: string, link: string): string {
        return etherscanLink.createCustomAccountLink(address, link);
    }

    public getTransactionLink(hash: string, link: string): string {
        return etherscanLink.createCustomExplorerLink(hash, link);
    }

    public getTokenLink(address: string, link: string): string {
        return etherscanLink.createCustomTokenTrackerLink(address, link);
    }
}