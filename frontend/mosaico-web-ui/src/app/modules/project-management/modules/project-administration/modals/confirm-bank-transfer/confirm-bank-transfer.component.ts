import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { BlockchainService, ErrorHandlingService, FormDialogBase } from 'mosaico-base';
import { CrowdsaleHubService } from 'mosaico-project';
import { OrdersService, Transaction } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-confirm-bank-transfer',
  templateUrl: './confirm-bank-transfer.component.html',
  styleUrls: ['./confirm-bank-transfer.component.scss']
})
export class ConfirmBankTransferComponent extends FormDialogBase implements OnInit, OnDestroy{
  transactionId: string;
  isLoading = true;
  subs = new SubSink();
  deploying$ = new BehaviorSubject<boolean>(false);
  showTransactionSucceeded = false;
  @Input() projectId: string;
  transaction: Transaction;

  constructor(modalService: NgbModal, private store: Store, private translateService: TranslateService, private errorHandler: ErrorHandlingService,
    private toastr: ToastrService, private blockchainService: BlockchainService, private hub: CrowdsaleHubService, private orderService: OrdersService) {
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

  createForm(): void {
    this.form = new FormGroup({});
  }

  open(transactionId: string): void {
    this.transactionId = transactionId;
    this.createForm();
    this.resetState();
    this.hub.startConnection();
    this.hub.addPurchaseListener();
    super.open();
    this.loadData();
    this.modalRef.closed.subscribe((res) => {
      this.hub.removeListener();
    });
    this.subs.sink = this.hub.purchaseSuccessful$.subscribe((res) => {
      if (res) {
        this.showSuccessMessage();
        this.showTransactionSucceeded = true;
        this.deploying$.next(false);
      }
    });
    this.subs.sink = this.hub.purchaseFailed$.subscribe((res) => {
      if (res) {
        this.toastr.error(res);
        this.deploying$.next(false);
      }
    });
    this.subs.sink = this.deploying$.subscribe((deploying) => {
      if (deploying === true) {
        this.form.disable();
      }
      else {
        this.form.enable();
      }
    });
  }

  private resetState(): void {
    this.isLoading = true;
    this.hub.resetObjects();
    this.deploying$ = new BehaviorSubject<boolean>(false);
    this.showTransactionSucceeded = false;
    this.subs.unsubscribe();
  }

  loadData(): void {
    this.isLoading = true;
    this.subs.sink = this.orderService.getTransaction(this.projectId, this.transactionId).subscribe((res) => {
      this.transaction = res?.data;
      this.isLoading = false;
    }, (error) => { this.errorHandler.handleErrorWithToastr(error); });
  }

  showSuccessMessage(): void {
    this.toastr.success('Transaction was successfully confirmed');
  }

  showTransactionInitiated(): void {
    this.toastr.success('Transaction initiated. Please, wait...');
  }

  submit(): void {
    if (this.transactionId?.length > 0 && this.projectId?.length > 0) {
          this.deploying$.next(true);
          this.subs.sink = this.orderService.confirmBankTransfer(this.projectId, this.transactionId).subscribe((res) => {
            this.showTransactionInitiated();
          }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.deploying$.next(false); });
    }
  }

}
