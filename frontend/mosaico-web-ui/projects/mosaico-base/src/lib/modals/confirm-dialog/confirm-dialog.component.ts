import { Component, ContentChild, Input, OnChanges, OnInit, TemplateRef } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { DialogBase } from "../dialog-base";

@Component({
    selector: 'lib-confirm-dialog',
    templateUrl: './confirm-dialog.component.html',
    styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent extends DialogBase implements OnInit, OnChanges {
    public displayMode: 'message' | 'content';
    public payload: any;
    @Input() title: string;
    @Input() message: string;
    @Input() submitButtonText = 'Submit';
    @Input() cancelButtonText = 'Cancel';
    @Input() usePayload = false;

    @ContentChild(TemplateRef) modalContent: TemplateRef<any>;

    constructor(modalService: NgbModal) {
        super(modalService);
        this.size = 'sm';
    }

    ngOnInit() {
        this.ngOnChanges();
    }

    public open(payload?: any): void {
        this.payload = payload;
        super.open();
    }

    ngOnChanges(): void {
        this.displayMode = this.modalContent ? 'content' : 'message';
    }

    public submit(): void {
        if (this.modalRef) {
            if (this.usePayload) {
                this.modalRef.close(this.payload);
            } else {
                this.modalRef.close(true);
            }
        }
    }
}