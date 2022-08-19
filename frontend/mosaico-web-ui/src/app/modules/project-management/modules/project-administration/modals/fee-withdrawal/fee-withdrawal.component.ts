import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormDialogBase } from 'mosaico-base';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-fee-withdrawal',
  templateUrl: './fee-withdrawal.component.html',
  styleUrls: ['./fee-withdrawal.component.scss']
})
export class FeeWithdrawalComponent extends FormDialogBase implements OnInit, OnDestroy {
  @Input() projectId: string;

  deploying$ = new BehaviorSubject<boolean>(false);
  subs = new SubSink();

  constructor(modalService: NgbModal) {
    super(modalService);
    this.extraOptions = {
      modalDialogClass: "mosaico-payment-modal"
    };
  }
  
  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
  }

  open(payload?: any): void {
    this.createForm();
    super.open(payload);
  }

  private createForm(): void {
    this.form = new FormGroup({
      walletAddress: new FormControl(null, [Validators.required])
    });
  }

  public confirm(): void {

  }

}
