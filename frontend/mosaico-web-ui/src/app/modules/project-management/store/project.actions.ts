import { Company } from 'mosaico-dao';
import {createAction, props} from '@ngrx/store';
import {
  Faq,
  Page,
  Project,
  ProjectArticle,
  ProjectDocument,
  ProjectPackage,
  ProjectPermissions,
  Stage
} from 'mosaico-project';
import {KangaPaymentCurrencyEstimate, PaymentMethodType, Token} from 'mosaico-wallet';
import { PaymentCurrency } from 'mosaico-project';

// # PROJECT DETAILS VIEW
export const setCurrentProject = createAction('[Project preview] Set current project preview data', props<Project>());
export const setCurrentProjectPreviewId = createAction('[Project preview] Set current project preview ID', props<{ id: string }>());
export const setCurrentProjectPermissions = createAction('[Project preview] Set current user project permissions', props<ProjectPermissions>());
export const setCurrentProjectDocuments = createAction('[Project preview] Set current user project documents', props<{ documents: ProjectDocument[] }>());
export const clearCurrentProjectPreview = createAction('[Project preview] Clear');
export const setCurrentProjectActiveStage = createAction('[Project preview] Set current project active stage', props<{ activeStage: Stage }>());
export const setProjectTitle = createAction('[Project preview] Set current project title', props<{title: string}>());
export const setProjectToken = createAction('Project preview] Set current project token', props<{token: Token}>());
export const setProjectStages = createAction('[Project preview] Set current project stages', props<{stages: Stage[]}>());
export const setProjectPage = createAction('[Project preview] Set current project page', props<{page: Page}>());
export const setPageShortDescription = createAction('[Project preview] Set current page short description', props<{shortDescription: string}>());
export const setCurrentProjectCompany = createAction('[Project preview] Set current company', props<{company: Company}>());
export const setCurrentProjectCaps = createAction('[Project preview] set current project caps', props<{hardCap: number, softCap: number}>());
export const setUserSubscribeProject = createAction('[Project preview] Set user isSubscribed', props<{isSubscribed: boolean}>());
export const setCurrentCompanyPaymentDetails = createAction('[Project preview] Set company payment details', props<{walletAddress: string, network: string}>());

export const setProjectFaqs = createAction('[Project preview] Set current project faqs', props<{ faqs: Faq[] }>());
export const setProjectPackages = createAction('[Project preview] Set current project packages', props<{ packages: ProjectPackage[] }>());

// # PROJECT TEMPLATE
export const setProjectEditTemplate = createAction('[Project template] Set Project Edit Template', props<{ templateKey: string; language: string, documentId: string }>());
export const clearProjectTemplate = createAction('[Project template] Clear');

// # PROJECT ARTICLE
export const setCurrentProjectArticle = createAction('[Project article] Set Current Project Article', props<{ article: ProjectArticle }>());
export const clearProjectArticle = createAction('[Project article] Clear');

// # PROJECT PURCHASE
export const setKangaEstimates = createAction('[Project purchase] Set kanga estimates required for purchase calculation', props<{ kangaEstimates: KangaPaymentCurrencyEstimate[] }>());
export const setPaymentMethods = createAction('[Project preview] Set enabled payment methods', props<{ paymentMethods: PaymentMethodType[] }>());
export const setPaymentCurrencies = createAction('[Project preview] Set enabled payment currencies', props<{ paymentCurrencies: string[] }>());
export const setPaymentCurrenciesDetails = createAction('[Project preview] Set enabled payment currencies', props<{ paymentCurrencies: PaymentCurrency[] }>());
