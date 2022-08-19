import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {KangaLoginRedirectComponent} from './kanga-login-redirect.component';

const routes: Routes = [
  {
    path: '',
    component: KangaLoginRedirectComponent,
    data: {
      title: 'Locked out account'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KangaLoginRedirectRoutingModule { }
