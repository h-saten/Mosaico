import {createReducer, on} from "@ngrx/store";
import {WalletInfo, WalletBalance, Token, TokenPermissions, KangaWalletBalance} from "mosaico-wallet";
import {
  setWalletBalance,
  setWalletInfo,
  clearWalletState,
  setToken,
  clearToken,
  setWalletBlockchainNetwork,
  setTokenPermissions,
  setMetamaskChain,
  setMetamaskConnected,
  setMetamaskAddress,
  setKangaWalletBalance,
  setKangaTotalAssetValue
} from "./wallet.actions";
import { BlockchainNetworkType } from 'mosaico-base';
import { setWalletConnected, setWalletConnectAddress } from './wallet.actions';

export interface WalletState {
  walletInfo: WalletInfo;
  walletBalance: WalletBalance;
  token: Token;
  networks: BlockchainNetworks[];
  tokenPermissions: TokenPermissions;
  selectedChainId: string;
  metamaskAddress: string;
  isMetamaskConnected: boolean;
  kangaAssets: KangaWalletBalance[];
  kangaTotalAssetValue: number;
  isWalletConnected: boolean;
  walletConnectAddress: string;
}

export interface BlockchainNetworks {
  network: BlockchainNetworkType;
  walletAddress: string;
}

export const initialBalance: WalletState = {
  walletInfo: null,
  walletBalance: null,
  token: null,
  networks: [],
  tokenPermissions: null,
  selectedChainId: null,
  metamaskAddress: null,
  isMetamaskConnected: false,
  kangaAssets: [],
  kangaTotalAssetValue: 0,
  isWalletConnected: false,
  walletConnectAddress: null
};

export const walletReducers = createReducer(
  initialBalance,

  on(setWalletInfo, (state, walletInfo) => {
    return {...state, walletInfo};
  }),

  on(setWalletBalance, (state, walletBalance) => {
    return {...state, walletBalance };
  }),

  on(clearWalletState, state => {
    return {...state, walletInfo: null, walletBalance: null, networks: [] };
  }),

  on(setWalletBlockchainNetwork, (state, { networks }) => {
    return {...state, networks };
  }),

  on(setToken, (state, { token }) => {
    return {...state, token };
  }),

  on(setTokenPermissions, (state, perms) => {
    return {...state, tokenPermissions: perms };
  }),

  on(clearToken, (state) => {
    return {...state, token: null, tokenPermissions: null };
  }),

  on(setMetamaskChain, (state, { chainId }) => {
    return {...state, selectedChainId: chainId };
  }),
  on(setMetamaskConnected, (state, { isConnected }) => {
    return { ...state, isMetamaskConnected: isConnected };
  }),
  on(setMetamaskAddress, (state, { address }) => {
    return { ...state, metamaskAddress: address };
  }),
  on(setKangaWalletBalance, (state, { kangaAssets }) => {
    return { ...state, kangaAssets: kangaAssets };
  }),
  on(setKangaTotalAssetValue, (state, {totalAssetValue}) => {
    return { ...state, kangaTotalAssetValue: totalAssetValue};
  }),
  on(setWalletConnected, (state, {isConnected}) => {
    return { ...state, isWalletConnected: isConnected};
  }),
  on(setWalletConnectAddress, (state, {address}) => {
    return { ...state, walletConnectAddress: address };
  })
);

