import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from '../../shared/shared.module';
import {SpinnerContainerModule} from '../../shared/spinner-container/spinner-container.module';
import {LoginExternalErrorRoutingModule} from './login-external-error-routing.module';
import {LoginExternalErrorComponent} from './login-external-error.component';

@NgModule({
    imports: [
        CommonModule,
        LoginExternalErrorRoutingModule,
        SharedModule,
        SpinnerContainerModule
    ],
  declarations: [LoginExternalErrorComponent]
})
export class LoginExternalErrorModule { }
