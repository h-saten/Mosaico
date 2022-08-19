import { Component, Input } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogBase } from 'mosaico-base';

@Component({
  selector: 'app-document-viewer',
  templateUrl: './document-viewer.component.html',
  styleUrls: ['./document-viewer.component.scss']
})
export class DocumentViewerComponent extends DialogBase {
  @Input("documentTitle")
  documentTitle: string = "Document";
  @Input("documentAddress")
  documentAddress: string;

  constructor(modalService: NgbModal) {
    super(modalService);
    this.size = 'xl';
  }

  open(documentAddress?: string) {
    if (documentAddress) {
      this.documentAddress = documentAddress;
    }
    super.open();
  }

}
