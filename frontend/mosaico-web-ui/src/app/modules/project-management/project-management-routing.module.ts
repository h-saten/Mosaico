import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/app/guards/auth.guard";
import { UserEvaluationGuard } from "../user-management/guards/user-evaluation.guard";
import {
  ProjectCompanyDetailsComponent
} from "./components";
import { ProjectPathEnum } from "./constants";
import { AccessBuyGuard, ProjectEditGuard, ProjectGuard } from "./guards";
import { NewProjectComponent, ProjectComponent, ProjectInvitationComponent } from "./pages";
import { AuthorizePrivateSaleComponent } from "./pages/authorize-private-sale/authorize-private-sale.component";
import { CheckoutComponent } from "./pages/checkout/checkout.component";
import { ExternalTransactionConfirmationComponent } from "./pages/external-transaction-confirmation/external-transaction-confirmation.component";
import { TransactionThankYouComponent } from "./pages/transaction-thank-you/transaction-thank-you.component";

const routes: Routes = [
  {
    path: 'create',
    component: NewProjectComponent,
    canActivate: [AuthGuard, UserEvaluationGuard]
  },
  {
    path: 'invitation',
    component: ProjectInvitationComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'invest',
    component: CheckoutComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'privateSale',
    component: AuthorizePrivateSaleComponent,
    canActivate: [AuthGuard, UserEvaluationGuard]
  },
  {
    path: 'orderConfirmation',
    component: ExternalTransactionConfirmationComponent
  },
  { path: ':projectId', loadChildren: () => import('../token-page/token-page.module').then(m => m.TokenPageModule) },
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
export class ProjectManagementRoutingModule { }
