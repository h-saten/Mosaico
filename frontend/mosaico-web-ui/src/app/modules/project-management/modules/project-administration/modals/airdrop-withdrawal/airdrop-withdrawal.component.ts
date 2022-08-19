import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { BlockchainService, ErrorHandlingService, FormDialogBase } from 'mosaico-base';
import { Airdrop, AirdropService } from 'mosaico-project';
import { WalletService, WalletHubService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-airdrop-withdrawal',
  templateUrl: './airdrop-withdrawal.component.html',
  styleUrls: ['./airdrop-withdrawal.component.scss']
})
export class AirdropWithdrawalComponent extends FormDialogBase implements OnInit, OnDestroy {
  airdropId: string;
  isLoading = true;
  subs = new SubSink();
  airdrop: Airdrop;
  deploying$ = new BehaviorSubject<boolean>(false);
  showTransactionSucceeded = false;
  pendingUsersCount = 0;
  pendingTokens = 0;
  @Input() projectId: string;

  constructor(modalService: NgbModal, private store: Store, private translateService: TranslateService, private service: AirdropService, private errorHandler: ErrorHandlingService,
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
    this.form = new FormGroup({});
  }

  open(airdropId: string): void {
    this.airdropId = airdropId;
    this.createForm();
    this.resetState();
    this.hub.startConnection();
    this.hub.addAirdropListeners();
    super.open();
    this.loadData();
    this.modalRef.closed.subscribe((res) => {
      this.hub.removeListener();
    });
    this.subs.sink = this.hub.airdropWithdrawn$.subscribe((res) => {
      if (res) {
        this.showSuccessMessage();
        this.showTransactionSucceeded = true;
        this.deploying$.next(false);
      }
    });
    this.subs.sink = this.hub.airdropWithdrawalFailed$.subscribe((res) => {
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
    this.subs.sink = this.service.getAirdrop(this.airdropId).subscribe((res) => {
      this.airdrop = res?.data;
      this.pendingTokens = this.airdrop.pendingParticipants * this.airdrop.tokensPerParticipant;
      this.pendingUsersCount = this.airdrop.pendingParticipants;
      this.isLoading = false;
    }, (error) => { this.errorHandler.handleErrorWithToastr(error); });
  }

  showSuccessMessage(): void {
    this.toastr.success('Airdrop successfully distributed');
  }

  showTransactionInitiated(): void {
    this.toastr.success('Transaction initiated. Please, wait...');
  }

  submit(): void {
    if (this.airdropId?.length > 0 && this.projectId?.length > 0) {
          this.deploying$.next(true);
          this.subs.sink = this.service.distribute(this.projectId, this.airdropId).subscribe((res) => {
            this.showTransactionInitiated();
          }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.deploying$.next(false); });
    }
  }
}
