import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslationService } from 'mosaico-base';
import { SharedModule } from '../shared';
import { DexRoutingModule } from './dex-routing.module';
import { ExchangeComponent } from './components/exchange/exchange.component';
import { locale as enLang } from './i18n/en';
import { locale as plLang } from './i18n/pl';
import { ComingSoonPageModule } from '../coming-soon-page/coming-soon-page.module';


@NgModule({
  declarations: [
    ExchangeComponent
  ],
  imports: [
    CommonModule,
    NgSelectModule,
    SharedModule,
    DexRoutingModule,
    ComingSoonPageModule
  ]
})
export class DexModule {
  constructor(translationService: TranslationService) {
    translationService.loadTranslations(enLang, plLang);
  }
}
