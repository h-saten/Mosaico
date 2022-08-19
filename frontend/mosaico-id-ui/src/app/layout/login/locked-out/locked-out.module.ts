import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../shared/shared.module';
import {LockedOutComponent} from './locked-out.component';
import {LockedOutRoutingModule} from './locked-out-routing.module';

@NgModule({
  imports: [
    CommonModule,
    LockedOutRoutingModule,
    SharedModule
  ],
  declarations: [LockedOutComponent]
})
export class LockedOutModule { }
