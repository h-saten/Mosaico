import { Component, HostBinding, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormBase } from 'mosaico-base';
import { Stage } from 'mosaico-project';
import { DeploymentEstimate, OrdersService, OrderValidationResponse, Token } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { UserService } from 'src/app/modules/user-management/services';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
import { KangaPostPurchaseInfoComponent } from '../kanga-exchange/kanga-post-purchase-info/kanga-post-purchase-info.component';
import { MosaicoPaymentService } from '../payment.service';

@Component({
  selector: 'app-binance',
  templateUrl: './binance.component.html',
  styleUrls: ['./binance.component.scss']
})
export class BinanceComponent extends FormBase implements OnInit, OnDestroy, OnChanges{
  @HostBinding('class') classes = 'w-100';
  @Input() projectId;
  @Input() projectSlug;
  @Input() token: Token;
  @Input() stage: Stage;
  @Input() presetAmount: number;
  @Input() refCode: string;

  private subs: SubSink = new SubSink();
  private userId: string;

  public binanceRedirectUrl: string;
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
  @ViewChild('postPurchaseModal') postPurchaseModal: KangaPostPurchaseInfoComponent;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private store: Store,
    private toastr: ToastrService,
    private ordersService: OrdersService,
    private errorHandler: ErrorHandlingService,
    private router: Router,
    private translateService: TranslateService
  ) {
    super();
    this.paymentService = new MosaicoPaymentService('BINANCE', ordersService, userService);
  }

  async ngOnInit(): Promise<void> {
    this.currentUrl = this.router.url;
    this.createForm();
    this.subs.sink = this.store.select(selectUserInformation).subscribe((res) => {
      this.userId = res?.id;
      this.initPaymentServiceAsync();
    });
    this.form?.get('refCode')?.setValue(this.refCode);
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
      this.subs.sink = this.paymentService.onTokenAmountChanged.subscribe((tokenAmount) => {
        const tokenAmountControl = this.form?.get('tokenAmount');
        if (tokenAmount !== this.currentTokenAmount) {
          this.currentTokenAmount = tokenAmount;
          tokenAmountControl?.setValue(tokenAmount ?? 0);
        }
      });
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
      acceptAll: [false],
      paymentAmount: [0, [Validators.required]],
      phoneNumber: [null],
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
      this.subs.sink = this.ordersService.initBinance(this.projectId, {
        refCode,
        payedAmount: this.paymentService.paymentAmount,
        currency: this.paymentService.currency
      }).subscribe((res) => {
        this.toastr.success(this.translateService.instant('PROJECT_PURCHASE.TRANSACTION_INIT'));
        this.binanceRedirectUrl = res.data.checkoutUrl;
        this.postPurchaseModal?.open(this.binanceRedirectUrl);
        this.subs.sink = this.postPurchaseModal.closed.subscribe((res) => {
          if(res === true) {
            this.router.navigateByUrl(`/project/${this.projectSlug}`);
          }
        });
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        this.isDeploying$.next(false);
      });
    }
  }

}
