import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MosFundRoutingModule } from './mos-fund-routing.module';
import { MosFundComponent } from './mos-fund.component';
import { InlineSVGModule } from 'ngx-svg-inline';
import { NgApexchartsModule } from 'ng-apexcharts';
import { TranslationService } from 'mosaico-base';
import { locale as enLangEn } from './../i18n/en';
import { locale as enLangPl } from './../i18n/pl';
import { TranslateModule } from '@ngx-translate/core';
import { ComingSoonPageModule } from '../../coming-soon-page/coming-soon-page.module';
import { SharedModule } from '../../shared';
import { MomentModule } from 'ngx-moment';

@NgModule({
  declarations: [
    MosFundComponent
  ],
  imports: [
    CommonModule,
    MosFundRoutingModule,
    InlineSVGModule,
    NgApexchartsModule,
    TranslateModule,
    ComingSoonPageModule,
    SharedModule,
    MomentModule
  ]
})
export class MosFundModule {
  constructor(translateService: TranslationService) {
    translateService.loadTranslations(enLangEn, enLangPl);
  }
}
