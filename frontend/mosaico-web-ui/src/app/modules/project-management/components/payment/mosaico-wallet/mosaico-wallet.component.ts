import { AfterViewInit, Component, HostBinding, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorHandlingService, FormBase } from "mosaico-base";
import { SubSink } from "subsink";
import { CrowdsaleHubService, Stage } from "mosaico-project";
import { FormBuilder, Validators } from "@angular/forms";
import { ActiveBlockchainService, CCQuoteResponse, DeploymentEstimate, ExchangeRate, OrdersService, OrderValidationResponse, PaymentCurrency, Token, TokenBalance } from "mosaico-wallet";
import { Store } from "@ngrx/store";
import { selectUserInformation } from "../../../../user-management/store";
import { ToastrService } from "ngx-toastr";
import { BehaviorSubject, zip } from 'rxjs';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { UserService } from 'src/app/modules/user-management/services';
import { MosaicoPaymentService } from '../payment.service';

@Component({
  selector: 'app-mosaico-wallet',
  templateUrl: './mosaico-wallet.component.html',
  styleUrls: ['./mosaico-wallet.component.scss']
})
export class MosaicoWalletComponent extends FormBase implements OnInit, OnDestroy, OnChanges {
  @HostBinding('class') classes = 'w-100';
  @Input() projectId;
  @Input() projectSlug;
  @Input() token: Token;
  @Input() stage: Stage;
  @Input() presetAmount: number;
  @Input() refCode: string;

  private subs: SubSink = new SubSink();
  private userId: string;
  private transactionWasInitiated = false;

