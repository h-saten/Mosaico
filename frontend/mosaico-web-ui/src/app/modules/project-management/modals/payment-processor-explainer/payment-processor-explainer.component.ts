import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogBase } from 'mosaico-base';

@Component({
  selector: 'app-payment-processor-explainer',
  templateUrl: './payment-processor-explainer.component.html',
  styleUrls: ['./payment-processor-explainer.component.scss']
})
export class PaymentProcessorExplainerComponent extends DialogBase implements OnInit{
  localStorageItemKey = 'show-payment-explainer';

  constructor(modalService: NgbModal) { super(modalService); }

  ngOnInit(): void {
  }

  open(payload?: any): void {
    super.open(payload);
    if(localStorage.getItem(this.localStorageItemKey) === 'true'){
      this.modalRef.close(true);
    }
  }

  submit(store: boolean) {
    if(store === true) {
      localStorage.setItem(this.localStorageItemKey, 'true');
    }
    this.modalRef.close(true);
  }

}
