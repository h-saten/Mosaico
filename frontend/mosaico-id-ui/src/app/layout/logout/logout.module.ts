import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {LogoutComponent} from './logout.component';
import {LogoutRoutingModule} from './logout-routing.module';
import {SharedModule} from '../../shared/shared.module';
import {SpinnerContainerModule} from '../../shared/spinner-container/spinner-container.module';
import {TranslateModule} from '@ngx-translate/core';

@NgModule({
    imports: [
        CommonModule,
        LogoutRoutingModule,
        SharedModule,
        SpinnerContainerModule,
        TranslateModule
    ],
  declarations: [LogoutComponent]
})
export class LogoutModule { }
