import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { TokenBalance, WalletService, SendWalletTokensCommand, Token, WalletHubService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BlockchainService, ErrorHandlingService, FormDialogBase, validateForm } from 'mosaico-base';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SubSink } from 'subsink';
import { Store } from '@ngrx/store';
import { selectWalletTokenBalance } from '../../store';
import { BehaviorSubject } from 'rxjs';
import { selectUserInformation } from '../../../user-management/store/user.selectors';

@Component({
  selector: 'app-user-wallet-send',
  templateUrl: './user-wallet-send.component.html',
  styleUrls: ['./user-wallet-send.component.scss']
})
export class WalletSendComponent extends FormDialogBase implements OnInit, OnDestroy {
  isLoading = false;
  walletAddress: string;
  network: string;
  subs = new SubSink();
  tokens: TokenBalance[] = [];
  selectedToken: TokenBalance;
  isSubmitting = false;
  deploying$ = new BehaviorSubject<boolean>(false);
  showTransactionSucceeded = false;
  constructor(modalService: NgbModal, private store: Store, private translateService: TranslateService, private walletService: WalletService, private errorHandler: ErrorHandlingService,
    private toastr: ToastrService, private blockchainService: BlockchainService, private hub: WalletHubService) {
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
      if (res && res.length > 0) {
        const token = this.tokens.find((t) => t.id === res);
        if (token) {
          this.selectedToken = token;
        }
      }
      else {
        this.selectedToken = null;
      }
    });
  }

  open(walletAddress?: string, network?: string): void {
    this.createForm();
    this.resetState();
    this.walletAddress = walletAddress;
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
    this.isLoading = true;
    this.selectedToken = null;
    this.hub.resetObjects();
    this.deploying$ = new BehaviorSubject<boolean>(false);
    this.showTransactionSucceeded = false;
  }

  loadData(): void {
    this.store.select(selectUserInformation).subscribe((user) => {
      this.walletService.getTokenBalance(user.id, this.network).subscribe((response) => {
        this.tokens = response?.data?.tokens;
        this.isLoading = false;
      });
    }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.modalRef.close(); });
  }

  submit(): void {
    const formValue = this.form.getRawValue() as SendWalletTokensCommand;
    if (formValue && validateForm(this.form) && this.blockchainService.isValidAddress(formValue.address)) {
      if (this.selectedToken && this.selectedToken.id === formValue.tokenId) {
        if (this.selectedToken.isPaymentCurrency) {
          this.deploying$.next(true);
          const command = { paymentCurrencyId: this.selectedToken.id, amount: formValue.amount, address: formValue.address };
          this.subs.sink = this.walletService.sendCurrency(this.walletAddress, this.network, command).subscribe((res) => {
            this.showTransactionInitiated();
          }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.deploying$.next(false); });
          return;
        }
        else {
          this.deploying$.next(true);
          this.subs.sink = this.walletService.sendTokens(this.walletAddress, this.network, formValue).subscribe((res) => {
            this.showTransactionInitiated();
          }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.deploying$.next(false); });
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
