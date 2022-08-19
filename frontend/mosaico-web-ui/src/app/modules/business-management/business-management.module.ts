import { InlineSVGModule } from 'ngx-svg-inline';
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { BusinessManagementRoutingModule } from './business-management-routing.module';
import { RoleManagerModule } from '../role-manager/role-manager.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { SharedModule } from '../shared';
import { StoreModule } from '@ngrx/store';
import { featureKey, CompanyReducers } from './store';
import { MomentModule } from 'ngx-moment';
import { NgApexchartsModule } from 'ng-apexcharts';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { ClipboardModule } from 'ngx-clipboard';
import { CompanyComponent , InvitationComponent, MyCompaniesComponent } from './pages';
import { locale as enLang } from './i18n/en';
import { locale as plLang } from './i18n/pl';
import { ImageCropperModule } from 'ngx-image-cropper';
import { NgbModule, NgbAccordionModule, NgbActiveModal, NgbDatepickerModule, NgbDropdownModule, NgbModalModule, NgbNavModule, NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslationService } from '../../../../projects/mosaico-base/src/lib/services/translation.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { SwiperModule } from 'swiper/angular';
import { CreateCompanyComponent } from './pages/create-company/create-company.component';
import { MosaicoWalletModule } from 'mosaico-wallet';
import { ProgressBarModule} from 'primeng/progressbar';
import { ImageCropperLocalModule } from '../shared-modules/image-cropper-local/image-cropper-local.module';
import { NewCompanyMemberComponent, CompanyWalletSendComponent, NewCompanyTokenComponent, EditCompanyLogoComponent } from './modals';
import { KnowYourBusinessComponent, CompanyOverviewComponent, EditCompanyComponent, CompanyTeamsComponent,
          CompanyProjectsComponent, CompanyMenuComponent, CompanyVotingComponent,
          CompanyWalletComponent, CompanyDetailsCardComponent, CompanyWalletSummaryComponent,
          CompanyWalletStatisticsComponent, CompanyWalletAssetsComponent, WalletAssetComponent, CompanyWalletTransactionsComponent} from './components';
import { SocialMediaComponent } from './components/social-media/social-media.component';
import { ButtonSaveModule } from '../shared-modules/button-save/button-save.module';
import { SubscriptionToNewsletterComponent } from './modals/subscription-to-newsletter/subscription-to-newsletter.component';
import { NewProposalComponent } from './modals/new-proposal/new-proposal.component';
import { CompanyDescriptionComponent } from './components/company-description/company-description.component';
import { AllCompaniesComponent } from './pages/all-companies/all-companies.component';
import { ComingSoonPageModule } from '../coming-soon-page/coming-soon-page.module';
import { CompanyHoldersComponent } from './components/company-holders/company-holders.component';
import { CompanyHoldersListComponent } from './components/company-holders/company-holders-list/company-holders-list.component';
import {ImportCompanyTokenComponent} from "./modals/import-token/import-token.component";
import { InputNumberModule } from 'primeng/inputnumber';

@NgModule({
    declarations: [
        CompanyComponent,
        MyCompaniesComponent,
        EditCompanyComponent,
        CompanyOverviewComponent,
        InvitationComponent,
        CompanyTeamsComponent,
        NewCompanyMemberComponent,
        KnowYourBusinessComponent,
        CompanyWalletTransactionsComponent,
        CompanyProjectsComponent,
        CompanyMenuComponent,
        CompanyVotingComponent,
        CompanyWalletComponent,
        CompanyDetailsCardComponent,
        CompanyWalletSummaryComponent,
        CompanyWalletStatisticsComponent,
        CompanyWalletAssetsComponent,
        WalletAssetComponent,
        CompanyWalletSendComponent,
        EditCompanyLogoComponent,
        CreateCompanyComponent,
        NewCompanyTokenComponent,
        SocialMediaComponent,
        SubscriptionToNewsletterComponent,
        NewProposalComponent,
        CompanyHoldersComponent,
        CompanyHoldersListComponent,
        CompanyDescriptionComponent,
        AllCompaniesComponent,
        ImportCompanyTokenComponent
    ],
    imports: [
      CommonModule,
      ClipboardModule,
      BusinessManagementRoutingModule,
      RoleManagerModule,
      InlineSVGModule,
      FormsModule,
      ReactiveFormsModule,
      NgbNavModule,
      SharedModule,
      NgbAccordionModule,
      NgbPopoverModule,
      MomentModule,
      ImageCropperModule,
      ImageCropperLocalModule,
      NgApexchartsModule,
      NgbTooltipModule,
      NgbAccordionModule,
      NgbPopoverModule,
      NgbModule,
      NgbDatepickerModule,
      NgbModalModule,
      NgbDropdownModule,
      StoreModule.forFeature(featureKey, CompanyReducers),
      NgSelectModule,
      MosaicoWalletModule,
      ProgressBarModule,
      ButtonSaveModule,
      ComingSoonPageModule,
      SwiperModule,
      InputNumberModule
    ],
    exports: [
    ],
    providers: [
    ]
  })
  export class BusinessManagementModule {
    constructor(translationService: TranslationService) {
      translationService.loadTranslations(enLang, plLang);
    }
  }
