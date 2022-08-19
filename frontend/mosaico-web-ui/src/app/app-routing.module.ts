import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';
import { SigninComponent } from './components/signin/signin.component';

const routes: Routes = [
  {
    path: 'signin',
    component: SigninComponent
  },
  {
    path: '',
    loadChildren: () => import('./_metronic/layout/layout.module').then((m) => m.LayoutModule)
  },
  {
    path: '**',
    redirectTo: '',
  }
];

const routerOptions: ExtraOptions = {
  anchorScrolling: "enabled",
  scrollOffset: [0, 90],
  scrollPositionRestoration: 'enabled',
  // enableTracing: true
}

@NgModule({
  imports: [RouterModule.forRoot(routes, routerOptions)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
