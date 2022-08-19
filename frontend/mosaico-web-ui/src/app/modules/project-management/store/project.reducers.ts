import {createReducer, on} from "@ngrx/store";
import {
  clearCurrentProjectPreview,
  clearProjectTemplate,
  setCurrentProject,
  setCurrentProjectActiveStage,
  setCurrentProjectCaps,
  setCurrentProjectCompany,
  setCurrentProjectDocuments,
  setCurrentProjectPermissions,
  setCurrentProjectPreviewId, setKangaEstimates,
  setPageShortDescription, setPaymentCurrencies, setPaymentMethods,
  setProjectEditTemplate,
  setProjectFaqs,
  setProjectPackages,
  setUserSubscribeProject,
  setProjectPage,
  setProjectStages,
  setProjectTitle,
  setProjectToken,
  setCurrentProjectArticle,
  clearProjectArticle, setPaymentCurrenciesDetails, setCurrentCompanyPaymentDetails
} from "./project.actions";
import {KangaPaymentCurrencyEstimate, PaymentMethodType, Token} from 'mosaico-wallet';
import {
  CreateUpdateFaqCommand,
  Faq,
  Page, PaymentCurrency,
  Project,
  ProjectArticle,
  ProjectDocument,
  ProjectDocumentTemplate,
  ProjectPackage,
  ProjectPermissions,
  Stage
} from "mosaico-project";
import {Company} from "mosaico-dao";

export interface ProjectState {
  projectPreview: ProjectPreviewState;
  projectEdit: ProjectEditState;
  projectTemplate: ProjectTemplateState;
  projectArticle:  ProjectArticleState;
  projectPurchase: ProjectPurchaseState;
}
export interface ProjectEditState {
  projectId: string;
  project: Project;
  stages: Stage[] | null;
  documents: ProjectDocument[] | null;
  template: ProjectDocumentTemplate | null;
}
export interface ProjectPreviewState {
  projectId: string;
  project: Project;
  documents: ProjectDocument[];
  template: ProjectDocumentTemplate;
  company: Company;
  permissions: ProjectPermissions;
  activeStage: Stage;
  token: Token;
  faqs: Faq[];
  packages: ProjectPackage[];
  paymentCurrencies: PaymentCurrency[];
  page: Page;
  isSubscribed: boolean;
  companyWalletAddress?: string;
  companyWalletNetwork?: string;
}

export interface ProjectPurchaseState {
  kangaEstimates: KangaPaymentCurrencyEstimate[] | null;
  paymentMethods: PaymentMethodType[];
}

export interface ProjectTemplateState {
  language: string;
  templateKey: string;
  documentId: string;
}

export interface ProjectArticleState {
  article: ProjectArticle;
}

export const projectInitialState: ProjectState = {
  projectPreview: {} as ProjectPreviewState,
  projectEdit: {} as ProjectEditState,
  projectTemplate: {} as ProjectTemplateState,
  projectArticle: {} as ProjectArticleState,
  projectPurchase: {} as ProjectPurchaseState
};

// **FAQ**
export interface FaqState {
  faqDetails: FaqDetails;
  faqEdit: FaqEditState;
}

export interface FaqCreation {
  currentFaqId: string | null;
  details: CreateUpdateFaqCommand | null;
}

export interface FaqDetails {
  pageId: string | null;
}

export interface FaqEditState {
  Id: string;
  faqId: string;
  faq: Faq;
  title: string | null;
  content: string | null;
  isHidden: boolean | null;
  order: number | null;
}

export const faqInitialState: FaqState = {
  faqDetails: {} as FaqDetails,
  faqEdit: {} as FaqEditState
};

