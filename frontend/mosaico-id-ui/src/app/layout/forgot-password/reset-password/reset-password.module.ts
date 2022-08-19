import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from '../../../shared/shared.module';
import {ResetPasswordRoutingModule} from './reset-password-routing.module';
import {ResetPasswordComponent} from './reset-password.component';
import {ForgotModule} from '../forgot/forgot.module';
import { RecaptchaV3Module } from "ng-recaptcha";

@NgModule({
    imports: [
        CommonModule,
        ResetPasswordRoutingModule,
        SharedModule,
        ForgotModule,
        RecaptchaV3Module
    ],
  declarations: [ResetPasswordComponent]
})
export class ResetPasswordModule { }
