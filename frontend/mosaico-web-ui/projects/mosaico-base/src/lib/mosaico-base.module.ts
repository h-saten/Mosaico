import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { ConfirmDialogComponent, InformationDialogComponent } from './modals';


@NgModule({
  declarations: [
    ConfirmDialogComponent,
    InformationDialogComponent
  ],
  imports: [
    CommonModule,
    TranslateModule
  ],
  exports: [
    TranslateModule,
    ConfirmDialogComponent,
    InformationDialogComponent
  ]
})
export class MosaicoBaseModule { }
