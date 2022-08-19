import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabTouchDirective } from './mattab-touch.directive';

@NgModule({
  declarations: [MatTabTouchDirective],
  imports: [
    CommonModule
  ],
  exports: [MatTabTouchDirective]
})
export class MatTabTouchModule { }
