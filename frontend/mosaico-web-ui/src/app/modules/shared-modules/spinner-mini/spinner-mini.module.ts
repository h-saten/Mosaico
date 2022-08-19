import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpinnerMiniDirective } from './spinner-mini.directive';



@NgModule({
  declarations: [SpinnerMiniDirective],
  imports: [
    CommonModule
  ],
  exports: [SpinnerMiniDirective]
})
export class SpinnerMiniModule { }
