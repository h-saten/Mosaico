import { createAction, props } from '@ngrx/store';
import { Company, CompanyPermissions, CompanyWalletInfo } from 'mosaico-dao';
import { CompanyWalletBalance } from 'mosaico-wallet';
import { BlockchainNetworks } from './business.reducers';

// # Company DETAILS VIEW
export const setCurrentCompany = createAction('[Company preview] Set current Company preview data', props<Company>());
export const setCompanyWalletBalance = createAction('[Company preview] Set company wallet tokens balance', props<CompanyWalletBalance>());
export const setCompanyPermissions = createAction('[Company] Set company permissions', props<CompanyPermissions>());
export const clearCompany = createAction('[Company] Clean current company');
// # Company Wallet
export const setCompanyWallet = createAction('[Wallet] Set Company Wallet Addresses', props<CompanyWalletInfo>());
export const setWalletBlockchainNetwork = createAction('[Wallet] Set User Wallet Blockchain Network', props<{networks: BlockchainNetworks[]}>());
//#company subscription
export const setUserSubscribedCompany = createAction(
  '[Company preview] Set user isSubscribed', props<{isSubscribed: boolean}>()
  );
