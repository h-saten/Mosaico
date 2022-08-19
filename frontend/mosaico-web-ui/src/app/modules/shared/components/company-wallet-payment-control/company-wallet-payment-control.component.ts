import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { ControlValueAccessor, FormControl, FormGroup, NG_VALUE_ACCESSOR, Validators } from '@angular/forms';
import { FormBase } from 'mosaico-base';
import { ActiveBlockchainService, CompanyNativeBalanceResponse, CompanyWalletService, DeploymentEstimate, DeploymentEstimateHubService, SystemWallet, SystemWalletService } from 'mosaico-wallet';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-company-wallet-payment-control',
  templateUrl: './company-wallet-payment-control.component.html',
  styleUrls: ['./company-wallet-payment-control.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: CompanyWalletPaymentControlComponent
    }
  ]
})
export class CompanyWalletPaymentControlComponent extends FormBase implements OnInit, OnDestroy, OnChanges, ControlValueAccessor {
  systemWallets: SystemWallet[] = [];
  estimate: DeploymentEstimate;
  estimates: DeploymentEstimate[] = [];
  sub = new SubSink();
  @Input() contractName = "erc20_v1";
  @Input() network = "Ethereum";
  @Input() companyId: string;
  isBalanceLoaded = false;
  currentBalance: CompanyNativeBalanceResponse;
  walletControl = new FormControl('MOSAICO_WALLET', [Validators.required]);
  timer: NodeJS.Timer;
  isLoading$ = new BehaviorSubject<boolean>(false);

  onChange = (type) => { };
  onTouched = () => { };

  constructor(private systemWalletService: SystemWalletService, private activeBlockchain: ActiveBlockchainService, private estimateHub: DeploymentEstimateHubService,
    private walletService: CompanyWalletService) {
    super();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.refreshEstimate();
    if(changes.network) {
      this.isBalanceLoaded = false;
      this.getCompanyBalance();
    }
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
    this.estimateHub?.removeListener();
    if(this.timer){
      clearInterval(this.timer);
    }
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      wallet: this.walletControl
    });
    this.sub.sink = this.walletControl.valueChanges.subscribe((newVal) => {
      this.refreshEstimate();
    });
    this.sub.sink = this.systemWalletService.getSystemWallets().subscribe((wallets) => {
      this.systemWallets = wallets?.data;
    });
    this.sub.sink = this.activeBlockchain.getEstimates().subscribe((res) => {
      if (res?.data && res?.data.length > 0) {
        this.estimates = res.data;
        this.refreshEstimate();
      }
    });
    this.sub.sink = this.isLoading$.subscribe((newVal) => {
    });

    this.estimateHub.startConnection();
    this.estimateHub.addListener();
    this.sub.sink = this.estimateHub.estimates$.subscribe((newEstimates) => {
      if (newEstimates && newEstimates.length > 0) {
        this.estimates = newEstimates;
        this.refreshEstimate();
      }
    });

    this.timer = setInterval(() => {
      if(this.companyId?.length > 0 && this.network?.length > 0) {
        this.getCompanyBalance();
      }
    }, 2000);
  }

  private getCompanyBalance(): void {
    if (!this.isBalanceLoaded) {
      this.isLoading$.next(true);
      this.sub.sink = this.walletService.getCompanyBalance(this.companyId).subscribe((res) => {
        this.currentBalance = res?.data;
        this.isBalanceLoaded = true;
        this.isLoading$.next(false);
      }, (error) => { this.isBalanceLoaded = false; this.isLoading$.next(false); });
    }
  }

  public refreshEstimate(): void {
    const paymentMethod = this.form?.get('wallet')?.value;
    if (this.network?.length > 0 && paymentMethod?.length > 0) {
      this.estimate = this.estimates?.find((e) => e.contractVersion === this.contractName && e.network === this.network && e.paymentMethod === paymentMethod);
    }
  }

  writeValue(type: string): void {
    this.walletControl.setValue(type);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    if (isDisabled) {
      this.form.disable();
    }
    else {
      this.form.enable();
    }
  }

}
