import { Store } from '@ngrx/store';
import { APP_INITIALIZER } from "@angular/core";
import { ActiveBlockchainService } from 'mosaico-wallet';
import { setActiveBlockchains, setCurrentBlockchain } from '../store/actions';

function blockchainInitializer(store: Store, blockchainService: ActiveBlockchainService) {
    return () => {
        return new Promise((resolve, reject) => {
           blockchainService.getActiveBlockchains().subscribe((res) => {
            if(res && res.data) {
                store.dispatch(setActiveBlockchains({networks: res.data.networks}));
                const defaultNetwork = res.data.networks?.find((t) => t.isDefault === true) ?? res.data.networks[0];
                if(defaultNetwork) {
                    store.dispatch(setCurrentBlockchain({blockchain: defaultNetwork.name}));
                }
            }
            resolve(true);
           }, (error) => {reject(error)});
        });
    };
}

export const BlockchainInitializer = {
    provide: APP_INITIALIZER,
    useFactory: blockchainInitializer,
    multi: true,
    deps: [Store, ActiveBlockchainService]
}