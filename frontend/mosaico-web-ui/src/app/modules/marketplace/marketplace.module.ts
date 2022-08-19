import { InlineSVGModule } from 'ngx-svg-inline';
import { TranslationService } from 'mosaico-base';
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { MarketplaceRoutingModule } from './marketplace-routing.module';
import { ProjectsComponent, MyProjectsComponent } from './pages';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared';
import { MomentModule } from 'ngx-moment';
import { NgbTooltipModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { CardsModule } from 'src/app/_metronic';
import { CardProjectsComponent } from './components/card-projects/card-projects.component';
import { SalesProgressBarComponent } from './components/sales-progress-bar/sales-progress-bar.component';
import { ShowMoreRowModule } from 'src/app/modules/shared-modules/show-more-row/show-more-row.module';
import { MenuProjectsComponent } from './components/menu-projects/menu-projects.component';
import { MenuProjectsDirective } from './components/menu-projects/menu-projects.directive';
import {locale as enLangEn} from './i18n/en';
import {locale as enLangPl} from './i18n/pl';

@NgModule({
    declarations: [
        ProjectsComponent,
        MyProjectsComponent,
        CardProjectsComponent,
        SalesProgressBarComponent,
        MenuProjectsComponent,
        MenuProjectsDirective
    ],
    imports: [
      CommonModule,
      MarketplaceRoutingModule,
      InlineSVGModule,
      SharedModule,
      MomentModule,
      NgbTooltipModule,
      RouterModule,
      CardsModule,
      FormsModule,
      ReactiveFormsModule,
      ShowMoreRowModule,
      NgbDropdownModule
    ],
    exports: [
    ],
    providers: [
    ]
  })
  export class MarketplaceModule {
  constructor(translateService: TranslationService) {
    translateService.loadTranslations(enLangEn, enLangPl);
  }
}
