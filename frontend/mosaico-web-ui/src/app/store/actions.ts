import { createAction, props } from '@ngrx/store';
import { BlockchainNetworkType } from 'mosaico-base';
import { Blockchain } from 'mosaico-wallet';

export const setCurrentBlockchain = createAction('[Global] Set current blockchain', props<{blockchain: BlockchainNetworkType}>());
export const setActiveBlockchains = createAction('[Global] Set current active blockchains', props<{networks: Blockchain[]}>());
export const setCurrentUrl = createAction('[Global] Set current url', props<{currentUrl: string}>());
export const setShowFullWidth = createAction('[Global] Set show full width', props<{showFullWidth: boolean}>());

