import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatExpansionModule } from '@angular/material/expansion';

import { SwiperModule } from 'swiper/angular';
import { TranslationService } from 'mosaico-base';

import { SharedModule } from '../shared';
import { SpinnerMiniModule } from '../shared-modules';

import { AboutUsComponent } from './about-us.component';
import { AboutUsRoutingModule } from './about-us-routing.module';
import { 
  CheckOpenPositionsComponent,
  CheckOutWhitepaperComponent, 
  CompanyRoadMapComponent,
  CompanyStructureComponent, 
  CompanyStructureCardComponent,
  ContactOurSalesComponent,
  DreamTeamComponent,
  GetInTouchComponent
} from './components/index';

import { locale as enLangEn } from './i18n/en';
import { locale as enLangPl } from './i18n/pl';

@NgModule({
  declarations: [
    AboutUsComponent,
    CheckOpenPositionsComponent,
    CheckOutWhitepaperComponent,
    CompanyRoadMapComponent,
    CompanyStructureComponent,
    CompanyStructureCardComponent,
    ContactOurSalesComponent,
    DreamTeamComponent,
    GetInTouchComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatExpansionModule,
    SwiperModule,
    AboutUsRoutingModule,
    SharedModule,
    SpinnerMiniModule
  ]
})
export class AboutUsModule { 
  constructor(translateService: TranslationService) {
    translateService.loadTranslations(enLangEn, enLangPl);
  }
}
