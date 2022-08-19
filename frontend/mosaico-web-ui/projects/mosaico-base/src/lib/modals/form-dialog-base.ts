import { Directive, EventEmitter, Output, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { DEFAULT_MODAL_OPTIONS } from '../constants';
import { FormBase } from '../utils';

@Directive()
export class FormDialogBase extends FormBase{
    @ViewChild(TemplateRef) content: TemplateRef<any>;
    @Output() closed: EventEmitter<any> = new EventEmitter<any>();
    @Output() dismissed: EventEmitter<any> = new EventEmitter<any>();
    protected modalRef: NgbModalRef;
    size: 'lg' | 'sm' | 'xl';
    extraOptions: NgbModalOptions;
    isOpened: boolean = false;

    constructor(protected modalService: NgbModal) {
        super();
        this.size = 'lg';
    }

    open(payload?: any): void {
        let options: NgbModalOptions = DEFAULT_MODAL_OPTIONS;
        if(this.extraOptions){
            options = {
                ...options,
                ...this.extraOptions
            };
        }
        this.modalRef = this.modalService.open(this.content, {...options, size: this.size});
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