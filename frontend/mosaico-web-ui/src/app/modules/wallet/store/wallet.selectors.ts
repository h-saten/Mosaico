import { createFeatureSelector, createSelector } from "@ngrx/store";
import { WalletState } from "./wallet.reducer";

export const featureKey = 'wallet';
export const selectWallet = createFeatureSelector<WalletState>(featureKey);

export const selectUserWallet = createSelector(
  selectWallet,
  (state: WalletState) => state?.walletInfo
);

export const selectWalletTokenBalance = createSelector(
  selectWallet,
  (state: WalletState) => state?.walletBalance
);

export const selectToken = createSelector(
  selectWallet,
  (state: WalletState) => state?.token
);

export const selectWalletNetworks = createSelector(
  selectWallet,
  (state: WalletState) => state?.networks
);

export const selectTokenPermissions = createSelector(
  selectWallet,
  (state: WalletState) => state?.tokenPermissions
);

export const selectMetamaskChainId = createSelector(
  selectWallet,
  (state: WalletState) => state?.selectedChainId
);

export const selectMetamaskConnected = createSelector(
  selectWallet,
  (state: WalletState) => state?.isMetamaskConnected
);

export const selectMetamaskAddress = createSelector(
  selectWallet,
  (state: WalletState) => state?.metamaskAddress
);

export const selectKangaAssets = createSelector(
  selectWallet,
  (state: WalletState) => state?.kangaAssets
);

export const selectTotalKangaAssetValue = createSelector(
  selectWallet,
  (state: WalletState) => state?.kangaTotalAssetValue
);

export const selectWalletConnected = createSelector(
    selectWallet,
    (state: WalletState) => state?.isWalletConnected
);

export const selectWalletConnectAddress = createSelector(
  selectWallet,
  (state: WalletState) => state?.walletConnectAddress
);