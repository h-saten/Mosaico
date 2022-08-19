import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ErrorsPageRoutingModule } from './errors-page-routing.module';
import { Error401Component, Error403Component } from './pages';

import { TranslateModule } from '@ngx-translate/core';
import { locale as enLang } from './i18n/en';
import { locale as plLang } from './i18n/pl';
import { TranslationService } from 'mosaico-base';


@NgModule({
  declarations: [
    Error401Component,
    Error403Component
  ],
  imports: [
    CommonModule,
    ErrorsPageRoutingModule,
    TranslateModule
  ]
})
export class ErrorsPageModule {
  constructor(translationService: TranslationService) {
    translationService.loadTranslations(enLang, plLang);
  }
}
