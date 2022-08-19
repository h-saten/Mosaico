import { Component, HostBinding, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { BlockchainNetworkType, BlockchainService, ErrorHandlingService, FormBase } from 'mosaico-base';
import { Stage } from 'mosaico-project';
import { Blockchain, CCQuoteResponse, DeploymentEstimate, ExchangeRate, OrdersService, OrderValidationResponse, PaymentCurrency, StablecoinSevice, Token } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, zip } from 'rxjs';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
import { selectCompanyWallet } from '../../../store/project.selectors';
import { Router } from '@angular/router';
import { selectCurrentActiveBlockchains } from '../../../../../store/selectors';
import { UserService } from '../../../../user-management/services/user.service';
import { TranslateService } from '@ngx-translate/core';
import { MosaicoPaymentService } from '../payment.service';

@Component({
  selector: 'app-metamask-wallet',
  templateUrl: './metamask-wallet.component.html',
  styleUrls: ['./metamask-wallet.component.scss']
})
export class MetamaskWalletComponent extends FormBase implements OnInit, OnChanges {
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
  selectedNetwork: BlockchainNetworkType;
  walletAddress: string;
  stableCoins: string[];
  nativeCurrency: string[];
  transactionWasInitiated: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private store: Store,
    private toastr: ToastrService,
    private ordersService: OrdersService,
    private errorHandler: ErrorHandlingService,
    private router: Router,
    private translateService: TranslateService,
    private blockchainService: BlockchainService,
    private stablecoin: StablecoinSevice
  ) {
    super();
    this.paymentService = new MosaicoPaymentService('METAMASK', ordersService, userService);
  }

  ngAfterViewInit(): void {
  }

  async ngOnInit(): Promise<void> {
    this.currentUrl = this.router.url;
    this.createForm();
    this.subs.sink = this.store.select(selectUserInformation).subscribe((res) => {
      this.userId = res?.id;
      this.initPaymentServiceAsync();
    });
    this.form?.get('refCode')?.setValue(this.refCode);
    this.subs.sink = this.store.select(selectCompanyWallet).subscribe((res) => {
      if (res && res.walletAddress?.length > 0) {
        this.selectedNetwork = <BlockchainNetworkType>res.network;
      }
    });
  }

  async initPaymentServiceAsync(): Promise<void> {
    if (!this.paymentService.isLoading && !this.paymentService.isDataLoaded) {
      this.paymentService.onQuoteChanged.subscribe((q) => {
        if(q){
          this.walletAddress = q.paymentAddress;
          this.stableCoins = q.currencies.filter((c) => !c.nativeChainCurrency).map((c) => c.ticker);
          this.nativeCurrency = q.currencies.filter((c) => c.nativeChainCurrency).map((c) => c.ticker);
        }
      });
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
          this.initTransaction();
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

  private async trySwitchNetworkAsync(targetChain: string): Promise<void> {
    try {
      const web3 = await this.blockchainService.getWeb3();
      const provider: any = web3.currentProvider;
      const chain = web3.utils.toHex(+targetChain);
      await provider.request({
        method: "wallet_switchEthereumChain",
        params: [{ chainId: chain }]
      });
    }
    catch (error) {
      this.toastr.error(error);
    }
  }

  private async verifyNetworkAsync(selectedChainId: string, callback?: (network: Blockchain) => Promise<void>): Promise<void> {
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((res) => {
      const selectedNetwork = res?.find((c) => c.chainId === selectedChainId);
      const desiredNetwork = res?.find((c) => c.name === this.selectedNetwork);
      if (!selectedNetwork || !desiredNetwork || desiredNetwork.chainId !== selectedNetwork.chainId) {
        this.toastr.error(`Unsupported network is selected in metamask. Please, switch to ${this.selectedNetwork}`);
        this.trySwitchNetworkAsync(desiredNetwork.chainId);
        this.isDeploying$.next(false);
        return;
      }
      else {
        if (callback) {
          callback(selectedNetwork);
        }
      }
    });
  }

  async initTransaction(): Promise<void> {
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
          const currentWallet = await this.blockchainService.authenticateToMetamask((error) => {
            if (error) {
              this.errorHandler.handleErrorWithToastr(error);
              this.isDeploying$.next(false);
            }
          });
          if (!currentWallet || currentWallet.length === 0) {
            this.isDeploying$.next(false);
            return;
          }
          const chainId = await this.blockchainService.getChainId();
          await this.verifyNetworkAsync(chainId?.toString(), async (selectedNetwork: Blockchain) => {
            const currentWallet = await this.blockchainService.getCurrentWallet();
            if (this.stableCoins.includes(this.paymentService.currency)) {
              await this.handleStableCoinAsync(currentWallet, this.walletAddress, this.paymentService.paymentAmount);
            }
            else if (this.nativeCurrency.includes(this.paymentService.currency)) {
              await this.handleNativeAsync(currentWallet, this.walletAddress, this.paymentService.paymentAmount?.toString());
            }
          });
        }
      }
      else {
        this.isDeploying$.next(false);
      }
    }
  }

  private onMetamaskError(error) {
    this.errorHandler.handleErrorWithToastr(error);
    this.isDeploying$.next(false);
  }

  private async handleStableCoinAsync(owner: string, recipient: string, amount: number): Promise<void> {
    const abi = this.stablecoin.getAbi(this.selectedNetwork, this.paymentService.currency);
    if (!abi) {
      this.toastr.error('Unsupported stablecoin');
      this.isDeploying$.next(false);
      return;
    }
    const web3 = await this.blockchainService.getWeb3();
    const decimal = 6;
    const paymentCurrency = this.paymentService.paymentCurrencies.find((c) => c.ticker === this.paymentService.currency);
    if (!paymentCurrency) {
      this.toastr.error(`Unsupport payment currency ${this.paymentService.currency}.`);
      this.isDeploying$.next(false);
      return;
    }
    const amountToSend = Math.ceil(amount * Math.pow(10, decimal));
    const contract = new web3.eth.Contract(abi, paymentCurrency.contractAddress);
    let value = web3.utils.toBN(amountToSend);
    try{
      var requiredGas = await contract.methods.transfer(recipient, value).estimateGas({ from: owner });
    }
    catch(e) {
      this.onMetamaskError(e);
      throw e;
    }
    const gasPrice = await web3.eth.getGasPrice();
    await contract.methods.transfer(recipient, value).send({ from: owner, gas: requiredGas, gasPrice }).on('receipt', this.transactionCompleted.bind(this))
      .on('error', this.onMetamaskError.bind(this))
      .on('transactionHash', (hash) => {
        this.createTransaction(hash);
      });
  }

  private async handleNativeAsync(owner: string, recipient: string, amount: string): Promise<void> {
    const web3 = await this.blockchainService.getWeb3();
    const amountToSend = web3.utils.toWei(amount);
    const gasPrice = await web3.eth.getGasPrice();
    const value: any = web3.utils.toBN(amountToSend);
    await web3.eth.sendTransaction({ to: recipient, from: owner, gasPrice, value }).on('receipt', this.transactionCompleted.bind(this))
      .on('error', this.onMetamaskError.bind(this))
      .on('transactionHash', (hash) => {
        this.createTransaction(hash);
      });
  }

  private createTransaction(hash: string): void {
    let refCode = this.form.get('refCode')?.value;
    if(!refCode || refCode.length === 0){
      refCode = this.refCode;
    }
    this.subs.sink = this.ordersService.initMetamask(this.projectId, {
      transactionHash: hash,
      currency: this.paymentService.currency,
      tokenAmount: this.paymentService.tokenAmount,
      fiatAmount: this.paymentService.paymentAmount,
      refCode
    }).subscribe((rs) => {
      this.transactionWasInitiated = true;
      this.toastr.success(this.translateService.instant('PROJECT_PURCHASE.TRANSACTION_INIT'));
    }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.isDeploying$.next(false); });
  }

  transactionCompleted(payload: any): void {
    const int = setInterval(() => {
      if (this.transactionWasInitiated === true) {
        this.router.navigateByUrl(`/project/${this.projectSlug}/orderConfirmation?network=${this.selectedNetwork}&tx=${payload?.transactionHash}`);
        clearInterval(int);
      }
    }, 500);
  }
}
