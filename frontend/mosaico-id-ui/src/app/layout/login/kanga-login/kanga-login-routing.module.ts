import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {KangaLoginComponent} from './kanga-login.component';

const routes: Routes = [
  {
    path: '',
    component: KangaLoginComponent,
    data: {
      title: 'Locked out account'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KangaLoginRoutingModule { }
