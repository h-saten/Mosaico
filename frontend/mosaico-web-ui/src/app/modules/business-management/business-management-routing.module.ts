import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CompanyComponent, InvitationComponent, MyCompaniesComponent } from "./pages";
import { EditCompanyComponent } from './components';
import { CompanyGuard } from './guards/company.guard';
import { CompanyProjectsComponent } from './components/company-projects/company-projects.component';
import { CompanyVotingComponent } from './components/company-voting/company-voting.component';
import { CompanyWalletComponent } from './components/company-wallet/company-wallet.component';
import { CreateCompanyComponent } from './pages/create-company/create-company.component';
import { AllCompaniesComponent } from './pages/all-companies/all-companies.component';
import { CompanyHoldersComponent } from './components/company-holders/company-holders.component';
import { UserEvaluationGuard } from "../user-management/guards/user-evaluation.guard";
import { AuthGuard } from "src/app/guards/auth.guard";

const routes: Routes = [
    {
      path: 'create',
      component: CreateCompanyComponent,
      canActivate: [AuthGuard, UserEvaluationGuard]
    },
    {
      path: 'my',
      component: MyCompaniesComponent,
      canActivate: [AuthGuard, UserEvaluationGuard]
    },
    {
      path: 'invitation/:id',
      component: InvitationComponent,
      pathMatch: 'full'
    },
    {
      path: ':id',
      component: CompanyComponent,
      canActivate: [CompanyGuard],
      children:
      [
        {
          path: 'overview',
          component: CompanyProjectsComponent,
        },
        {
          path: 'voting',
          component: CompanyVotingComponent,
        },
        {
          path: 'wallet',
          component: CompanyWalletComponent,
        },
        {
          path: 'holders',
          component: CompanyHoldersComponent,
        },
        {
          path: 'settings',
          component: EditCompanyComponent,
          canActivate: [AuthGuard]
        },
        { path: '', redirectTo: 'overview', pathMatch: 'full' },
        { path: '**', redirectTo: 'overview', pathMatch: 'full' }
      ]
    },
    {
      path: '',
      component: AllCompaniesComponent
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
  ];

  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
  })
  export class BusinessManagementRoutingModule {}
