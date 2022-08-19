import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { BlockchainService, ErrorHandlingService, FormDialogBase } from 'mosaico-base';
import { DividendStakingService, MosaicoRelayMilkyService, StakeCommand, StakingPair, StakingService, SystemWallet, SystemWalletService, WalletHubService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
import { IStakingProcessor } from './staking-processors/istaking-processor';
import { MosaicoStakingProcessor } from './staking-processors/mosaico.staking-processor';
import { Metamaskv1StakingProcessor } from './staking-processors/metamask.v1.staking-processor';
import { MilkyMetamaskStakingProcessor } from './staking-processors/milky-metamask.staking-processor';

@Component({
  selector: 'app-stake-modal',
  templateUrl: './stake-modal.component.html',
  styleUrls: ['./stake-modal.component.scss']
})
export class StakeModalComponent extends FormDialogBase implements OnInit, OnDestroy {
  subs = new SubSink();
  isLoading = true;
  deploying$ = new BehaviorSubject<boolean>(false);
  showTransactionSucceeded = false;
  command: StakeCommand;
  userId: string;
  network = 'Polygon';
  stakingPair: StakingPair;
  @Input() stakingPairs: StakingPair[] = [];
  systemWallets: SystemWallet[] = [];
  dividendAbi: any;
  tokenAbi: any;
  whitelistedMetamaskVersions = ['milky_v1', 'v1'];
  showMetamask: boolean = false;
  regulation = '';
  termsAndConditionsUrl = '';

  constructor(modalService: NgbModal, private store: Store,
    private mosaicoStaker: MosaicoStakingProcessor, private v1Staker: Metamaskv1StakingProcessor, private translateService: TranslateService,
    private milkyStaker: MilkyMetamaskStakingProcessor, private errorHandler: ErrorHandlingService,
    private toastr: ToastrService, private hub: WalletHubService, private systemWalletService: SystemWalletService) {
    super(modalService);
    this.extraOptions = {
      modalDialogClass: "mosaico-staking-modal"
    };
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.systemWalletService.getSystemWallets().subscribe((wallets) => {
      this.systemWallets = wallets?.data;
      const metamask = this.systemWallets.find((sw) => sw.key === 'METAMASK');
      if (metamask) {
        metamask.disabled = false;
      }
    });
    this.form = new FormGroup({
      wallet: new FormControl('MOSAICO_WALLET', [Validators.required]),
      accepted: new FormControl(false, [Validators.requiredTrue])
    });

  }

  async submit(): Promise<void> {
    if (this.command) {
      this.deploying$.next(true);
      const wallet = this.form.get('wallet')?.value;
      let staker: IStakingProcessor;
      if (wallet === 'MOSAICO_WALLET') {
        staker = this.mosaicoStaker;
      }
      else if (wallet === 'METAMASK' && this.stakingPair && this.stakingPair.version === 'milky_v1') {
        staker = this.milkyStaker;
      }
      else if (wallet === 'METAMASK' && this.stakingPair && this.stakingPair.version === 'v1') {
        staker = this.v1Staker;
      }
      else {
        this.toastr.error('Unsupported payment method');
        this.deploying$.next(false);
      }

      try {
        const approvalResponse = await staker.approve(this.stakingPair, this.command.balance, this.command.days);
        const stakeResponse = await staker.stake(this.stakingPair, this.command.balance, this.command.days);
      }
      catch (e) {
        this.errorHandler.handleErrorWithToastr(e);
        this.deploying$.next(false);
      }
    }
  }

  private setStakingRegulation(): void {
    if(this.stakingPair && this.stakingPair.stakingRegulation && this.stakingPair.stakingRegulation.length > 0) {
      this.regulation = this.stakingPair.stakingRegulation;
    }
    else {
      this.subs.sink = this.translateService.get('WALLET_STAKING.DISCLAIMER.STANDARD.FULL_INFO').subscribe((r) => {
        this.regulation = r;
      });
    }
  }

  private setRegulationUrl(): void {
    if(this.stakingPair && this.stakingPair.termsAndConditionsUrl && this.stakingPair.termsAndConditionsUrl.length > 0) {
      this.termsAndConditionsUrl = this.stakingPair.termsAndConditionsUrl;
    }
    else {
      this.termsAndConditionsUrl = null;
    }
  }

  private resetForm(): void {
    this.form?.get('wallet')?.setValue('MOSAICO_WALLET');
    this.form?.get('accepted')?.setValue(false);
  }

  open(payload?: StakeCommand) {
    this.resetState();
    this.command = payload;
    if (this.command && this.command.stakingPairId) {
      this.stakingPair = this.stakingPairs?.find((s) => s.id === this.command.stakingPairId);
      if (this.stakingPair) {
        this.setStakingRegulation();
        this.setRegulationUrl();
        if (this.whitelistedMetamaskVersions.includes(this.stakingPair.version)) {
          this.showMetamask = true;
        }
        else {
          this.showMetamask = false;
        }
      }
    }
    this.hub.startConnection();
    this.hub.addStakeListeners();
    super.open(payload);
    setTimeout(() => this.isLoading = false, 1000);
    this.modalRef.closed.subscribe((res) => {
      this.hub.removeListener();
    });
    this.subs.sink = this.store.select(selectUserInformation).subscribe((res) => {
      this.userId = res?.id;
    });
    this.subs.sink = this.hub.stakeSucceeded$.subscribe((res) => {
      if (res) {
        this.showSuccessMessage();
        this.showTransactionSucceeded = true;
        this.deploying$.next(false);
      }
    });
    this.subs.sink = this.hub.stakeFailed$.subscribe((res) => {
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

  getStakeAssetSymbol(): string {
    return this.stakingPair?.type === 'Token' ? this.stakingPair?.stakingToken?.symbol : this.stakingPair?.stakingPaymentCurrency?.ticker;
  }

  private resetState(): void {
    this.isLoading = true;
    this.hub.resetObjects();
    this.deploying$ = new BehaviorSubject<boolean>(false);
    this.showTransactionSucceeded = false;
    this.subs.unsubscribe();
    this.stakingPair = null;
    this.resetForm();
  }

}
