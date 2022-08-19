import { InlineSVGModule } from 'ngx-svg-inline';
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { InvitationsRoutingModule } from './invitations-routing.module';
import { InvitationsComponent } from './pages';
// import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared';
import { MomentModule } from 'ngx-moment';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
// import { CardsModule } from 'src/app/_metronic';
// import { CardProjectsComponent } from './components/card-projects/card-projects.component';


@NgModule({
    declarations: [
        InvitationsComponent,
    ],
    imports: [
      CommonModule,
      InvitationsRoutingModule,
      InlineSVGModule,
      SharedModule,
      MomentModule,
      NgbTooltipModule,
      RouterModule,
      // CardsModule,
      // FormsModule,
      // ReactiveFormsModule,
    ],
    exports: [
    ],
    providers: [
    ]
  })
  export class InvitationsModule {}
