import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {
  CertificateEditorComponent,
  CertificateGeneratorComponent,
  ProjectAdministrationComponent,
  ProjectAirdropsComponent,
  ProjectCampaignComponent,
  ProjectConfigurationComponent,
  ProjectDashboardComponent,
  ProjectInvestorsComponent,
  ProjectTransactionsComponent
} from './components';
import {ProjectAdministrationRoutingModule} from './project-administration-routing.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgSelectModule} from '@ng-select/ng-select';
import {NgbAccordionModule, NgbModalModule, NgbModule, NgbTooltipModule} from '@ng-bootstrap/ng-bootstrap';
import {SharedModule} from '../../../shared';
import {NgApexchartsModule} from 'ng-apexcharts';
import {CreateAirdropComponent} from './modals/create-airdrop/create-airdrop.component';
import {MosaicoBaseModule, TranslationService} from 'mosaico-base';
import {ClipboardModule} from 'ngx-clipboard';
import {DragDropModule} from "@angular/cdk/drag-drop";

import {locale as enLangEn} from './i18n/en';
import {locale as enLangPl} from './i18n/pl';
import {ButtonSaveModule, ColorEditModule, SpinnerMiniModule} from "../../../shared-modules";
import {Ng2SearchPipeModule} from 'ng2-search-filter';
import {TableModule} from 'primeng/table';
import {InputTextModule} from 'primeng/inputtext';
import {AirdropWithdrawalComponent} from './modals/airdrop-withdrawal/airdrop-withdrawal.component';
import {AirdropParticipantsComponent} from './components/airdrop-participants/airdrop-participants.component';
import {StatisticsTilesComponent} from "./components/project-dashboard/statistics-tiles/statistics-tiles.component";
import {SaleStatisticsChartComponent} from "./components/project-dashboard/sale-statistics-chart/sale-statistics-chart.component";
import {TopInvestorsComponent} from "./components/project-dashboard/top-investors/top-investors.component";
import {VisitsChartComponent} from "./components/project-dashboard/visits-chart/visits-chart.component";
import {TransactionsChartComponent} from "./components/project-dashboard/transactions-chart/transactions-chart.component";
import {RaisedCapitalDailyChartComponent} from "./components/project-dashboard/raised-capital-daily-chart/raised-capital-daily-chart.component";
import {InvestorCardComponent} from './modals/investor-card/investor-card.component';
import { ConfirmBankTransferComponent } from './modals/confirm-bank-transfer/confirm-bank-transfer.component';
import {CalendarModule} from 'primeng/calendar';
import { FeesComponent } from './components/project-dashboard/fees/fees.component';
import { FeeWithdrawalComponent } from './modals/fee-withdrawal/fee-withdrawal.component';
import {FileUploadModule} from 'primeng/fileupload';
import { BasicInfoEditComponent } from './components/basic-info-edit/basic-info-edit.component';
import { EditSocialMediaComponent } from './components/edit-social-media/edit-social-media.component';
import { EditPageColorsComponent } from './components/edit-page-colors/edit-page-colors.component';
import { NewTokenComponent } from '../../modals';
import { EditProjectCoverComponent } from './components/edit-project-cover/edit-project-cover.component';
import { TransactionOperationsComponent } from './components/transaction-operations/transaction-operations.component';
import { TransactionFeeManagerComponent } from './components/transaction-fee-manager/transaction-fee-manager.component';
import { NewsletterSubscribersComponent } from './components/project-dashboard/newsletter-subscribers/newsletter-subscribers.component';
import { ProjectShortDescriptionComponent } from './components/project-short-description/project-short-description.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { VisibilityEditComponent } from './components/visibility-edit/visibility-edit.component';
import { AffiliationComponent } from './components/affiliation/affiliation.component';
import { ProjectAdminTokenManagementComponent } from './components/token-management/token-management.component';
import { WalletModule } from 'src/app/modules/wallet';
import { EditProjectFeeComponent } from './components/edit-project-fee/edit-project-fee.component';
import { AddPartnerComponent } from './modals/add-partner/add-partner.component';

@NgModule({
  declarations: [
    ProjectAdministrationComponent,
    ProjectCampaignComponent,
    ProjectConfigurationComponent,
    ProjectDashboardComponent,
    ProjectInvestorsComponent,
    ProjectTransactionsComponent,
    ProjectAirdropsComponent,
    CreateAirdropComponent,
    CertificateGeneratorComponent,
    CertificateEditorComponent,
    AirdropWithdrawalComponent,
    AirdropParticipantsComponent,
    StatisticsTilesComponent,
    SaleStatisticsChartComponent,
    TopInvestorsComponent,
    VisitsChartComponent,
    TransactionsChartComponent,
    RaisedCapitalDailyChartComponent,
    InvestorCardComponent,
    ConfirmBankTransferComponent,
    FeesComponent,
    FeeWithdrawalComponent,
    BasicInfoEditComponent,
    EditSocialMediaComponent,
    EditPageColorsComponent,
    EditProjectCoverComponent,
    TransactionOperationsComponent,
    TransactionFeeManagerComponent,
    NewsletterSubscribersComponent,
    ProjectShortDescriptionComponent,
    VisibilityEditComponent,
    AffiliationComponent,
    ProjectAdminTokenManagementComponent,
    EditProjectFeeComponent,
    AddPartnerComponent
  ],
  imports: [
    CommonModule,
    ClipboardModule,
    ProjectAdministrationRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    NgbTooltipModule,
    NgbAccordionModule,
    SharedModule,
    Ng2SearchPipeModule,
    TableModule,
    InputTextModule,
    NgApexchartsModule,
    NgbModalModule,
    MosaicoBaseModule,
    ClipboardModule,
    DragDropModule,
    NgbModule,
    MosaicoBaseModule,
    SharedModule,
    ButtonSaveModule,
    SpinnerMiniModule,
    MosaicoBaseModule,
    CalendarModule,
    FileUploadModule,
    ColorEditModule,
    TranslateModule,
    WalletModule
  ]
})
export class ProjectAdministrationModule {
  constructor(translateService: TranslationService) {
    translateService.loadTranslations(enLangEn, enLangPl);
  }
}