  public isDeploying$ = new BehaviorSubject<boolean>(false);
  public currentUrl: string;
  public estimates: DeploymentEstimate[] = [];
  public currentEstimate: DeploymentEstimate;
  public paymentService: MosaicoPaymentService;
  private latestValidationResult: OrderValidationResponse;
  public isPhoneNumberInvalid = false;
  public showPhoneVerification = false;
  showRefCode = false;
  private currentPaymentValue: number;
  private currentTokenAmount: number;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private userService: UserService,
    private store: Store,
    private toastr: ToastrService,
    private ordersService: OrdersService,
    private errorHandler: ErrorHandlingService,
    private hub: CrowdsaleHubService,
    private router: Router,
    private activeBlockchain: ActiveBlockchainService,
    private translateService: TranslateService
  ) {
    super();
    this.paymentService = new MosaicoPaymentService('MOSAICO_WALLET', ordersService, userService);
  }

  async ngOnInit(): Promise<void> {
    this.currentUrl = this.router.url;
    this.createForm();
    this.subs.sink = this.store.select(selectUserInformation).subscribe((res) => {
      this.userId = res?.id;
      this.initPaymentServiceAsync();
    });
    await this.initHubConnection();
    await this.getBlockchainEstimates();
    this.form?.get('refCode')?.setValue(this.refCode);

    const tokenAmountControl = this.form?.get('tokenAmount');
    this.route.queryParams.subscribe(data => {
      tokenAmountControl.setValue(data.amount)
    })
  }

  setMaxPaymentValue(): void {
    const balance = this.paymentService.currentBalance;
    this.paymentService.paymentAmount = balance;
  }

  async initPaymentServiceAsync(): Promise<void> {
    if (!this.paymentService.isLoading && !this.paymentService.isDataLoaded) {
      await this.paymentService.initAsync({
        presetTokenAmount: this.presetAmount,
        projectId: this.projectId,
        stage: this.stage,
        userId: this.userId
      });
      this.subs.sink = this.paymentService.onCurrencyChanged.subscribe((newCurrency) => {
        this.refreshEstimate();
        this.updateMinMaxValidators();
      });
      this.subs.sink = this.paymentService.onValidationFailed.subscribe((res) => {
        if (res) {
          this.subs.sink = this.translateService.get(`CHECKOUT.${res?.status}`).subscribe((t) => {
            this.toastr.error(t);
          });
        }
      });
      this.updateMinMaxValidators();
      // this.subs.sink = this.paymentService.onTokenAmountChanged.subscribe((tokenAmount) => {
      //   const tokenAmountControl = this.form?.get('tokenAmount');
      //   if (tokenAmount !== this.currentTokenAmount) {
      //     this.currentTokenAmount = tokenAmount;
      //     tokenAmountControl?.setValue(tokenAmount ?? 0);
      //   }
      // });
      this.subs.sink = this.paymentService.onPaymentAmountChanged.subscribe((paymentAmount) => {
        const paymentAmountControl = this.form?.get('paymentAmount');
        if (paymentAmount !== this.currentPaymentValue) {
          this.currentPaymentValue = paymentAmount;
          paymentAmountControl?.setValue(paymentAmount ?? 0);
        }
      });
      this.form.get('tokenAmount').valueChanges.subscribe((newTokenAmount) => {
        this.paymentService.tokenAmount = +newTokenAmount;
      });
      this.form.get('paymentAmount').valueChanges.subscribe((newPaymentAmount) => {
        this.paymentService.paymentAmount = +newPaymentAmount;
      });
    }
  }

  private async initHubConnection(): Promise<void> {
    this.hub.startConnection();
    this.hub.addPurchaseListener();
    this.subs.sink = this.hub.purchaseFailed$.subscribe((res) => {
      if (res) {
        this.toastr.error(res);
        this.isDeploying$.next(false);
      }
    });
    this.subs.sink = this.hub.purchaseSuccessful$.subscribe((res) => {
      if (res && this.transactionWasInitiated === true) {
        this.isDeploying$.next(false);
        this.router.navigateByUrl(`/project/${this.projectSlug}/orderConfirmation?tx=${res}&network=${this.token.network}`);
      }
    });
    this.subs.sink = this.isDeploying$.subscribe((res) => {
      if (res === true) {
        this.form.disable();
      }
      else {
        this.form.enable();
      }
    });
  }

  private async getBlockchainEstimates(): Promise<void> {
    try {
      const response = await this.activeBlockchain.getEstimates().toPromise();
      this.estimates = response?.data;
      this.refreshEstimate();
    }
    catch (error) {
      this.errorHandler.handleErrorWithToastr(error);
    }
  }

  private refreshEstimate(): void {
    this.currentEstimate = this.paymentService.currency === 'MATIC' ?
      this.estimates?.find((e) => e.paymentMethod === 'MOSAICO_WALLET' && e.network === this.token?.network && e.contractVersion === 'native_transfer') :
      this.estimates?.find((e) => e.paymentMethod === 'MOSAICO_WALLET' && e.network === this.token?.network && e.contractVersion === 'erc20_v1_transfer');
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.stage && changes.stage.currentValue) {
      this.paymentService.settings = {
        presetTokenAmount: this.presetAmount,
        projectId: this.projectId,
        stage: this.stage,
        userId: this.userId
      };
      this.updateMinMaxValidators();
    }
    if(changes.refCode && changes.refCode.currentValue) {
      this.form?.get('refCode')?.setValue(this.refCode);
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    this.hub.removeListener();
    this.paymentService.destroy();
  }

  private updateMinMaxValidators(): void {
    if (this.form) {
      this.form.controls.tokenAmount.addValidators([
        Validators.required,
        Validators.min(this.paymentService.minimumPurchase),
        Validators.max(this.paymentService.maximumPurchase)
      ]);
      this.form.updateValueAndValidity();
    }
  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      tokenAmount: [0, [Validators.required]],
      regulationsAccepted: [false, [Validators.requiredTrue]],
      marketingAccepted: [false, [Validators.requiredTrue]],
      personalDataAccepted: [false],
      acceptAll: [false],
      paymentAmount: [0, [Validators.required]],
      phoneNumber: [null],
      refCode: [null, [Validators.minLength(0), Validators.maxLength(15)]]
    });
    this.form.controls.acceptAll.valueChanges.subscribe((newValue) => {
      if (newValue === true) {
        this.form.controls.regulationsAccepted.setValue(true);
        this.form.controls.marketingAccepted.setValue(true);
        this.form.controls.personalDataAccepted.setValue(true);
      }
      else {
        this.form.controls.regulationsAccepted.setValue(false);
        this.form.controls.marketingAccepted.setValue(false);
        this.form.controls.personalDataAccepted.setValue(false);
      }
    });
  }

  currencyChanged(symbol: string): void {
    this.paymentService.currency = symbol;
  }

  async initiatePurchase(): Promise<void> {
    if (this.isDeploying$.value === false) {
      this.isDeploying$.next(true);
      this.subs.sink = this.paymentService.onValidationSucceeded.subscribe((res) => {
        this.latestValidationResult = res;
      });
      const canPurchase = await this.paymentService.validateAsync();
      if (canPurchase === true) {
        if (this.latestValidationResult?.isPhoneNumberRequired === true) {
          this.showPhoneVerification = true;
          this.form.get('phoneNumber')?.enable();
        }
        else {
          this.createTransaction();
        }
      }
      else {
        this.isDeploying$.next(false);
      }
    }
  }

  public async verifyPhone(): Promise<void> {
    if(this.showPhoneVerification === true) {
      const phone = this.form.get('phoneNumber')?.value;
      if(phone && phone.length > 0) {
        this.form.get('phoneNumber')?.disable();
        this.isPhoneNumberInvalid = false;
        this.subs.sink = this.userService.updatePhoneNumber({
          phoneNumber: phone
        }).subscribe((res) => {
          this.subs.sink = this.translateService.get('CHECKOUT.PHONE_UPDATED').subscribe((t) => {
            this.toastr.success(t);
          });
          this.showPhoneVerification = false;
          this.createTransaction();
          this.form.get('phoneNumber')?.setValue(null);
        }, (error) => {
          this.errorHandler.handleErrorWithToastr(error);
        });
      }
      else {
        this.isPhoneNumberInvalid = true;
        this.subs.sink = this.translateService.get('CHECKOUT.INVALID_PHONE_NUMBER').subscribe((t) => {
          this.toastr.error(t);
        });
      }
    }
  }

  private createTransaction(): void {
    if (this.showPhoneVerification === false) {
      let refCode = this.form.get('refCode')?.value;
      if(!refCode || refCode.length === 0){
        refCode = this.refCode;
      }
      this.subs.sink = this.ordersService.initMosaicoWallet(this.projectId, {
        refCode,
        payedAmount: this.paymentService.paymentAmount,
        currency: this.paymentService.currency
      }).subscribe((res) => {
        this.toastr.success(this.translateService.instant('PROJECT_PURCHASE.TRANSACTION_INIT'));
        this.transactionWasInitiated = true;
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        this.isDeploying$.next(false);
      });
    }
  }
}
