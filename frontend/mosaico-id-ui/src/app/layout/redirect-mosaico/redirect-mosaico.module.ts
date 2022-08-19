import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from '../../shared/shared.module';
import {SpinnerContainerModule} from '../../shared/spinner-container/spinner-container.module';
import {RedirectMosaicoComponent} from './redirect-mosaico.component';
import {RedirectMosaicoRoutingModule} from './redirect-mosaico-routing.module';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        SpinnerContainerModule,
        RedirectMosaicoRoutingModule
    ],
  declarations: [RedirectMosaicoComponent]
})
export class RedirectMosaicoModule { }
