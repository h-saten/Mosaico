import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Registration',
      status: false
    },
    children: [
      {
        path: 'success',
        loadChildren: () => import('./registration-success/registration-success.module').then(m => m.RegistrationSuccessModule)
      },
      {
        path: '',
        loadChildren: () => import('./social-reg/social-reg.module').then(m => m.SocialRegModule)
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrationRoutingModule { }



