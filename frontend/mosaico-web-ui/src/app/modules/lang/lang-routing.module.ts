import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SigninComponent } from 'src/app/components/signin/signin.component';
import { UserEvaluationGuard } from '../user-management/guards/user-evaluation.guard';
import { LangComponent } from './components/lang/lang.component';

const routes: Routes = [
    {
        path: '',
        component: LangComponent,
        pathMatch: 'full',
        loadChildren: () => import('../../modules/main-page/main-page.module').then((m) => m.MainPageModule),
    },
    {
        path: 'user',
        component: LangComponent,
        loadChildren: () =>
            import('../../modules/user-management/user-management.module').then((m) => m.UserManagementModule),
    },
    {
        path: 'project',
        component: LangComponent,
        loadChildren: () =>
            import('../../modules/project-management/project-management.module').then((m) => m.ProjectManagementModule),
    },
    {
        path: 'projects',
        component: LangComponent,
        loadChildren: () =>
            import('../../modules/marketplace/marketplace.module').then((m) => m.MarketplaceModule),
    },
    {
        path: 'invitations',
        component: LangComponent,
        canLoad: [UserEvaluationGuard],
        loadChildren: () =>
            import('../../modules/invitations/invitations.module').then((m) => m.InvitationsModule),
    },
    {
        path: 'dao',
        component: LangComponent,
        loadChildren: () =>
            import('../../modules/business-management/business-management.module').then((m) => m.BusinessManagementModule),
    },
    {
        path: 'portfolio',
        component: LangComponent,
        loadChildren: () =>
            import('../../modules/mos-fund/mos-fund/mos-fund.module').then((m) => m.MosFundModule),
    },
    {
        path: 'wallet',
        component: LangComponent,
        loadChildren: () =>
            import('../../modules/wallet/wallet.module').then((m) => m.WalletModule),
    },
    { path: 'airdrop', component: LangComponent, loadChildren: () => import('../../modules/airdrop/airdrop.module').then(m => m.AirdropModule) },
    {
        path: 'admin',
        component: LangComponent,
        loadChildren: () =>
            import('../../modules/admin/admin.module').then((m) => m.AdminModule),
    },
    {
        path: 'about',
        component: LangComponent,
        loadChildren: () => import('../about-us/about-us.module').then(m => m.AboutUsModule)
    },
    { path: 'dex', component: LangComponent, loadChildren: () => import('../../modules/dex/dex.module').then(m => m.DexModule) },
    {
        path: 'signin',
        component: SigninComponent
    },
    {
        component: LangComponent,
        path: 'coming-soon',
        loadChildren: () =>
            import('../../modules/coming-soon-page/coming-soon-page.module').then((m) => m.ComingSoonPageModule)
    },
    {
        component: LangComponent,
        path: 'error',
        loadChildren: () => import('../../modules/errors-page/errors-page.module').then(m => m.ErrorsPageModule)
    },
    {  component: LangComponent, path: ':projectId', loadChildren: () => import('../../modules/token-page/token-page.module').then(m => m.TokenPageModule) },
    {
        path: '**',
        redirectTo: '/projects',
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LangRoutingModule { }
