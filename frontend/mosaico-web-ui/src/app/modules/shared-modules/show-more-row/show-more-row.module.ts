import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShowMoreRowComponent } from './show-more-row.component';
import { SharedModule } from 'src/app/modules/shared';


@NgModule({
  declarations: [ShowMoreRowComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  exports: [ShowMoreRowComponent]
})
export class ShowMoreRowModule { }
