import { NgModule } from '@angular/core';
import { MosaicoBaseModule, TranslationService } from 'mosaico-base';
import { locale as enLang } from './i18n/en';
import { locale as plLang } from './i18n/pl';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InlineSVGModule } from 'ngx-svg-inline';
import { NgSelectModule } from '@ng-select/ng-select';
import { TransakModalComponent } from './components/transak-modal/transak-modal.component';
import { RampModalComponent } from './components/ramp-modal/ramp-modal.component';

@NgModule({
  declarations: [
    TransakModalComponent,
    RampModalComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MosaicoBaseModule,
    InlineSVGModule,
    NgSelectModule
  ],
  exports: [
    MosaicoBaseModule,
    TransakModalComponent,
    RampModalComponent
  ]
})
export class MosaicoWalletModule {
  constructor(translateService: TranslationService){
    translateService.loadTranslations(enLang, plLang);
  }
}
