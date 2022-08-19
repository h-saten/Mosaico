import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { ProjectCompanyDetailsComponent } from '../project-management/components';
import { ProjectPathEnum } from '../project-management/constants';
import { AccessBuyGuard, ProjectEditGuard, ProjectGuard } from '../project-management/guards';
import { ProjectComponent } from '../project-management/pages';
import { CheckoutComponent } from '../project-management/pages/checkout/checkout.component';
import { TransactionThankYouComponent } from '../project-management/pages/transaction-thank-you/transaction-thank-you.component';

const projectSubRoutes = [
  {
    path: '',
    component: ProjectCompanyDetailsComponent
  },
  {
    path: ProjectPathEnum.Settings,
    canLoad: [AuthGuard, ProjectEditGuard],
    loadChildren: () => import('../project-management/modules/project-administration/project-administration.module').then(m => m.ProjectAdministrationModule),
    data: { hideHeader: true}
  },
  {
    canActivate: [AccessBuyGuard, AuthGuard],
    path: ProjectPathEnum.Fund,
    component: CheckoutComponent,
    data: { hideHeader: true}
  },
  { path: 'orderConfirmation', canActivate: [AuthGuard], component: TransactionThankYouComponent, data: { hideHeader: true} },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

const routes: Routes = [
  {
    path: '',
    component: ProjectComponent,
    canActivate: [ProjectGuard],
    children: projectSubRoutes
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TokenPageRoutingModule { }
