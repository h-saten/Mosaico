import { BlockchainNetworkType } from 'mosaico-base';
import { createReducer, on } from '@ngrx/store';
import { setActiveBlockchains, setCurrentBlockchain, setCurrentUrl, setShowFullWidth } from './actions';
import { Blockchain } from 'mosaico-wallet';

export interface GlobalState {
    selectedBlockchain: BlockchainNetworkType;
    activeBlockchains: Blockchain[];
    selectedCurrentUrl: string;
    showFullWidth: boolean;
}

export const initialState: GlobalState = {
    selectedBlockchain: "Ganache",
    activeBlockchains: [],
    selectedCurrentUrl: '',
    showFullWidth: false
};

export const globalReducers = createReducer(
    initialState,
    on(setCurrentBlockchain, (state, {blockchain}) => {
      return { ...state, selectedBlockchain: blockchain };
    }),
    on(setActiveBlockchains, (state, {networks}) => {
        return { ...state, activeBlockchains: networks };
    }),
    on(setCurrentUrl, (state, {currentUrl}) => {
      return { ...state, selectedCurrentUrl: currentUrl };
    }),
    on(setShowFullWidth, (state, {showFullWidth}) => {
      return { ...state, showFullWidth };
    })
);
