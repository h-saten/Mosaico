import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LockedOutComponent} from './locked-out.component';

const routes: Routes = [
  {
    path: '',
    component: LockedOutComponent,
    data: {
      title: 'Locked out account'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LockedOutRoutingModule { }
