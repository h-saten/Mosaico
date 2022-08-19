import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Error401Component } from './pages/error401/error401.component';
import { Error403Component } from './pages/error403/error403.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '401',
    pathMatch: 'full',
  },
  {
    path: '401',
    component: Error401Component
  },
  {
    path: '403',
    component: Error403Component
  },
  // {
  //   path: '',
  //   redirectTo: '401',
  //   pathMatch: 'full',
  // },
  {
    path: '**',
    redirectTo: '401',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ErrorsPageRoutingModule { }
