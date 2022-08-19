import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
// import { } from "./components";
// import { AccessBuyGuard, ProjectGuard } from "./guards";
import { InvitationsComponent } from "./pages";

const routes: Routes = [
    // {
    //   // Guard
    //   path: '',
    //   component: InvitationsComponent
    // },
    {
      path: '',
      redirectTo: 'my',
      pathMatch: 'full',
    },
    {
      // Guard
      path: 'my',
      component: InvitationsComponent
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
  ];

  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
  })
  export class InvitationsRoutingModule {}
