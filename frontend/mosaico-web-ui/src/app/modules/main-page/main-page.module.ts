import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { SwiperModule } from 'swiper/angular';
import { MainPageRoutingModule } from './main-page-routing.module';
import { MainPageComponent } from './pages';
import {
  SectionBuildYourCapitalComponent,
  SectionFollowStepsComponent,
  SectionFollowStepsInvestComponent,
  SectionFollowStepsCreateComponent,
  SectionInvestWithUsComponent,
  SectionIntroComponent,
  SectionMeetTeamComponent,
  SectionMobileApplicationComponent,
  SectionMobileAppComponent,
  SectionOurTeamComponent,
  SectionOurTeamManagementComponent,
  SectionPillarsComponent,
  SectionPillarsInvestmentComponent,
  SectionPlatformComponent,
  SectionWalletComponent,
  SectionGetintouchComponent,
  SectionProjectCarouselComponent
} from './components';
import { TranslationService } from 'mosaico-base';
import { SharedModule } from '../shared';

import { locale as enLangEn } from './i18n/en';
import { locale as enLangPl } from './i18n/pl';
import { SectionContactSalesComponent } from './components/section-contact-sales/section-contact-sales.component';
import {SpinnerMiniModule} from "../shared-modules";
// import { NgApexchartsModule } from 'ng-apexcharts';
// import { InlineSVGModule } from 'ngx-svg-inline';

@NgModule({
  declarations: [
    MainPageComponent,
    SectionBuildYourCapitalComponent,
    SectionContactSalesComponent,
    SectionFollowStepsComponent,
    SectionFollowStepsInvestComponent,
    SectionFollowStepsCreateComponent,
    SectionGetintouchComponent,
    SectionIntroComponent,
    SectionInvestWithUsComponent,
    SectionMeetTeamComponent,
    SectionMobileApplicationComponent,
    SectionOurTeamComponent,
    SectionOurTeamManagementComponent,
    SectionPlatformComponent,
    SectionPillarsComponent,
    SectionPillarsInvestmentComponent,
    SectionWalletComponent,
    SectionMobileAppComponent,
    SectionProjectCarouselComponent
  ],
  imports: [
    CommonModule,
    MainPageRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    SwiperModule,
    SpinnerMiniModule,
    NgbNavModule,
    // NgApexchartsModule,
    // InlineSVGModule
  ]
})
export class MainPageModule {
  constructor(translateService: TranslationService) {
    translateService.loadTranslations(enLangEn, enLangPl);
  }
}
