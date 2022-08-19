import { KYCPage, ProfileComponent } from './pages';
import { UserManagementRootPage } from './pages';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditProfileComponent, ProfileDetailsComponent } from './components';
import { VerifyPhoneNumberComponent } from "./components/verify-phone-number/verify-phone-number.component";
import { UserProfileGuard } from './guards/user-profile.guard';
import { UserKycGuard } from './guards/user-kyc.guard';
import { EvaluationFormComponent } from './pages/evaluation-form/evaluation-form.component';
import { AuthGuard } from 'src/app/guards/auth.guard';


const routes: Routes = [
  {
    path: '',
    component: UserManagementRootPage,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'kyc',
        component: KYCPage,
        canActivate: [UserKycGuard],
      },
      {
        path: 'profile',
        component: ProfileComponent,
        canActivate: [UserProfileGuard],
        children: [
          {
            path: '',
            component: ProfileDetailsComponent,
          },
          {
            path: 'edit',
            component: EditProfileComponent,
          },
          {
            path: 'phone-number/verify',
            component: VerifyPhoneNumberComponent,
          },
          { path: '**', redirectTo: 'profile', pathMatch: 'full' }
        ]
      },
      {
        path: 'evaluation',
        component: EvaluationFormComponent
      },
      { path: '', redirectTo: 'profile', pathMatch: 'full' },
      { path: '**', redirectTo: 'profile', pathMatch: 'full' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserManagementRoutingModule { }
