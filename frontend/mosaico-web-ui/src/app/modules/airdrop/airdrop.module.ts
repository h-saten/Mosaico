import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AirdropRoutingModule } from './airdrop-routing.module';
import { AirdropComponent } from './airdrop.component';
import { TranslateModule } from '@ngx-translate/core';
import { locale as enLang } from './i18n/en';
import { locale as plLang } from './i18n/pl';
import { TranslationService } from 'mosaico-base';

@NgModule({
  declarations: [
    AirdropComponent
  ],
  imports: [
    CommonModule,
    AirdropRoutingModule,
    TranslateModule
  ]
})
export class AirdropModule {
  constructor(translationService: TranslationService) {
    translationService.loadTranslations(enLang, plLang);
  }
}
