import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LangComponent } from './components/lang/lang.component';
import { LangRoutingModule } from './lang-routing.module';

@NgModule({
  declarations: [
    LangComponent
  ],
  imports: [
    CommonModule,
    LangRoutingModule
  ]
})
export class LangModule { }
