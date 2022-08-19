import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportAccountStolenRoutingModule } from './report-account-stolen-routing.module';
import { ReportAccountStolenComponent } from './report-account-stolen.component';
import { TranslateModule } from '@ngx-translate/core';


@NgModule({
  declarations: [
    ReportAccountStolenComponent
  ],
  imports: [
    CommonModule,
    ReportAccountStolenRoutingModule,
    TranslateModule
  ]
})
export class ReportAccountStolenModule { }
