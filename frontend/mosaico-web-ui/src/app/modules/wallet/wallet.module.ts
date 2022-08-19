import {WalletWidgetComponent} from './components/wallet-widget/wallet-widget.component';
import {DropdownMenuComponent} from './components/dropdown-menu/dropdown-menu.component';
import {WalletRoutingModule} from './wallet-routing.module';
import {InlineSVGModule} from 'ngx-svg-inline';
import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {EditTokenLogoComponent, WalletSendComponent, ConfirmModalComponent} from './modals';
import {DepositStakeComponent, WalletComponent} from './pages';
import {MomentModule} from 'ngx-moment';
import {VestingTokenDynamicComponent} from './modals/vesting-token-dynamic/vesting-token-dynamic.component';
import {NgApexchartsModule} from 'ng-apexcharts';
import {SharedModule} from '../shared';
import {
  NgbAccordionModule,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbModalModule,
  NgbModule,
  NgbNavModule,
  NgbPopoverModule,
  NgbTooltipModule
} from '@ng-bootstrap/ng-bootstrap';
import {ClipboardModule} from 'ngx-clipboard';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { SwiperModule } from 'swiper/angular';
import {locale as enLang} from './i18n/en';
import {locale as plLang} from './i18n/pl';
import {NgSelectModule} from '@ng-select/ng-select';
import {TranslationService} from '../../../../projects/mosaico-base/src/lib/services/translation.service';
import {ImageCropperModule} from 'ngx-image-cropper';
import {RoleManagerModule} from '../role-manager/role-manager.module';
import {StakingWalletComponent} from "./components/staking-wallet/staking-wallet.component";
import {StakingWithdrawalsComponent} from "./components/staking-wallet/staking-withdrawals/staking-wallet.component";
import {TokenStakingsComponent} from "./components/staking-wallet/token-stakings/token-stakings.component";
import {StoreModule} from "@ngrx/store";
import {featureKey, walletReducers} from "./store";
import {
  StakingComponent,
  WalletDashboardComponent,
  WalletOverviewComponent,
  WalletOverviewPackagesComponent,
  WalletOverviewStakingComponent,
  WalletOverviewVestingComponent,
  WalletPanelAssetsComponent,
  WalletPanelComponent,
  WalletPanelTransactionsComponent,
  WalletSummaryChartComponent,
  WalletSummaryComponent,
  WalletSummaryDetailsComponent,
  WalletSummaryDiagramComponent
} from './components';
import {MosaicoBaseModule} from 'mosaico-base';
import {MosaicoWalletModule} from 'mosaico-wallet';
import {WalletPanelAssetRowComponent} from './components';
import {TokenManagementComponent} from './pages/token-management/token-management.component';
import {VestingComponent} from './components/vesting/vesting.component';
import {DistributionComponent} from './components/distribution/distribution.component';
import {EnableStakingComponent} from './modals/enable-staking/enable-staking.component';
import {WalletPanelKangaAssetsComponent} from "./components/wallet-panel/wallet-panel-kanga-assets/wallet-panel-kanga-assets.component";
import {WalletPanelKangaAssetRowComponent} from "./components/wallet-panel/wallet-panel-kanga-assets/wallet-panel-kanga-assets-row/wallet-panel-kanga-assets-row.component";
import { ButtonSaveModule, ImageCropperLocalModule } from '../shared-modules';
import { TokenDeployComponent } from './modals/token-deploy/token-deploy.component';
import { TokenMintComponent } from './modals/token-mint/token-mint.component';
import { TokenBurnComponent } from './modals/token-burn/token-burn.component';
import { ComingSoonPageModule } from '../coming-soon-page/coming-soon-page.module';
import { NewStageComponent } from './modals/new-stage/new-stage.component';
import { CrowdsaleDeploymentComponent } from './modals/crowdsale-deployment/crowdsale-deployment.component';
import { StageDeploymentComponent } from './modals/stage-deployment/stage-deployment.component';
import { AuthorizationCodeComponent } from './modals/authorization-code/authorization-code.component';
import { ManualDepositComponent } from './modals/manual-deposit/manual-deposit.component';
import { StageEditingComponent } from './components/stage-editing/stage-editing.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ReserveComponent } from './components/reserve/reserve.component';
import { StakingStatisticsComponent } from './components/staking/staking-statistics/staking-statistics.component';
import { StakingAssetsComponent } from './components/staking/staking-assets/staking-assets.component';
import { TopStakingAssetsComponent } from './components/staking/top-staking-assets/top-staking-assets.component';
import { StakingPanelComponent } from './components/staking/staking-panel/staking-panel.component';
import { StakingPanelActiveComponent } from './components/staking/staking-panel/staking-panel-active/staking-panel-active.component';
import { StakingPanelHistoryComponent } from './components/staking/staking-panel/staking-panel-history/staking-panel-history.component';
import { MissingVaultComponent } from './components/distribution/components/missing-vault/missing-vault.component';
import { VaultDeploymentComponent } from './modals/vault-deployment/vault-deployment.component';
import { VaultDepositCreationComponent } from './modals/vault-deposit-creation/vault-deposit-creation.component';
import {CalendarModule} from 'primeng/calendar';
import { VaultSendTokensComponent } from './modals/vault-send-tokens/vault-send-tokens.component';
import { StakingManagementComponent } from './components/staking-management/staking-management.component';
import { VestingManagementComponent } from './components/vesting-management/vesting-management.component';
import { PrivateSaleVestingManagementComponent } from './components/private-sale-vesting-management/private-sale-vesting-management.component';
import { PersonalVestingManagementComponent } from './components/personal-vesting-management/personal-vesting-management.component';
import { NewVestingComponent } from './modals/new-vesting/new-vesting.component';
import { StakeModalComponent } from './modals/stake-modal/stake-modal.component';
import { UserAffiliationOverviewComponent } from './components/user-affiliation-overview/user-affiliation-overview.component';
import { TableModule } from 'primeng/table';
import { EditTokenStakingComponent } from './modals/edit-token-staking/edit-token-staking.component';
import { EditTokenVestingComponent } from './modals/edit-token-vesting/edit-token-vesting.component';
import { EditTokenDeflationComponent } from './modals/edit-token-deflation/edit-token-deflation.component';
import { WithdrawModalComponent } from './modals/withdraw-modal/withdraw-modal.component';
import { ClaimModalComponent } from './modals/claim-modal/claim-modal.component';
import { OperationsComponent } from './components/operations/operations.component';
import {InputNumberModule} from 'primeng/inputnumber';

