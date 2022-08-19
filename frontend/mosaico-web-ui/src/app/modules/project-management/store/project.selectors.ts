import { createFeatureSelector, createSelector } from "@ngrx/store";
import { PaymentMethodType } from "mosaico-wallet";
import { ProjectState, ProjectEditState, ProjectPreviewState } from '.';

export const featureKey = 'project';
export const selectProject = createFeatureSelector<ProjectState>(featureKey);

// PROJECT PREVIEW

export const selectProjectPreview = createSelector(
  selectProject,
  (state: ProjectState) => state?.projectPreview
);

// not used
export const selectPreviewProjectId = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.projectId
);

export const selectPreviewProject = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.project
);

// not used
export const selectPreviewProjectDocuments = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.documents
);

export const selectPreviewProjectPermissions = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.permissions
);

export const selectPreviewProjectActiveStage = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.activeStage
);

export const selectProjectPreviewToken = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.token
);

export const selectProjectPage = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.page
);

export const selectUserSubscribedProject = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.isSubscribed
);

export const selectProjectFaqs = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.faqs
);

export const selectProjectPackages = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.packages
);

export const selectProjectCompany = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.company
);

// PROJECT UPDATE

export const selectProjectEdit = createSelector(
  selectProject,
  (state: ProjectState) => state?.projectEdit
);

export const selectProjectEditId = createSelector(
  selectProjectEdit,
  (state: ProjectEditState) => state?.projectId
);

// not used
export const selectProjectEditDetails = createSelector(
  selectProjectEdit,
  (state: ProjectEditState) => state?.project
);

// not used
export const selectProjectEditStages = createSelector(
  selectProjectEdit,
  (state: ProjectEditState) => state?.stages
);

// not used
export const selectProjectEditDocuments = createSelector(
  selectProjectEdit,
  (state: ProjectEditState) => state?.documents
);

// PROJECT TEMPLATE
export const selectProjectDocumentTemplate = createSelector(
  selectProject,
  (state: ProjectState) => state?.projectTemplate
);

// Project Article
export const selectProjectArticle = createSelector(
  selectProject,
  (state: ProjectState) => state?.projectArticle
);
// PROJECT PURCHASE
export const selectProjectKangaEstimates = createSelector(
  selectProject,
  (state: ProjectState) => state?.projectPurchase?.kangaEstimates
);

export const selectProjectPaymentMethods = createSelector(
  selectProject,
  (state: ProjectState) => state?.projectPurchase?.paymentMethods ?? new Array<PaymentMethodType>()
);

export const selectProjectPaymentCurrencies = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.project?.paymentCurrencies
);

export const selectProjectPaymentCurrenciesDetails = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => state?.paymentCurrencies
);

export const selectCompanyWallet = createSelector(
  selectProjectPreview,
  (state: ProjectPreviewState) => { return { walletAddress: state?.companyWalletAddress, network: state?.companyWalletNetwork } }
)