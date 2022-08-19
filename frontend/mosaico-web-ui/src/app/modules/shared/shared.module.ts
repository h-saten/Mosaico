import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgbTooltipModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { AddressPipe, NoCommaPipe } from './pipes';
import {
  SpinnerContainerComponent,
  LanguagePickerComponent,
  ComingSoonComponent,
  MeetTeamSocialLinksComponent,
  NoItemsComponent,
  EditDetailsComponent,
  FeaturedProjectComponent,
  FeaturedProjectDetailsComponent,
  FeaturedProjectFooterComponent,
  FeaturedProjectHeaderComponent,
  JoinOurNewsletterComponent
} from './components';

import { TranslationService } from 'mosaico-base';
import { locale as enLangEn } from './i18n/en';
import { locale as enLangPl } from './i18n/pl';
import { BaseColorsDirective } from './directives';
import { TokenCreationComponent } from './components/token-creation/token-creation.component';
import { TokenTypeSelectorComponent } from './components/token-creation/token-type-selector/token-type-selector.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { InlineSVGModule } from 'ngx-svg-inline';
import { WalletPaymentControlComponent } from './components/wallet-payment-control/wallet-payment-control.component';
import { CompanyWalletPaymentControlComponent } from './components/company-wallet-payment-control/company-wallet-payment-control.component';
import { BeamerComponent } from './components/beamer/beamer.component';
import {TokenImportComponent} from "./components/token-import/token-import.component";
import { StonlyDirective } from '../../services/stonly.directive';
import { SubscriptionToNewsletterComponent } from '../project-management/modals';

@NgModule({
  declarations: [
    AddressPipe,
    NoCommaPipe,
    SpinnerContainerComponent,
    LanguagePickerComponent,
    ComingSoonComponent,
    BaseColorsDirective,
    TokenCreationComponent,
    TokenTypeSelectorComponent,
    WalletPaymentControlComponent,
    CompanyWalletPaymentControlComponent,
    BeamerComponent,
    TokenImportComponent,
    StonlyDirective,
    SubscriptionToNewsletterComponent,
    MeetTeamSocialLinksComponent,
    NoItemsComponent,
    EditDetailsComponent,
    FeaturedProjectComponent,
    FeaturedProjectDetailsComponent,
    FeaturedProjectFooterComponent,
    FeaturedProjectHeaderComponent,
    JoinOurNewsletterComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    NgbTooltipModule,
    NgbDropdownModule,
    ReactiveFormsModule,
    RouterModule,
    NgSelectModule,
    InlineSVGModule
  ],
  exports: [
    AddressPipe,
    NoCommaPipe,
    TranslateModule,
    SpinnerContainerComponent,
    LanguagePickerComponent,
    ComingSoonComponent,
    BaseColorsDirective,
    TokenCreationComponent,
    TokenTypeSelectorComponent,
    WalletPaymentControlComponent,
    CompanyWalletPaymentControlComponent,
    MeetTeamSocialLinksComponent,
    NoItemsComponent,
    BeamerComponent,
    TokenImportComponent,
    StonlyDirective,
    SubscriptionToNewsletterComponent,
    EditDetailsComponent,
    FeaturedProjectComponent,
    FeaturedProjectDetailsComponent,
    FeaturedProjectFooterComponent,
    FeaturedProjectHeaderComponent,
    JoinOurNewsletterComponent
  ]
})
export class SharedModule {
  constructor(translateService: TranslationService) {
    translateService.loadTranslations(enLangEn, enLangPl);
  }
}
