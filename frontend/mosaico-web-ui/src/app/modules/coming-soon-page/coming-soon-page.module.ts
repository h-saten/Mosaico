import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComingSoonPageComponent } from './coming-soon-page.component';
import { ComingSoonPageRoutingModule } from './coming-soon-page-routing.module';
import { SharedModule } from '../shared';

@NgModule({
  declarations: [ComingSoonPageComponent],
  imports: [
    CommonModule,
    ComingSoonPageRoutingModule,
    SharedModule
  ],
  exports: [
    ComingSoonPageComponent
  ]
})
export class ComingSoonPageModule { }
