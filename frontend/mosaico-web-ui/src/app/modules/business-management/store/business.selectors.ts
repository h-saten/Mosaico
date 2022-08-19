import { createFeatureSelector, createSelector } from "@ngrx/store";
import { CompanyState } from ".";

export const featureKey = 'company';

export const selectCompany = createFeatureSelector<CompanyState>(featureKey);

// COMPANYY PREVIEW
export const selectCompanyPreview = createSelector(
  selectCompany,
  (state: CompanyState) => state?.CompanyPreview
);

export const selectCompanyPermissions = createSelector(
    selectCompany,
    (state: CompanyState) => state?.Permissions
);

// COMPANY WALLET

export const selectCompanyWallet = createSelector(
    selectCompany,
    (state: CompanyState) => state?.CompanyWallet
);

export const selectCompanyWalletBalance = createSelector(
  selectCompany,
  (state: CompanyState) => state?.CompanyWalletBalance
);

export const selectWalletNetworks = createSelector(
  selectCompany,
  (state: CompanyState) => state?.Networks
);

// NEWSLETTER SUBSCRIPTION
export const selectUserSubscribedCompany = createSelector(
  selectCompany,
  (state: CompanyState) => state?.isSubscribed
);
