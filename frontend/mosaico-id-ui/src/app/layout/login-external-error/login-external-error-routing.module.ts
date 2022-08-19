import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginExternalErrorComponent} from './login-external-error.component';

const routes: Routes = [
  {
    path: '',
    component: LoginExternalErrorComponent,
    data: {
      title: 'Logout'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LoginExternalErrorRoutingModule { }