export const projectReducers = createReducer(
  projectInitialState,
  // PREVIEW
  on(setCurrentProjectPreviewId, (state, { id }) => {
    return { ...state, projectPreview: { ...state.projectPreview, projectId: id } };
  }),
  on(setCurrentProject, (state, project) => {
    return { ...state, projectPreview: { ...state.projectPreview, project } };
  }),
  on(setCurrentProjectPermissions, (state, permissions) => {
    return { ...state, projectPreview: { ...state?.projectPreview, permissions } };
  }),
  on(setCurrentProjectDocuments, (state, { documents }) => {
    return { ...state, projectPreview: { ...state.projectPreview, documents } };
  }),
  on(clearCurrentProjectPreview, (state) => {
    return { ...state, projectPreview: {permissions: state?.projectPreview?.permissions} as ProjectPreviewState };
  }),
  on(setCurrentProjectActiveStage, (state, { activeStage }) => {
    return { ...state, projectPreview:  { ...state.projectPreview, activeStage } };
  }),
  on(setProjectTitle, (state, {title}) => {
    return { ...state, projectPreview:  { ...state.projectPreview, project: {...state.projectPreview?.project, title}}};
  }),
  on(setProjectToken, (state, {token}) => {
    return {...state, projectPreview: { ...state.projectPreview, token}};
  }),
  on(setProjectStages, (state, { stages }) => {
    return { ...state, projectPreview: { ...state.projectPreview, project: { ...state.projectPreview?.project, stages } } };
  }),
  on(setCurrentProjectCaps, (state, {hardCap, softCap}) => {
    return { ...state, projectPreview: { ...state.projectPreview, project: { ...state.projectPreview?.project, hardCap, softCap } } };
  }),
  on(setProjectPage, (state, {page}) => {
    return {...state, projectPreview: {...state.projectPreview, page}};
  }),
  on(setPageShortDescription, (state, {shortDescription}) => {
    return { ...state, projectPreview:  { ...state.projectPreview, page: {...state.projectPreview?.page, shortDescription}}};
  }),
  on(setCurrentProjectCompany, (state, { company }) => {
    return { ...state, projectPreview:  { ...state.projectPreview, company }};
  }),
  on(setProjectFaqs, (state, { faqs }) => {
    // return { ...state, projectPreview: { ...state.projectPreview, project: { ...state.projectPreview?.project, faqs } } };
    return { ...state, projectPreview: { ...state.projectPreview, faqs } };
  }),

  on(setProjectPackages, (state, { packages }) => {
    return { ...state, projectPreview: { ...state.projectPreview, packages } };
  }),

  on(setUserSubscribeProject, (state, { isSubscribed }) => {
    return { ...state, projectPreview: { ...state.projectPreview, isSubscribed } };
  }),

  // TEMPLATE
  on(setProjectEditTemplate, (state, { templateKey, language, documentId }) => {
    return { ...state, projectTemplate: { ...state.projectTemplate, templateKey, language, documentId } };
  }),
  on(clearProjectTemplate, (state) => {
    return { ...state, projectTemplate: null };
  }),

  // ARTICLE
  on(setCurrentProjectArticle, (state, { article }) => {
    return { ...state, projectArticle: { ...state.projectArticle, article } };
  }),
  on(clearProjectArticle, (state) => {
    return { ...state, projectArticle: null };
  }),
  // PURCHASE
  on(setKangaEstimates, (state, {kangaEstimates}) => {
    return { ...state, projectPurchase: {...state.projectPurchase, kangaEstimates} };
  }),
  on(setPaymentMethods, (state, {paymentMethods}) => {
    return { ...state, projectPurchase: {...state.projectPurchase, paymentMethods} };
  }),
  on(setPaymentCurrenciesDetails, (state, {paymentCurrencies}) => {
    return { ...state, projectPreview: { ...state.projectPreview, paymentCurrencies } };
  }),
  on(setPaymentCurrencies, (state, {paymentCurrencies}) => {
    return { ...state, projectPreview: { ...state.projectPreview, project: {...state.projectPreview?.project, paymentCurrencies}}};
  }),
  on(setCurrentCompanyPaymentDetails, (state, {walletAddress, network}) => {
    return { ...state, projectPreview: { ...state.projectPreview, companyWalletAddress: walletAddress, companyWalletNetwork: network}};
  })
);
