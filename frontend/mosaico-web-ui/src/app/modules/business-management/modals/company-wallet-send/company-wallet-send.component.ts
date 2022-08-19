import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { CompanyTokenBalance, CompanyWalletService, SendCompanyTokensCommand, Token, WalletHubService } from 'mosaico-wallet';
import { FormDialogBase } from 'mosaico-base';
import { ToastrService } from 'ngx-toastr';
import { BlockchainService, ErrorHandlingService, validateForm } from 'mosaico-base';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SubSink } from 'subsink';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-company-wallet-send',
  templateUrl: './company-wallet-send.component.html',
  styleUrls: ['./company-wallet-send.component.scss']
})
export class CompanyWalletSendComponent extends FormDialogBase implements OnInit, OnDestroy {
  isLoading = false;
  companyId: string;
  network: string;
  subs = new SubSink();
  tokens: CompanyTokenBalance[] = [];
  selectedToken: CompanyTokenBalance;
  isSubmitting = false;
  deploying$ = new BehaviorSubject<boolean>(false);
  showTransactionSucceeded = false;

  constructor(modalService: NgbModal, private translateService: TranslateService, private walletService: CompanyWalletService, private errorHandler: ErrorHandlingService,
    private toastr: ToastrService, private blockchainService: BlockchainService, private hub: WalletHubService)
  { 
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
    this.form = new FormGroup({
      tokenId: new FormControl(null, [Validators.required]),
      address: new FormControl(null, [Validators.required]),
      amount: new FormControl(null, [Validators.required, Validators.min(0.1)])
    });
    this.subs.sink = this.form.get('tokenId').valueChanges.subscribe((res) => {
      if(res && res.length > 0){
        const token = this.tokens.find((t) => t.id === res);
        if(token){
          this.selectedToken = token;
        }
      }
      else {
        this.selectedToken = null;
      }
    });
  }

  open(companyId?: string, network?: string): void {
    this.createForm();
    this.resetState();
    this.companyId = companyId;
    this.network = network;
    this.hub.startConnection();
    this.hub.addWalletListeners();
    super.open();
    this.loadData();
    this.modalRef.closed.subscribe((res) => {
      this.hub.removeListener();
    });
    this.subs.sink = this.hub.sendTransactionSucceeded$.subscribe((res) => {
      if (res) {
        this.showSuccessMessage();
        this.showTransactionSucceeded = true;
        this.deploying$.next(false);
      }
    });
    this.subs.sink = this.hub.sendTransactionFailed$.subscribe((res) => {
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
    this.isSubmitting = false;
    this.isLoading = true;
    this.selectedToken = null;
    this.hub.resetObjects();
    this.deploying$ = new BehaviorSubject<boolean>(false);
    this.showTransactionSucceeded = false;
  }

  loadData(): void {
    if(this.companyId && this.companyId.length > 0) {
      this.subs.sink = this.walletService.getCompanyWalletTokens(this.companyId).subscribe((res) => {
        if(res && res.data){
          this.tokens = res.data?.tokens;
        }
        this.isLoading = false;
      });
    }
  }

  submit(): void {
    const formValue = this.form.getRawValue() as SendCompanyTokensCommand;
    if (formValue && validateForm(this.form) && this.blockchainService.isValidAddress(formValue.address)) {
      if(this.selectedToken && this.selectedToken.id === formValue.tokenId){
        if(this.selectedToken.isPaymentCurrency) {
          this.deploying$.next(true);
          const command = { paymentCurrencyId: this.selectedToken.id, amount: formValue.amount, address: formValue.address  };
          this.subs.sink = this.walletService.sendCurrency(this.companyId, command).subscribe((res) => {
            this.showTransactionInitiated();
          }, (error) => {this.errorHandler.handleErrorWithToastr(error); this.deploying$.next(false); });
          return;
        }
        else{
          this.deploying$.next(true);
          this.subs.sink = this.walletService.sendTokens(this.companyId, formValue).subscribe((res) => {
            this.showTransactionInitiated();
          }, (error) => {this.errorHandler.handleErrorWithToastr(error); this.deploying$.next(false); });
          return;
        }
      }
    }
    this.subs.sink = this.translateService.get('MODALS.WALLET_SEND.MESSAGES.INVALID_FORM').subscribe((t) => {
      this.toastr.error(t);
    });
  }

  showSuccessMessage(): void {
    this.subs.sink = this.translateService.get('MODALS.WALLET_SEND.MESSAGES.SUCCESS').subscribe((t) => {
      this.toastr.success(t);
    });
  }

  showTransactionInitiated(): void {
    this.subs.sink = this.translateService.get('MODALS.WALLET_SEND.MESSAGES.TRANSACTION_INITIATED').subscribe((t) => {
      this.toastr.success(t);
    });
  }

}
