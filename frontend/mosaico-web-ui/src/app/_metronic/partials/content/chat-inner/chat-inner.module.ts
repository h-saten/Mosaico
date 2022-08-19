import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InlineSVGModule } from 'ngx-svg-inline';
import { ChatInnerComponent } from './chat-inner.component';

@NgModule({
  declarations: [ChatInnerComponent],
  imports: [CommonModule, InlineSVGModule],
  exports: [ChatInnerComponent],
})
export class ChatInnerModule {}
