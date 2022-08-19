import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ForgotComponent } from './forgot.component';
import { ForgotRoutingModule } from './forgot-routing.module';
import { SharedModule } from '../../../shared/shared.module';
import {BackToLoginComponent} from '../../../shared/back-to-login/back-to-login.component';
import { RecaptchaV3Module } from "ng-recaptcha";

@NgModule({
  imports: [
    CommonModule,
    ForgotRoutingModule,
    SharedModule,
    RecaptchaV3Module
  ],
  exports: [
    BackToLoginComponent
  ],
  declarations: [ForgotComponent, BackToLoginComponent]
})
export class ForgotModule { }
