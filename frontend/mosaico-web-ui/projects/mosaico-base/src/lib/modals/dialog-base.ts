import { Output, EventEmitter, ViewChild, TemplateRef, Directive } from '@angular/core';
import { NgbModalRef, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { DEFAULT_MODAL_OPTIONS } from '../constants';

@Directive()
export abstract class DialogBase {
    @ViewChild(TemplateRef) content: TemplateRef<any>;
    @Output() closed: EventEmitter<any> = new EventEmitter<any>();
    @Output() dismissed: EventEmitter<any> = new EventEmitter<any>();
    protected modalRef: NgbModalRef;
    size: 'lg' | 'sm' | 'xl';
    extraOptions: NgbModalOptions;
    isOpened: boolean = false;

    constructor(protected modalService: NgbModal) {
        this.size = 'lg';
    }

    open(payload?: any): void {
        let options: NgbModalOptions = DEFAULT_MODAL_OPTIONS;
        if(this.extraOptions){
            options = {
                ...options,
                size: this.size,
                ...this.extraOptions
            };
        }
        this.modalRef = this.modalService.open(this.content, options);
        this.isOpened = true;
        this.modalRef.result.then((result) => {
            this.closed.emit(result);
            this.isOpened = false;
        }, (result) => {
            this.dismissed.emit(result);
            this.isOpened = false;
        });
    }
}