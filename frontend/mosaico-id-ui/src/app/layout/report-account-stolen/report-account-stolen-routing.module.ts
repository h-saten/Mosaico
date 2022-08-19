import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportAccountStolenComponent } from './report-account-stolen.component';

const routes: Routes = [
  {
    path: '',
    component: ReportAccountStolenComponent,
    data: {
      title: 'Report account stolen'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportAccountStolenRoutingModule { }
