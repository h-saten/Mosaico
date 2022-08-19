import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SocialLoginComponent} from './social-login.component';
import {SocialLoginRoutingModule} from './social-login-routing.module';
import {SharedModule} from '../../../shared/shared.module';
import {SpinnerContainerModule} from '../../../shared/spinner-container/spinner-container.module';
import {RecaptchaV3Module} from "ng-recaptcha";
import {DeviceAuthorizationModule} from "./device-authorization/device-authorization.module";

@NgModule({
    imports: [
        CommonModule,
        SocialLoginRoutingModule,
        SharedModule,
        SpinnerContainerModule,
        RecaptchaV3Module,
        DeviceAuthorizationModule
    ],
  declarations: [SocialLoginComponent]
})
export class SocialLoginModule { }
