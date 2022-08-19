import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {EditProjectMembersComponent} from '../../components';
import {ProjectPathEnum} from '../../constants';
import {
  ProjectAdministrationComponent,
  ProjectAirdropsComponent,
  ProjectCampaignComponent,
  ProjectConfigurationComponent,
  ProjectDashboardComponent,
  ProjectInvestorsComponent,
  ProjectTransactionsComponent
} from './components';
import { AirdropParticipantsComponent } from './components/airdrop-participants/airdrop-participants.component';
import { CertificateGeneratorComponent } from './components/certificate-generator/certificate-generator.component';
import { AffiliationComponent } from './components/affiliation/affiliation.component';
import { DashboardComponent } from '../../../wallet/components/dashboard/dashboard.component';
import { StakingManagementComponent } from 'src/app/modules/wallet/components/staking-management/staking-management.component';
import { VestingManagementComponent } from '../../../wallet/components/vesting-management/vesting-management.component';
import { ProjectAdminTokenManagementComponent } from './components/token-management/token-management.component';
import { TokenGuard } from 'src/app/modules/wallet/guards/token.guard';
import { AuthGuard } from 'src/app/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: ProjectAdministrationComponent,
    data: {
      path: ProjectPathEnum.Settings,
    },
    canActivate: [AuthGuard],
    children: [
      {
        path: 'dashboard',
        component: ProjectDashboardComponent
      },
      {
        path: 'transactions',
        component: ProjectTransactionsComponent
      },
      {
        path: 'investors',
        component: ProjectInvestorsComponent,
      },
      {
        path: 'tokenomy',
        component: ProjectAdminTokenManagementComponent,
        canActivate: [TokenGuard],
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
      {
        path: 'affiliation',
        component: AffiliationComponent,
      },
      {
        path: 'airdrops',
        children: [
          {
            path: '',
            component: ProjectAirdropsComponent
          },
          {
            path: ':id/participants',
            component: AirdropParticipantsComponent
          },
          {
            path: '**',
            redirectTo: ''
          }
        ]
      },
      // {
      //   path: 'campaign',
      //   component: ProjectCampaignComponent
      // },
      {
        path: 'certificate-generator',
        component: CertificateGeneratorComponent
      },
      {
        path: 'members',
        component: EditProjectMembersComponent
      },
      {
        path: 'configuration',
        component: ProjectConfigurationComponent
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: '**',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ],
  },
  {
    path: '**',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectAdministrationRoutingModule { }
