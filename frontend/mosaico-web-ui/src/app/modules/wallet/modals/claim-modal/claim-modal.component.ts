import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { FormDialogBase, ErrorHandlingService, BlockchainService } from 'mosaico-base';
import { WalletStake, WalletHubService, StakingService, Token } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-claim-modal',
  templateUrl: './claim-modal.component.html',
  styleUrls: ['./claim-modal.component.scss']
})
export class ClaimModalComponent extends FormDialogBase implements OnInit, OnDestroy {
  subs = new SubSink();
  isLoading = true;
  deploying$ = new BehaviorSubject<boolean>(false);
  showTransactionSucceeded = false;
  stake: WalletStake;
  token: Token;
  balance: number;

  constructor(modalService: NgbModal, private store: Store, private translateService: TranslateService, private errorHandler: ErrorHandlingService,
    private toastr: ToastrService, private blockchainService: BlockchainService, private hub: WalletHubService, private stakingService: StakingService) {
    super(modalService);
    this.extraOptions = {
      modalDialogClass: "mosaico-staking-modal"
    };
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
  }

  submit(): void {
    if (this.stake) {
      this.deploying$.next(true);
      this.subs.sink = this.stakingService.claimReward(this.stake.id).subscribe((res) => {
        this.showTransactionInitiated();
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.deploying$.next(false); });
    }
  }

  open(payload?: WalletStake) {
    this.resetState();
    this.stake = payload;
    this.hub.startConnection();
    this.hub.addStakeRewardListeners();
    super.open(payload);
    this.modalRef.closed.subscribe((res) => {
      this.hub.removeListener();
    });
    this.subs.sink = this.stakingService.getRewardEstimate(this.stake.id).subscribe((res) => {
      this.token = res?.data?.token;
      this.balance = res?.data?.balance;
      this.isLoading = false;
    }, (error) => {
      this.errorHandler.handleErrorWithToastr(error);
      this.modalRef.close(false);
    });
    this.subs.sink = this.hub.stakeRewardClaimedSuccessfully$.subscribe((res) => {
      if (res) {
        this.showSuccessMessage();
        this.showTransactionSucceeded = true;
        this.deploying$.next(false);
      }
    });
    this.subs.sink = this.hub.stakeRewardClaimFailed$.subscribe((res) => {
      if (res) {
        this.toastr.error(res);
        this.deploying$.next(false);
      }
    });
    this.subs.sink = this.deploying$.subscribe((deploying) => {
      if (deploying === true) {
        this.form?.disable();
      }
      else {
        this.form?.enable();
      }
    });
  }

  showSuccessMessage(): void {
    this.toastr.success('Transaction was successfully confirmed');
  }

  showTransactionInitiated(): void {
    this.toastr.success('Transaction initiated. Please, wait...');
  }

  private resetState(): void {
    this.isLoading = true;
    this.hub.resetObjects();
    this.deploying$ = new BehaviorSubject<boolean>(false);
    this.showTransactionSucceeded = false;
    this.subs.unsubscribe();
  }
}

