import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MosFundComponent } from './mos-fund.component';
import { ComingSoonPageComponent } from '../../coming-soon-page/coming-soon-page.component';

const routes: Routes = [
  {
    path: "",
    component: MosFundComponent
    // component: ComingSoonPageComponent
  },
  {
    path: '**',
    redirectTo: 'portfolio',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MosFundRoutingModule { }
