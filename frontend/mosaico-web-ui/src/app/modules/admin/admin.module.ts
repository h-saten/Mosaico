import { InlineSVGModule } from 'ngx-svg-inline';
import { TranslationService } from 'mosaico-base';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminProjectsComponent } from './pages/admin-projects/admin-projects.component';
import { AdminCompaniesComponent } from './pages/admin-companies/admin-companies.component';
import { AdminUserDeletionRequestsComponent} from './pages/admin-user-deletion-requests/admin-user-deletion-requests.component';
import { AdminUsersComponent } from './pages/admin-users/admin-users.component';
import { SharedModule } from '../shared';
import { NgbDropdownModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DropdownMenusModule } from 'src/app/_metronic';
import { FormsModule } from '@angular/forms';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import {TableModule} from 'primeng/table';
import {InputTextModule} from 'primeng/inputtext';
import {locale as enLangEn} from './i18n/en';
import {locale as enLangPl} from './i18n/pl';


@NgModule({
    declarations: [
      AdminProjectsComponent,
      AdminCompaniesComponent,
      AdminUsersComponent,
      AdminUserDeletionRequestsComponent
    ],
    imports: [
      CommonModule,
      AdminRoutingModule,
      InlineSVGModule,
      SharedModule,
      NgbDropdownModule,
      DropdownMenusModule,
      FormsModule,
      Ng2SearchPipeModule,
      NgbModule,
      TableModule,
      InputTextModule,
      TableModule
    ],
  })
  export class AdminModule {
  constructor(translateService: TranslationService) {
    translateService.loadTranslations(enLangEn, enLangPl);
  }
}
