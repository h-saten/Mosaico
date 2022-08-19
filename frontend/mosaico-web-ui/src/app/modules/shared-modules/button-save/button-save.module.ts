import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonSaveComponent } from './button-save.component';
import { SpinnerMiniModule } from '../spinner-mini/spinner-mini.module';
import { SharedModule } from '../../shared/shared.module';



@NgModule({
  declarations: [ButtonSaveComponent],
  imports: [
    CommonModule,
    SpinnerMiniModule,
    SharedModule
  ],
  exports: [ButtonSaveComponent]
})
export class ButtonSaveModule { }
