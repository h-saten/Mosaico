import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { SharedModule } from '../shared';
import { EditDocumentContentsComponent } from './components';
import { DocumentViewerComponent } from './modals';

@NgModule({
  declarations: [
    EditDocumentContentsComponent,
    DocumentViewerComponent,
  ],
  imports: [
    SharedModule,
    CommonModule,
    ReactiveFormsModule,
    NgxDocViewerModule
  ],
  exports: [
    EditDocumentContentsComponent,
    DocumentViewerComponent
  ]
})
export class DocumentManagementModule {

}
