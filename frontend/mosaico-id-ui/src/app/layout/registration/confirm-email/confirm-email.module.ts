import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../shared/shared.module';
import {ConfirmEmailRoutingModule} from './confirm-email-routing.module';
import {ConfirmEmailComponent} from './confirm-email.component';
import {SpinnerContainerModule} from '../../../shared/spinner-container/spinner-container.module';
import {TranslateModule} from '@ngx-translate/core';

@NgModule({
    imports: [
        CommonModule,
        ConfirmEmailRoutingModule,
        SharedModule,
        SpinnerContainerModule,
        TranslateModule
    ],
  declarations: [ConfirmEmailComponent]
})
export class ConfirmEmailModule { }
