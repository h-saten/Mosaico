import { AdminProjectsComponent } from './pages/admin-projects/admin-projects.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AdminCompaniesComponent } from './pages/admin-companies/admin-companies.component';
import { AdminUsersComponent } from './pages/admin-users/admin-users.component';
import { AdminUserDeletionRequestsComponent} from './pages/admin-user-deletion-requests/admin-user-deletion-requests.component'
import { AuthGuard } from 'src/app/guards/auth.guard';

const routes: Routes = [
  {
    path: 'projects',
    component: AdminProjectsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'companies',
    component: AdminCompaniesComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'users',
    component: AdminUsersComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'deletionrequests',
    component: AdminUserDeletionRequestsComponent,
    canActivate: [AuthGuard]
  },
  { path: '', redirectTo: 'projects', pathMatch: 'full' },
  { path: '**', redirectTo: 'projects', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule { }
