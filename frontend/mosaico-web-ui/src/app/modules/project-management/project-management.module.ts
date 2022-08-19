import {InlineSVGModule} from 'ngx-svg-inline';
import {CommonModule} from "@angular/common";
import {NgModule} from "@angular/core";
import {ProjectManagementRoutingModule} from './project-management-routing.module';
import {RoleManagerModule} from '../role-manager/role-manager.module';
import {DocumentTemplateComponent, NewProjectComponent, ProjectComponent, ProjectInvitationComponent} from './pages';
import {ImageModule} from 'primeng/image';

import {
  BlockchainAddressComponent,
  BlockchainLinksComponent,
  ButtonBuyTokensComponent,
  EditProjectMembersComponent,
  ExchangeListComponent,
  FaqRowComponent,
  MetaWalletFundingComponent,
  PaymentMethodListComponent,
  ProjectAddDocumentModalComponent,
  ProjectCompanyDetailsComponent,
  ProjectDocumentTemplatesListComponent,
  ProjectFaqComponent,
  ProjectFeaturesComponent,
  ProjectFooterComponent,
  ProjectMenuComponent,
  ProjectNewsComponent,
  ProjectNewsFormComponent,
  ProjectNewsCardComponent,
  ProjectOverviewComponent,
  ProjectPackagesComponent,
  ProjectPackagesRowComponent,
  ProjectStageProgressComponent,
  ProjectTeamsComponent,
  SalesInfoComponent,
  SelectDocumentTemplateComponent
} from './components';

import {
  ProjectSummaryComponent,
  MyDatePlPipe,
  ProjectIconsSmComponent,
  ProjectSalesProgressBarComponent,
  TokenEditorComponent,
  ProjectCounterComponent,
  StatusFollowShareComponent,
  ProjectInfoComponent,
  ProjectSubscriptionToNewsletterComponent,
  ProjectTitleComponent,
  ProjectTitleDirective
} from './components/project-summary';

import {CardsModule, DrawersModule, DropdownMenusModule, ExtrasModule, ModalsModule} from 'src/app/_metronic/partials';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {
  NgbAccordionModule,
  NgbActiveModal,
  NgbCollapseModule,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbModalModule,
  NgbNavModule,
  NgbPopoverModule,
  NgbTooltipModule
} from '@ng-bootstrap/ng-bootstrap';
import {SharedModule} from '../shared';
import {DocumentManagementModule} from '../document-management';
import {StoreModule} from '@ngrx/store';
import {featureKey, projectReducers} from './store';
import {MomentModule} from 'ngx-moment';
import {NgApexchartsModule} from 'ng-apexcharts';
import {ClipboardModule} from 'ngx-clipboard';
import {AccessBuyGuard} from './guards';
import {RouterModule} from '@angular/router';
import {CKEditorModule} from '@ckeditor/ckeditor5-angular';
import {ImageCropperModule} from 'ngx-image-cropper';
import {ProjectTeamsAddEditComponent} from './components/project-teams/project-team-add-edit/project-team-add-edit.component';
import {ProjectTeamManageModalComponent} from './components/project-teams/project-team-manage-modal/project-team-manage-modal.component';
import {locale as enLangEn} from './i18n/en';
import {locale as enLangPl} from './i18n/pl';
import {ProjectFaqFormComponent} from './components/project-faq/faq-form/faq-form.component';
import {ProjectTokenomicsComponent} from './components/project-tokenomics/project-tokenomics.component';
import {ProjectStagesComponent} from './components/project-stages/project-stages.component';
import {TranslationService} from 'mosaico-base';
import {MosaicoWalletModule} from 'mosaico-wallet';
import {CheckoutComponent} from './pages/checkout/checkout.component';
import {MosaicoWalletComponent} from './components/payment/mosaico-wallet/mosaico-wallet.component';
import {KangaExchangeComponent} from './components/payment/kanga-exchange/kanga-exchange.component';
import {MetamaskWalletComponent} from './components/payment/metamask-wallet/metamask-wallet.component';
import {CreditCardComponent} from './components/payment/credit-card/credit-card.component';
import {ProjectPackagesFormComponent} from './components/project-packages/project-packages-form/project-packages-form.component';
import {ImageCropperLocalModule} from '../shared-modules/image-cropper-local/image-cropper-local.module';
import {ProjectFaqModalComponent} from './components/project-faq/project-faq-modal/project-faq-modal.component';
import {
  ButtonSaveModule,
  ColorEditModule,
  InlineEditModule,
  ShowMoreRowModule,
  SpinnerMiniModule
} from 'src/app/modules/shared-modules';

import {NgSelectModule} from '@ng-select/ng-select';
import {
  ArticleCoverUploadComponent,
  ArticlePhotoUploadComponent,
  ActionSuccessComponent,
  EditMainColorsComponent,
  EditProjectLogoComponent,
  EditSocialMediaComponent,
  EditBankDataComponent,
  NewProjectMemberComponent,
  NewTokenComponent,
  ProjectCoverUploadComponent,
  StakingTokenClaimComponent
} from './modals';
import {ProjectPackagesModalComponent} from './components/project-packages/project-packages-modal/project-packages-modal.component';
import {ProjectReviewStatusComponent} from './components/project-review-status/project-review-status.component';
import {KangaPostPurchaseInfoComponent} from "./components/payment/kanga-exchange/kanga-post-purchase-info/kanga-post-purchase-info.component";
import {TransactionThankYouComponent} from "./pages/transaction-thank-you/transaction-thank-you.component";
import {SwiperModule} from 'swiper/angular';
import {RightSideComponent} from './components/project-overview/right-site/right-side.component';
import {PaymentCurrenciesListComponent} from "./components/project-overview/payment-currencies-list/payment-currencies-list.component";
import {InvestorCertificateComponent} from "./components/project-overview/investor-certificate/investor-certificate.component";
import {ProjectPartnersComponent} from './components/project-partners/project-partners.component';
import {ProjectPartnerAddEditComponent} from './components/project-partners/project-partner-add-edit/project-partner-add-edit.component';
import { AuthorizePrivateSaleComponent } from './pages/authorize-private-sale/authorize-private-sale.component';
import {InvestorCertificateLoaderComponent} from "./components/project-overview/investor-certificate/investor-certificate-loader/investor-certificate-loader.component";
import { BankTransferComponent } from './components/payment/bank-transfer/bank-transfer.component';
import { PaymentProcessorExplainerComponent } from './modals/payment-processor-explainer/payment-processor-explainer.component';
import { ProjectNftsComponent } from './components/project-nfts/project-nfts.component';
import { PageIntroVideoComponent } from './modals/page-intro-video/page-intro-video.component';
import { WalletModule } from '../wallet';
import { BinanceComponent } from './components/payment/binance/binance.component';
import { ExternalTransactionConfirmationComponent } from './pages/external-transaction-confirmation/external-transaction-confirmation.component';
import { PageReviewComponent } from './components/page-review/page-review.component';
import { InputNumberModule } from 'primeng/inputnumber';

@NgModule({
  declarations: [
    NewProjectComponent,
    ProjectAddDocumentModalComponent,
    ProjectComponent,
    ProjectSummaryComponent,
    ProjectCompanyDetailsComponent,
    ProjectNewsComponent,
    ProjectNewsFormComponent,
    ProjectNewsCardComponent,
    MetaWalletFundingComponent,
    ButtonBuyTokensComponent,
    ProjectTeamsComponent,
    ProjectTeamsAddEditComponent,
    ProjectTeamManageModalComponent,
    ProjectStageProgressComponent,
    EditProjectMembersComponent,
    NewProjectMemberComponent,
    ProjectInvitationComponent,
    ProjectMenuComponent,
    ProjectFooterComponent,
    ProjectOverviewComponent,
    MyDatePlPipe,
    ProjectSalesProgressBarComponent,
    ProjectFaqFormComponent,
    ProjectFaqComponent,
    FaqRowComponent,
    TokenEditorComponent,
    SalesInfoComponent,
    ArticlePhotoUploadComponent,
    ProjectFeaturesComponent,
    ProjectTokenomicsComponent,
    ProjectStagesComponent,
    SelectDocumentTemplateComponent,
    DocumentTemplateComponent,
    NewTokenComponent,
    ProjectDocumentTemplatesListComponent,
    CheckoutComponent,
    MosaicoWalletComponent,
    ActionSuccessComponent,
    ArticleCoverUploadComponent,
    KangaExchangeComponent,
    MetamaskWalletComponent,
    CreditCardComponent,
    ProjectFaqFormComponent,
    ProjectPackagesComponent,
    ProjectPackagesRowComponent,
    ProjectIconsSmComponent,
    ProjectPackagesFormComponent,
    ProjectPackagesModalComponent,
    ProjectFaqModalComponent,
    ProjectInfoComponent,
    ProjectCounterComponent,
    EditProjectLogoComponent,
    BlockchainLinksComponent,
    BlockchainAddressComponent,
    StatusFollowShareComponent,
    ExchangeListComponent,
    PaymentMethodListComponent,
    ProjectReviewStatusComponent,
    ProjectCoverUploadComponent,
    KangaPostPurchaseInfoComponent,
    TransactionThankYouComponent,
    RightSideComponent,
    EditMainColorsComponent,
    EditBankDataComponent,
    PaymentCurrenciesListComponent,
    InvestorCertificateComponent,
    InvestorCertificateLoaderComponent,
    EditSocialMediaComponent,
    ProjectPartnersComponent,
    ProjectPartnerAddEditComponent,
    AuthorizePrivateSaleComponent,
    ProjectSubscriptionToNewsletterComponent,
    ProjectTitleComponent,
    ProjectTitleDirective,
    BankTransferComponent,
    PaymentProcessorExplainerComponent,
    ProjectNftsComponent,
    StakingTokenClaimComponent,
    PageIntroVideoComponent,
    BinanceComponent,
    ExternalTransactionConfirmationComponent,
    PageReviewComponent
  ],
  imports: [
    CommonModule,
    ClipboardModule,
    ProjectManagementRoutingModule,
    RoleManagerModule,
    InlineSVGModule,
    CardsModule,
    FormsModule,
    ReactiveFormsModule,
    NgbNavModule,
    SharedModule,
    CKEditorModule,
    NgbAccordionModule,
    NgbPopoverModule,
    ShowMoreRowModule,
    StoreModule.forFeature(featureKey, projectReducers),
    MomentModule,
    NgApexchartsModule,
    NgbTooltipModule,
    RouterModule,
    NgbDatepickerModule,
    NgbModalModule,
    NgbDropdownModule,
    ImageCropperModule,
    ExtrasModule,
    ModalsModule,
    DrawersModule,
    DropdownMenusModule,
    DocumentManagementModule,
    InlineEditModule,
    MosaicoWalletModule,
    ImageCropperLocalModule,
    ButtonSaveModule,
    SpinnerMiniModule,
    NgSelectModule,
    NgbCollapseModule,
    SwiperModule,
    ColorEditModule,
    ImageModule,
    WalletModule,
    InputNumberModule
  ],
  exports: [
    MetaWalletFundingComponent
  ],
  providers: [
    AccessBuyGuard, NgbActiveModal
  ]
})
export class ProjectManagementModule {
  constructor(translateService: TranslationService) {
    translateService.loadTranslations(enLangEn, enLangPl);
  }
}
