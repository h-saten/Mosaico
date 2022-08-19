import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthComponent } from './layout/auth/auth.component';
import {LoginResolve} from './resolvers/LoginResolve';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  },
  {
    path: 'auth/redirect',
    component: AuthComponent,
    loadChildren: () => import('./layout/redirect-mosaico/redirect-mosaico.module').then(m => m.RedirectMosaicoModule),
    data: {
      path_0: 'redirect',
      title: 'auth.login.title'
    }
  },
  {
    path: 'auth/login',
    component: AuthComponent,
    loadChildren: () => import('./layout/login/social-login/social-login.module').then(m => m.SocialLoginModule),
    data: {
      path_0: 'login',
      title: 'auth.login.title'
    },
    resolve: { ico: LoginResolve }
  },
  {
    path: 'fb',
    component: AuthComponent,
    loadChildren: () => import('./layout/login/social-login/social-login.module').then(m => m.SocialLoginModule),
    data: {
      path_0: 'login',
      title: 'auth.login.title'
    },
    resolve: { ico: LoginResolve }
  },
  {
    path: 'google',
    component: AuthComponent,
    loadChildren: () => import('./layout/login/social-login/social-login.module').then(m => m.SocialLoginModule),
    data: {
      path_0: 'login',
      title: 'auth.login.title'
    },
    resolve: { ico: LoginResolve }
  },
  {
    path: 'kanga-login/:token',
    component: AuthComponent,
    loadChildren: () => import('./layout/login/kanga-login/kanga-login.module').then(m => m.KangaLoginModule),
    data: {
      path_0: 'login',
      title: 'auth.login.title'
    },
    resolve: { ico: LoginResolve }
  },
  {
    path: 'kanga-login/:token',
    component: AuthComponent,
    loadChildren: () => import('./layout/login/kanga-login/kanga-login.module').then(m => m.KangaLoginModule),
    data: {
      path_0: 'login',
      title: 'auth.login.title'
    },
    resolve: { ico: LoginResolve }
  },
  {
    path: 'login/kanga',
    loadChildren: () => import('./layout/login/kanga-login-redirect/kanga-login-redirect.module')
      .then(m => m.KangaLoginModule),
    data: {
      path_0: 'login',
      title: 'auth.login.title'
    },
    resolve: { ico: LoginResolve }
  },
  {
    path: 'auth/forgot',
    component: AuthComponent,
    loadChildren: () => import('./layout/forgot-password/forgot/forgot.module').then(m => m.ForgotModule),
    data: {
      path_0: 'forgot',
      title: 'auth.forgot.title'
    },
  },
  {
    path: 'auth/registration',
    component: AuthComponent,
    loadChildren: () => import('./layout/registration/social-reg/social-reg.module').then(m => m.SocialRegModule),
    data: {
      path_0: 'registration',
      title: 'auth.register.title',
      data_administration_info: true
    },
  },
  {
    path: 'auth/registration/success',
    component: AuthComponent,
    loadChildren: () => import('./layout/registration/registration-success/registration-success.module').then(m => m.RegistrationSuccessModule),
    data: {
      path_0: 'registration',
      title: 'auth.register.success.title'
    },
  },
  {
    path: 'auth/resetPassword',
    component: AuthComponent,
    loadChildren: () => import('./layout/forgot-password/reset-password/reset-password.module').then(m => m.ResetPasswordModule),
    data: {
      path_0: 'registration',
      title: 'auth.reset_password.title'
    },
  },
  {
    path: 'auth/changeEmail',
    component: AuthComponent,
    loadChildren: () => import('./layout/change-email/change-email/change-email.module').then(m => m.ChangeEmailModule),
    data: {
      path_0: 'changeEmail',
      title: 'auth.change_email.title'
    },
  },
  {
    path: 'auth/reportStolen',
    component: AuthComponent,
    loadChildren: () => import('./layout/report-account-stolen/report-account-stolen.module').then(m => m.ReportAccountStolenModule),
    data: {
      path_0: 'reportStolen',
      title: 'auth.report_account_stolen.title'
    },
  },
  {
    path: 'auth/confirmEmail',
    component: AuthComponent,
    loadChildren: () => import('./layout/registration/confirm-email/confirm-email.module').then(m => m.ConfirmEmailModule),
    data: {
      path_0: 'registration',
      title: 'auth.confirm_email.title'
    },
  },
  {
    path: 'auth/lockedOut',
    component: AuthComponent,
    loadChildren: () => import('./layout/login/locked-out/locked-out.module').then(m => m.LockedOutModule),
    data: {
      path_0: 'registration',
      title: 'auth.locked_out.title'
    },
  },
  {
    path: 'auth/logout',
    component: AuthComponent,
    loadChildren: () => import('./layout/logout/logout.module').then(m => m.LogoutModule),
    data: {
      path_0: 'registration',
      title: 'auth.logout.title'
    },
  },
  {
    path: 'auth/login/external/error',
    component: AuthComponent,
    loadChildren: () => import('./layout/login-external-error/login-external-error.module').then(m => m.LoginExternalErrorModule),
    data: {
      path_0: 'login',
      title: 'auth.login.title'
    },
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
