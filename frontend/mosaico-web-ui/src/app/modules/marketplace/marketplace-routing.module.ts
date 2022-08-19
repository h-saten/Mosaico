import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MyProjectsComponent, ProjectsComponent } from "./pages";
import { ProjectsPathEnum, ProjectsStatusEnum } from './';
import { AuthGuard } from "src/app/guards/auth.guard";

/*
export interface DataRouters {
  path_1: ProjectsPathEnum;
  status: ProjectsStatusEnum;
}
*/

const routes: Routes = [
    {
      path: ProjectsPathEnum.Main,
      component: ProjectsComponent,
      data: {
        status: ProjectsStatusEnum.All,
        path: ProjectsPathEnum.Main
      }
    },
    {
      path: ProjectsPathEnum.Sale,
      component: ProjectsComponent,
      data: {
        status: ProjectsStatusEnum.Sale,
        path: ProjectsPathEnum.Sale
      }
    },
    {
      path: ProjectsPathEnum.PrivateSale,
      component: ProjectsComponent,
      data: {
        status: ProjectsStatusEnum.PrivateSale,
        path: ProjectsPathEnum.PrivateSale
      }
    },
    {
      path: ProjectsPathEnum.PreSale,
      component: ProjectsComponent,
      data: {
        status: ProjectsStatusEnum.PreSale,
        path: ProjectsPathEnum.PreSale
      }
    },
    {
      path: ProjectsPathEnum.Upcoming,
      component: ProjectsComponent,
      data: {
        status: ProjectsStatusEnum.Upcoming,
        path: ProjectsPathEnum.Upcoming
      }
    },
    {
      path: ProjectsPathEnum.InReview,
      component: ProjectsComponent,
      data: {
        status: ProjectsStatusEnum.InReview,
        path: ProjectsPathEnum.InReview
      }
    },
    {
      path: ProjectsPathEnum.SecondaryMarket,
      component: ProjectsComponent,
      data: {
        status: ProjectsStatusEnum.SecondaryMarket,
        path: ProjectsPathEnum.SecondaryMarket
      }
    },
    {
      path: ProjectsPathEnum.Closed,
      component: ProjectsComponent,
      data: {
        status: ProjectsStatusEnum.Closed,
        path: ProjectsPathEnum.Closed
      }
    },

    {
      path: ProjectsPathEnum.My,
      component: MyProjectsComponent,
      canActivate: [AuthGuard]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
  ];

  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
  })
  export class MarketplaceRoutingModule {}
