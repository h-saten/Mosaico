import {createAction, props} from '@ngrx/store';
import {WalletInfo, WalletBalance, Token, TokenPermissions, KangaWalletBalance} from 'mosaico-wallet';
import { BlockchainNetworks } from '../../business-management/store';

export const setWalletInfo = createAction('[Wallet] Set User Wallet Information', props<WalletInfo>());
export const setWalletBalance = createAction('[Wallet] Set user wallet tokens balance', props<WalletBalance>());
export const setMetamaskChain = createAction('[Wallet] Set Metamask chain', props<{chainId: string}>());
export const clearWalletState = createAction('[Company] Clean user wallet state');
export const setToken = createAction('[Token] Set current token', props<{token: Token}>());
export const clearToken = createAction('[Token] Clear token');
export const setTokenPermissions = createAction('[Token] Set permissions', props<TokenPermissions>());
export const setWalletBlockchainNetwork = createAction('[Wallet] Set User Wallet Blockchain Network', props<{networks: BlockchainNetworks[]}>());
export const setMetamaskConnected = createAction('[Wallet] Set Metamask Connected', props<{ isConnected: boolean }>());
export const setMetamaskAddress = createAction('[Wallet] Set User Metamask Addresses', props<{ address: string }>());
export const setKangaWalletBalance = createAction('[Wallet] Set Kanga wallet balance', props<{  kangaAssets: KangaWalletBalance[] }>());
export const setKangaTotalAssetValue = createAction('[Wallet] Set Kanga Total Wallet Value', props<{totalAssetValue: number}>());

export const setWalletConnected = createAction('[Wallet] Set User Connected WalletConnect', props<{isConnected: boolean}>());
export const setWalletConnectAddress = createAction('[Wallet] Set User Connect Wallet', props<{address: string}>());