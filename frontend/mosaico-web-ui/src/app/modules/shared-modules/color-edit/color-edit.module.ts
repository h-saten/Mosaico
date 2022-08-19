import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ColorEditComponent } from './color-edit.component';

import { ColorPickerModule } from 'ngx-color-picker';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';

@NgModule({
  declarations: [
    ColorEditComponent
  ],
  imports: [
    CommonModule,
    ColorPickerModule,
    FormsModule
  ],
  exports: [ColorEditComponent]
})
export class ColorEditModule { }