@NgModule({
  declarations: [
      WalletComponent,
      WalletSendComponent,
      WalletWidgetComponent,
      DropdownMenuComponent,
      VestingTokenDynamicComponent,
      DepositStakeComponent,
      StakingWalletComponent,
      StakingWithdrawalsComponent,
      TokenStakingsComponent,
      WalletDashboardComponent,
      WalletSummaryComponent,
      WalletSummaryDetailsComponent,
      WalletSummaryDiagramComponent,
      WalletPanelAssetRowComponent,
      WalletSummaryChartComponent,
      WalletOverviewComponent,
      WalletOverviewStakingComponent,
      WalletOverviewVestingComponent,
      WalletOverviewPackagesComponent,
      WalletPanelComponent,
      WalletPanelAssetsComponent,
      WalletPanelTransactionsComponent,
      TokenManagementComponent,
      StakingComponent,
      VestingComponent,
      DistributionComponent,
      EnableStakingComponent,
      WalletPanelKangaAssetsComponent,
      WalletPanelKangaAssetRowComponent,
      EditTokenLogoComponent,
      TokenDeployComponent,
      TokenMintComponent,
      TokenBurnComponent,
      NewStageComponent,
      CrowdsaleDeploymentComponent,
      StageDeploymentComponent,
      AuthorizationCodeComponent,
      ManualDepositComponent,
      StageEditingComponent,
      DashboardComponent,
      ReserveComponent,
      StakingStatisticsComponent,
      StakingAssetsComponent,
      TopStakingAssetsComponent,
      StakingPanelComponent,
      StakingPanelActiveComponent,
      StakingPanelHistoryComponent,
      MissingVaultComponent,
      VaultDeploymentComponent,
      VaultDepositCreationComponent,
      VaultSendTokensComponent,
      StakingManagementComponent,
      VestingManagementComponent,
      PrivateSaleVestingManagementComponent,
      PersonalVestingManagementComponent,
      NewVestingComponent,
      StakeModalComponent,
      UserAffiliationOverviewComponent,
      EditTokenStakingComponent,
      EditTokenVestingComponent,
      EditTokenDeflationComponent,
      WithdrawModalComponent,
      ClaimModalComponent,
      ConfirmModalComponent,
      OperationsComponent
  ],
  imports: [
      CommonModule,
      ClipboardModule,
      RoleManagerModule,
      InlineSVGModule,
      FormsModule,
      ReactiveFormsModule,
      NgbNavModule,
      NgbAccordionModule,
      NgbPopoverModule,
      MomentModule,
      ImageCropperModule,
      NgApexchartsModule,
      NgbTooltipModule,
      NgbModule,
      NgbDatepickerModule,
      NgbModalModule,
      NgbDropdownModule,
      NgSelectModule,
      SwiperModule,
      WalletRoutingModule,
      SharedModule,
      StoreModule.forFeature(featureKey, walletReducers),
      MosaicoBaseModule,
      MosaicoWalletModule,
      ButtonSaveModule,
      ImageCropperLocalModule,
      ComingSoonPageModule,
      CalendarModule,
      TableModule,
      InputNumberModule
  ],
  exports: [
    TokenManagementComponent,
    ConfirmModalComponent,
  ]
})
export class WalletModule {
  constructor(translationService: TranslationService) {
    translationService.loadTranslations(enLang, plLang);
  }
}
