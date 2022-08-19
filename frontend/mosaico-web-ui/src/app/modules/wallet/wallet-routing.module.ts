
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WalletComponent } from './pages';
import { StakingComponent, VestingComponent, WalletDashboardComponent } from './components';
import { TokenManagementComponent } from './pages/token-management/token-management.component';
import { DistributionComponent } from './components/distribution/distribution.component';
import { TokenGuard } from './guards/token.guard';
import { UserEvaluationGuard } from '../user-management/guards/user-evaluation.guard';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { StakingManagementComponent } from './components/staking-management/staking-management.component';
import { VestingManagementComponent } from './components/vesting-management/vesting-management.component';
import { UserAffiliationOverviewComponent } from './components/user-affiliation-overview/user-affiliation-overview.component';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { OperationsComponent } from './components/operations/operations.component';

const routes: Routes = [
    {
      path: '',
      component: WalletComponent,
      canActivate: [AuthGuard, UserEvaluationGuard],
      children: [
        {
          path: 'dashboard',
          component: WalletDashboardComponent
        },
        {
          path: 'staking',
          component: StakingComponent
        },
        {
          path: 'affiliation',
          component: UserAffiliationOverviewComponent
        },
        {
          path: 'vesting',
          component: VestingComponent
        },
        {
          path: '',
          redirectTo: 'dashboard'
        }
      ]
    },
    {
      path: 'operations',
      component: OperationsComponent,
      canActivate: [AuthGuard]
    },
    {
      path: 'token/:tokenId',
      component: TokenManagementComponent,
      canActivate: [AuthGuard, TokenGuard],
      children: [
        {
          path: 'dashboard',
          component: DashboardComponent
        },
        {
          path: 'staking',
          component: StakingManagementComponent
        },
        {
          path: 'vesting',
          component: VestingManagementComponent
        },
        {
          path: '',
          pathMatch: 'full',
          redirectTo: 'dashboard'
        },
        {
          path: '**',
          pathMatch: 'full',
          redirectTo: ''
        }
      ]
    },
    { path: '', redirectTo: '', pathMatch: 'full' },
    { path: '**', redirectTo: '', pathMatch: 'full' },
  ];

  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
  })
  export class WalletRoutingModule {}
