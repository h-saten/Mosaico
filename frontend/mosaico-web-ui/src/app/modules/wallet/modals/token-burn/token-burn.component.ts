import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormDialogBase, validateForm } from 'mosaico-base';
import { ActiveBlockchainService, Blockchain, DeploymentEstimate, DeploymentEstimateHubService, SystemWallet, SystemWalletService, Token, TokenService, WalletHubService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';
import { selectMetamaskChainId } from '../../store';

@Component({
  selector: 'app-token-burn',
  templateUrl: './token-burn.component.html',
  styleUrls: ['./token-burn.component.scss']
})
export class TokenBurnComponent extends FormDialogBase implements OnInit, OnDestroy {

  subs = new SubSink();
  deploying = new BehaviorSubject<boolean>(false);
  @Input() token: Token;
  networks: Blockchain[] = [];
  contractVersionToDeploy = '';
  constructor(modalService: NgbModal, private translateService: TranslateService, private tokenService: TokenService, private toastr: ToastrService,
    private errorHandler: ErrorHandlingService,
    private store: Store,
    private walletHub: WalletHubService) {
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

  open(): void {
    this.createForm();
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((b) => {
      this.networks = b;
    });
    this.walletHub.startConnection();
    this.walletHub.addBurningListeners();

    this.subs.sink = this.walletHub.tokenBurned$.subscribe((result) => {
      if(result && result === this.token.id){
        this.subs.sink = this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.BURN_SUCCESS').subscribe((t) => {
          this.toastr.success(t);
          this.deploying.next(false);
        });
        this.modalRef.close(true);
      }
    });

    this.subs.sink = this.walletHub.tokenBurningFailed$.subscribe((error) => {
      if(error && error.length > 0) {
        this.toastr.error(error);
        this.deploying.next(false);
      }
    });

    this.subs.sink = this.deploying.subscribe((v) => {
      if(v === true) {
        this.form.disable();
      }
      else{
        this.form.enable();
      }
    });
    super.open();
    this.modalRef.dismissed.subscribe(() => {
      this.stopHubs();
      this.subs.unsubscribe();
    });
    this.modalRef.closed.subscribe(() => {
      this.stopHubs();
      this.subs.unsubscribe();
    });
  }

  private stopHubs(): void {
    this.walletHub.removeListener();
    this.walletHub.resetObjects();    
  }

  createForm(): void {
    this.form = new FormGroup({
      wallet: new FormControl('MOSAICO_WALLET', [Validators.required]),
      amount: new FormControl(null, [Validators.required, Validators.min(1), Validators.max(100000000)])
    });
  }

  async save(): Promise<void> {
    if (validateForm(this.form)) {
      const wallet = this.form.get('wallet').value;
      const amount = this.form.get('amount').value;
      if(wallet === 'METAMASK') {
        this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.UNSUPPORTED').subscribe((t) => {
          this.toastr.error(t);
        });
      }
      else if(wallet === 'MOSAICO_WALLET'){
        this.deploying.next(true);
        this.subs.sink = this.tokenService.burn(this.token.id, {amount})
        .subscribe((response) => {
          this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.TRANSACTION_INITIATED').subscribe((t) => {
            this.toastr.success(t);
          });
        }, (error) => { this.deploying.next(false); this.errorHandler.handleErrorWithToastr(error); });
      }
    }
    else {
      this.subs.sink = this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.INVALID_FORM').subscribe((res) => {
        this.toastr.error(res);
      });
    }
  }
}
