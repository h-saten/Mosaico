import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RedirectMosaicoComponent} from './redirect-mosaico.component';

const routes: Routes = [
  {
    path: '',
    component: RedirectMosaicoComponent,
    data: {
      title: 'Redirect'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RedirectMosaicoRoutingModule { }

