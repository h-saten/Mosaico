import { WalletTokenListComponent } from './../../../../modules/wallet/components/wallet-token-list/wallet-token-list.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InlineSVGModule } from 'ngx-svg-inline';
import { NotificationsInnerComponent } from './dropdown-inner/notifications-inner/notifications-inner.component';
import { QuickLinksInnerComponent } from './dropdown-inner/quick-links-inner/quick-links-inner.component';
import { UserInnerComponent } from './dropdown-inner/user-inner/user-inner.component';
import { LayoutScrollTopComponent } from './scroll-top/scroll-top.component';
import { WalletInnerComponent } from './dropdown-inner';
import { NonCustodialWalletInnerComponent } from './dropdown-inner/non-custodial-wallet-inner/non-custodial-wallet-inner.component';
import { MosaicoBaseModule, TranslationService } from 'mosaico-base';
import { SharedModule } from 'src/app/modules/shared';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { ClipboardModule } from 'ngx-clipboard';
import {locale as enLangEn} from './i18n/en';
import {locale as enLangPl} from './i18n/pl';

@NgModule({
  declarations: [
    NotificationsInnerComponent,
    QuickLinksInnerComponent,
    UserInnerComponent,
    LayoutScrollTopComponent,
    WalletInnerComponent,
    WalletTokenListComponent,
    NonCustodialWalletInnerComponent
  ],
  imports: [CommonModule, InlineSVGModule, RouterModule, MosaicoBaseModule, SharedModule, NgbTooltipModule, ClipboardModule],
  exports: [
    NotificationsInnerComponent,
    QuickLinksInnerComponent,
    UserInnerComponent,
    LayoutScrollTopComponent,
    WalletInnerComponent,
    WalletTokenListComponent,
    NonCustodialWalletInnerComponent
  ],
})
export class ExtrasModule {
  constructor(translationService: TranslationService) {
    translationService.loadTranslations(enLangEn, enLangPl);
  }
}

