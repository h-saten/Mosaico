import { Routes } from '@angular/router';
import { SigninComponent } from '../components/signin/signin.component';
import { AuthGuard } from '../guards/auth.guard';
import { UserEvaluationGuard } from '../modules/user-management/guards/user-evaluation.guard';

const Routing: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('../modules/main-page/main-page.module').then((m) => m.MainPageModule),
  },
  {
    path: 'pl',
    loadChildren: () => import('../modules/lang/lang.module').then(m => m.LangModule),
    data: {
      lang: 'pl'
    },
  },
  {
    path: 'en',
    loadChildren: () => import('../modules/lang/lang.module').then(m => m.LangModule),
    data: {
      lang: 'en'
    },
  },
  {
    path: 'user',
    loadChildren: () =>
      import('../modules/user-management/user-management.module').then((m) => m.UserManagementModule),
  },
  {
    path: 'project',
    loadChildren: () =>
      import('../modules/project-management/project-management.module').then((m) => m.ProjectManagementModule),
  },
  {
    path: 'projects',
    loadChildren: () =>
      import('../modules/marketplace/marketplace.module').then((m) => m.MarketplaceModule),
  },
  {
    path: 'invitations',
    canLoad: [UserEvaluationGuard],
    loadChildren: () =>
      import('../modules/invitations/invitations.module').then((m) => m.InvitationsModule),
  },
  {
    path: 'dao',
    loadChildren: () =>
      import('../modules/business-management/business-management.module').then((m) => m.BusinessManagementModule),
  },
  {
    path: 'portfolio',
    loadChildren: () =>
      import('../modules/mos-fund/mos-fund/mos-fund.module').then((m) => m.MosFundModule),
  },
  {
    path: 'wallet',
    canLoad: [UserEvaluationGuard],
    loadChildren: () =>
      import('../modules/wallet/wallet.module').then((m) => m.WalletModule),
  },
  { path: 'airdrop', loadChildren: () => import('../modules/airdrop/airdrop.module').then(m => m.AirdropModule) },
  {
    path: 'admin',
    canLoad: [AuthGuard],
    data: {
      roles: ['ADMIN']
    },
    loadChildren: () =>
      import('../modules/admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: 'about',
    loadChildren: () => import('../modules/about-us/about-us.module').then(m => m.AboutUsModule)
  },
  { path: 'dex', loadChildren: () => import('../modules/dex/dex.module').then(m => m.DexModule) },
  {
    path: 'signin',
    component: SigninComponent
  },
  {
    path: 'coming-soon',
    loadChildren: () =>
      import('../modules/coming-soon-page/coming-soon-page.module').then((m) => m.ComingSoonPageModule)
  },
  {
    path: 'error',
    loadChildren: () => import('../modules/errors-page/errors-page.module').then(m => m.ErrorsPageModule)
  },
  { path: ':projectId', loadChildren: () => import('../modules/token-page/token-page.module').then(m => m.TokenPageModule) },
  {
    path: '**',
    redirectTo: '/projects',
    pathMatch: 'full'
  }
];

export { Routing };
