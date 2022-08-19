import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpinnerContainerComponent } from './spinner-container.component';
// import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  imports: [
    CommonModule,
    // NgxSpinnerModule
  ],
  declarations: [SpinnerContainerComponent],
  exports: [SpinnerContainerComponent]
})
export class SpinnerContainerModule { }
