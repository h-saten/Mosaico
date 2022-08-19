import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AirdropComponent } from './airdrop.component';

const routes: Routes = [
  { path: ':slug', component: AirdropComponent },
  {
    path: '**',
    redirectTo: 'error/404',
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AirdropRoutingModule { }
