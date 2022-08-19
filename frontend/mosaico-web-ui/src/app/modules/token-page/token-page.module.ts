import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TokenPageRoutingModule } from './token-page-routing.module';
import { ProjectManagementModule } from '../project-management/project-management.module';


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    TokenPageRoutingModule,
    ProjectManagementModule
  ]
})
export class TokenPageModule { }
