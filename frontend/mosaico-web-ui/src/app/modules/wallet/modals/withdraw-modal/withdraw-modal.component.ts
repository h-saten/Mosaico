import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { FormDialogBase, ErrorHandlingService, BlockchainService } from 'mosaico-base';
import { StakeCommand, StakingPair, WalletHubService, StakingService, WalletStake, SystemWallet, SystemWalletService, MosaicoRelayMilkyService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { SubSink } from 'subsink';
import { IStakingProcessor } from '../stake-modal/staking-processors/istaking-processor';
import { Metamaskv1StakingProcessor } from '../stake-modal/staking-processors/metamask.v1.staking-processor';
import { MilkyMetamaskStakingProcessor } from '../stake-modal/staking-processors/milky-metamask.staking-processor';
import { MosaicoStakingProcessor } from '../stake-modal/staking-processors/mosaico.staking-processor';

@Component({
  selector: 'app-withdraw-modal',
  templateUrl: './withdraw-modal.component.html',
  styleUrls: ['./withdraw-modal.component.scss']
})
export class WithdrawModalComponent extends FormDialogBase implements OnInit, OnDestroy {
  subs = new SubSink();
  isLoading = true;
  deploying$ = new BehaviorSubject<boolean>(false);
  showTransactionSucceeded = false;
  stake: WalletStake;
  dividendAbi: any;

  constructor(modalService: NgbModal, private errorHandler: ErrorHandlingService,
    private relayService: MosaicoRelayMilkyService,
    private mosaicoStaker: MosaicoStakingProcessor, private v1Staker: Metamaskv1StakingProcessor,
    private milkyStaker: MilkyMetamaskStakingProcessor,
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

  async submit(): Promise<void> {
    if (this.stake) {
      this.deploying$.next(true);
      const wallet = this.stake.walletType;

      let staker: IStakingProcessor;
      if (wallet === 'MOSAICO_WALLET') {
        staker = this.mosaicoStaker;
      }
      else if (wallet === 'METAMASK' && this.stake && this.stake.version === 'milky_v1') {
        staker = this.milkyStaker;
      }
      else if(wallet === 'METAMASK' && this.stake && this.stake.version === 'v1'){
        staker = this.v1Staker;
      }
      else {
        this.toastr.error('Unsupported payment method');
        this.deploying$.next(false);
      }

      try{
        const withdrawRequest = await staker.withdraw(this.stake);
      }
      catch(e) {
        this.deploying$.next(false);
      }
    }
  }

  open(payload?: WalletStake) {
    this.resetState();
    this.stake = payload;
    this.hub.startConnection();
    this.hub.addStakeWithdrawalListeners();
    super.open(payload);
    setTimeout(() => this.isLoading = false, 1000);
    this.modalRef.closed.subscribe((res) => {
      this.hub.removeListener();
    });
    this.subs.sink = this.hub.stakeWithdrawalSucceeded$.subscribe((res) => {
      if (res) {
        this.showSuccessMessage();
        this.showTransactionSucceeded = true;
        this.deploying$.next(false);
      }
    });
    this.subs.sink = this.hub.stakeWithdrawalFailed$.subscribe((res) => {
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
