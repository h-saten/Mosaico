import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from '../../../shared/shared.module';
import {RegistrationSuccessRoutingModule} from './registration-success-routing.module';
import {RegistrationSuccessComponent} from './registration-success.component';
import {ForgotModule} from '../../forgot-password/forgot/forgot.module';

@NgModule({
    imports: [
        CommonModule,
        RegistrationSuccessRoutingModule,
        SharedModule,
        ForgotModule
    ],
  declarations: [RegistrationSuccessComponent]
})
export class RegistrationSuccessModule { }
