import { Store } from '@ngrx/store';
import { APP_INITIALIZER } from "@angular/core";
import { BlockchainService, ConfigService } from 'mosaico-base';
import Web3 from "web3";
import { setMetamaskAddress, setMetamaskConnected } from '../modules/wallet';
import { setMetamaskChain } from '../modules/wallet/store/wallet.actions';

function moralisInitializer(store: Store, configService: ConfigService, blockchainService: BlockchainService) {
    return () => {
        return new Promise((resolve) => {
            if (window.ethereum) {
                const web3 = new Web3(Web3.givenProvider);
                web3.eth.getAccounts().then((accounts) => {
                    store.dispatch(setMetamaskConnected({ isConnected: true }));
                    if (accounts && accounts.length > 0) {
                        store.dispatch(setMetamaskAddress({ address: accounts[0] }));
                    }
                    web3.eth.getChainId().then((chainId) => {
                        dispatchChainId(chainId, store);
                    });
                });
                Web3.givenProvider.on('accountsChanged', (accounts) => {
                    if(accounts && accounts.length > 0){
                        store.dispatch(setMetamaskAddress({ address: accounts[0] }));
                    }
                });
                Web3.givenProvider.on('disconnect', () => {
                    store.dispatch(setMetamaskConnected({ isConnected: false }));
                    store.dispatch(setMetamaskAddress({ address: null }));
                });
                Web3.givenProvider.on('chainChanged', (chainId) => {
                    dispatchChainId(chainId, store);
                });
            }
            resolve(true);
        });
    };
}

export function dispatchChainId(chainId: any, store: Store): void {
    const web3 = new Web3(Web3.givenProvider);
    chainId = web3.utils.hexToNumber(chainId)?.toString();
    store.dispatch(setMetamaskChain({ chainId }));
}

export const MoralisInitializer = {
    provide: APP_INITIALIZER,
    useFactory: moralisInitializer,
    multi: true,
    deps: [Store, ConfigService, BlockchainService]
}