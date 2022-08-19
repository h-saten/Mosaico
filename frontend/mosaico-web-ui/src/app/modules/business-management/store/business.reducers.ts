import { createReducer, on } from "@ngrx/store";
import { setCurrentCompany, clearCompany, setCompanyWallet, setWalletBlockchainNetwork, setCompanyWalletBalance } from './business.actions';
import { setCompanyPermissions, setUserSubscribedCompany } from ".";
import { BlockchainNetworkType } from 'mosaico-base';
import { Company, CompanyPermissions, CompanyWalletInfo } from "mosaico-dao";
import { CompanyWalletBalance } from 'mosaico-wallet';

export interface CompanyState {
    CompanyPreview: Company;
    CompanyWalletBalance: CompanyWalletBalance;
    Permissions: CompanyPermissions;
    CompanyWallet: CompanyWalletInfo;
    Networks: BlockchainNetworks[];
    isSubscribed: boolean;
}

export interface BlockchainNetworks {
    network: BlockchainNetworkType,
    walletAddress: string
}


export const CompanyInitialState: CompanyState = {
  CompanyPreview: null,
  CompanyWalletBalance: null,
  Permissions: null,
  CompanyWallet: null,
  Networks: [],
  isSubscribed: false
};

export const CompanyReducers = createReducer(
    CompanyInitialState,
    // PREVIEW
    on(setCurrentCompany, (state, company) => {
        return { ...state, CompanyPreview: company };
    }),
    on(setCompanyPermissions, (state, permissions) => {
        return { ...state, Permissions: permissions };
    }),
    on(clearCompany, (state) => {
        return { ...state, CompanyPreview: null, CompanyWallet: null, Networks: [], Permissions: null };
    }),
    on(setCompanyWallet, (state, wallet) => {
        return { ...state, CompanyWallet: wallet };
    }),
    on(setCompanyWalletBalance, (state, walletBalance) => {
        return {...state, CompanyWalletBalance: walletBalance };
    }),
    on(setWalletBlockchainNetwork, (state, { networks }) => {
        return {...state, networks };
    }),
    on(setUserSubscribedCompany, (state, { isSubscribed }) => {
      return { ...state, isSubscribed };
    }),
);
