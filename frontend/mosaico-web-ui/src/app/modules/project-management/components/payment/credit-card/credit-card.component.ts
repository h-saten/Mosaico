import { AfterViewInit, Component, HostBinding, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { BlockchainNetworkType, ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';
import { CrowdsaleHubService, PaymentCurrency, Stage } from 'mosaico-project';
import { ExchangeRate, ExchangeRateService, InitiateTransactionCommand, OrdersService, RampUserKYC, Token, TokenBalance, TransakUserKYC, WalletService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { setWalletInfo, setWalletBalance } from 'src/app/modules/wallet';
import { SubSink } from 'subsink';
import { selectCompanyWallet } from '../../../store/project.selectors';

@Component({
  selector: 'app-credit-card',
  templateUrl: './credit-card.component.html',
  styleUrls: ['./credit-card.component.scss']
})
export class CreditCardComponent extends FormBase implements OnInit, OnDestroy, OnChanges, AfterViewInit  {

  @HostBinding('class') classes = 'w-100';
  @Input() projectId;
  @Input() token: Token;
  @Input() stage: Stage;
  @Input() projectSlug;
  @Input() presetAmount: number;
  @Input() refCode: string;

  subs: SubSink = new SubSink();
  tokenAmount = 10;
  tokenPrice = 0;
  currentPaymentAmount = 0;
  tokensPurchaseMinLimit = 0;
  tokensPurchaseMaxLimit = 0;
  isLoading$ = new BehaviorSubject<boolean>(true);
  userId: string;
  isDeploying$ = new BehaviorSubject<boolean>(false);
  transakUserKyc: TransakUserKYC;
  rampUserKyc: RampUserKYC;
  selectedNetwork: BlockchainNetworkType;
  walletAddress: string;
  transactionWasInitiated = false;
  paymentCurrencies = [ "EUR", "USD", "PLN" ];
  selectedCurrency = "USD";
  rates: ExchangeRate[] = [];
  currentTransactionCorrelationId: string;
  companyName: string;
  projectName: string;
  policyUrl: string;
  regulationUrl: string;
  isPurchaseInvalid = false;
  minimumPurchase: number;
  maximumPurchase: number;
  showRefCode = false;
  constructor(
    private formBuilder: FormBuilder,
    private store: Store,
    private toastr: ToastrService,
    private ordersService: OrdersService,
    private errorHandler: ErrorHandlingService,
    private hub: CrowdsaleHubService,
    private rateService: ExchangeRateService,
    private translateService: TranslateService,
    private router: Router
  ) {
    super();
    this.createForm();
  }

  ngAfterViewInit(): void {
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectUserInformation).subscribe((res) => {
      this.userId = res?.id;
      if(res) {
        this.transakUserKyc = {
          email: res.email,
          userData: {
            email: res.email,
            firstName: res.firstName,
            lastName: res.lastName,
            dob: res.dob ? new Date(res.dob)?.toDateString() : null,
            address: {
              city: res.city,
              countryCode: res.country,
              addressLine1: res.street
            },
            mobileNumber: res.phoneNumber
          }
        };
        this.rampUserKyc = {
          email: res.email
        };
      }
    });

    this.subs.sink = this.ordersService.getQuote(this.projectId, "CREDIT_CARD").subscribe((res) => {
      this.rates = res?.data?.exchangeRates;
      this.walletAddress = res.data?.paymentAddress;
      this.companyName = res.data.companyName;
      this.projectName = res.data.projectName;
      this.policyUrl = res.data.privacyPolicyUrl;
      this.regulationUrl = res.data.regulationsUrl;
      this.setInitialTokensAmountValue(this.presetAmount > 0 && this.presetAmount > res?.data?.minimumPurchase ? this.presetAmount : res?.data?.minimumPurchase);
      this.updateMinMaxValidators(res?.data?.minimumPurchase, res?.data?.maximumPurchase);
      this.minimumPurchase = res?.data?.minimumPurchase;
      this.maximumPurchase = res?.data?.maximumPurchase;
      this.isLoading$.next(false);
      this.recalculateTokenPrice();
      this.recalculatePaymentAmount();
    });

    this.subs.sink = this.store.select(selectCompanyWallet).subscribe((res) => {
      if(res && res.walletAddress?.length > 0) {
        this.selectedNetwork = <BlockchainNetworkType>res.network;
      }
    });
    this.setInitialTokensAmountValue(this.presetAmount > 0 && this.presetAmount > this.stage?.minimumPurchase ? this.presetAmount : this.stage?.minimumPurchase);
    this.subs.sink = this.isDeploying$.subscribe((res) => {
      if(res === true) {
        this.form.disable();
      }
      else {
        this.form.enable();
      }
    });
    this.form?.get('refCode')?.setValue(this.refCode);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.stage && changes.stage.currentValue) {
      this.tokenPrice = this.stage.tokenPrice;
      this.recalculateTokenPrice();
      this.recalculatePaymentAmount();
    }
    if(changes.refCode && changes.refCode.currentValue) {
      this.form?.get('refCode')?.setValue(this.refCode);
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  
  private setInitialTokensAmountValue(value: number): void {
    if (this.form) {
      this.tokenAmount = value;
      this.form.controls.tokenAmount.setValue(value);
      this.recalculatePaymentAmount();
    }
  }

  private updateMinMaxValidators(min: number, max: number): void {
    if (this.form) {
      this.tokensPurchaseMinLimit = min;
      this.tokensPurchaseMaxLimit = max;
      this.form.controls.tokenAmount.addValidators([
        Validators.required,
        Validators.min(min),
        Validators.max(max)
      ]);
      this.form.updateValueAndValidity();
    }
  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      tokenAmount: [this.tokenAmount, [Validators.required]],
      regulationsAccepted: [false, [Validators.requiredTrue]],
      marketingAccepted: [false, [Validators.requiredTrue]],
      acceptAll: [false],
      refCode: [null, [Validators.minLength(0), Validators.maxLength(15)]]
    });
    this.form.controls.acceptAll.valueChanges.subscribe((newValue) => {
      if (newValue === true) {
        this.form.controls.regulationsAccepted.setValue(true);
        this.form.controls.marketingAccepted.setValue(true);
      }
      else {
        this.form.controls.regulationsAccepted.setValue(false);
        this.form.controls.marketingAccepted.setValue(false);
      }
    });
    this.form.controls.tokenAmount.valueChanges.subscribe((newValue) => {
      this.recalculatePaymentAmount();
      this.tokenAmount = newValue;
    });
  }

  private recalculatePaymentAmount(): void {
    if (this.form?.controls.tokenAmount.value) {
      this.currentPaymentAmount = this.tokenPrice * this.form.controls.tokenAmount.value;
    }
  }
  
  currencyChanged(symbol: string): void {
    this.selectedCurrency = symbol;
    this.recalculateTokenPrice();
    this.recalculatePaymentAmount();
  }
  
  widgetClosed(e: any): void {
    if(this.transactionWasInitiated === true && this.currentTransactionCorrelationId?.length > 0) {
      this.subs.sink = this.ordersService.getStatus(this.currentTransactionCorrelationId).subscribe((res) => {
          if(res?.data?.status === 'IN_PROGRESS') {
            this.toastr.success('Transaction is processing. It may take up to 10 minutes for tokens to appear in your wallet, if payment succeeds.');
            this.router.navigateByUrl(`/project/${this.projectSlug}/orderConfirmation`);
          }
      });
    }
    this.currentTransactionCorrelationId = null;
    this.isDeploying$.next(false);
  }

  rampTransactionConfirmed(order: any): void {
    if(this.isDeploying$.value === true) return;
    this.isDeploying$.next(true);
    let refCode = this.form.get('refCode')?.value;
    if(!refCode || refCode.length === 0){
      refCode = this.refCode;
    }
    this.subs.sink = this.ordersService
      .initRampOrder(this.projectId, {...order?.payload?.purchase, tokenAmount: this.tokenAmount, secret: order?.payload?.purchaseViewToken, refCode})
      .subscribe((res) => {
        this.currentTransactionCorrelationId = res.data;
        localStorage.removeItem('refCode');
        this.transactionWasInitiated = true;
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        this.isDeploying$.next(false);
      });
  }

  transakTransactionConfirmed(order: any) : void {
    if(this.isDeploying$.value === true) return;

    this.isDeploying$.next(true);
    let refCode = this.form.get('refCode')?.value;
    if(!refCode || refCode.length === 0){
      refCode = this.refCode;
    }
    this.subs.sink = this.ordersService
      .initTransakOrder(this.projectId, {...order?.status, tokenAmount: this.tokenAmount, refCode})
      .subscribe((res) => {
        this.currentTransactionCorrelationId = res.data;
        localStorage.removeItem('refCode');
        this.transactionWasInitiated = true;
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        this.isDeploying$.next(false);
      });
  }

  recalculateTokenPrice(): void {
    const exchangeRate = this.rates.find((r) => r.currency === this.selectedCurrency);
    if(exchangeRate && this.stage) {
      this.tokenPrice = this.stage.tokenPrice / exchangeRate.exchangeRate;
    }
  }

  initTransaction(callback: () => void): void {
    this.validateOrder(async (canPurchase) => {
      if(callback && canPurchase === true) {
        callback();
      }
      else {
        this.isDeploying$.next(false);
      }
    });
  }

  private validateOrder(callback?: (canPurchase: boolean) => Promise<void>): void {
    this.subs.sink = this.ordersService.validate(this.projectId, { tokenAmount: this.tokenAmount, currency: this.selectedCurrency, payedAmount: this.currentPaymentAmount, paymentMethod: 'CREDIT_CARD' }).subscribe((res) => {
      if (callback) {
        callback(res?.data?.status === 'OK');
      }
      if (res?.data?.status !== 'OK') {
        this.isPurchaseInvalid = true;
        this.subs.sink = this.translateService.get(`CHECKOUT.${res?.data?.status}`).subscribe((t) => {
          this.toastr.error(t);
        });
      }
    });
  }

}
