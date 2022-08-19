import {Component, OnDestroy, OnInit} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import {ActivatedRoute} from "@angular/router";
import {
  selectPreviewProjectActiveStage, selectProjectPaymentCurrenciesDetails,
  selectProjectPaymentMethods, selectProjectPreview,
  selectProjectPreviewToken,
  setCurrentCompanyPaymentDetails,
  setCurrentProjectActiveStage, setPaymentCurrenciesDetails
} from "../../store";
import {SubSink} from "subsink";
import {Store} from "@ngrx/store";
import {PaymentMethod, PaymentMethodService, PaymentMethodType, Token} from "mosaico-wallet";
import {PaymentCurrency, ProjectService, Stage} from "mosaico-project";
import { StonlyService } from '../../../../services/stonly.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit, OnDestroy {

  public subs: SubSink = new SubSink();

  paymentGatewayType: PaymentMethodType;
  projectId: string;
  projectSlug: string;
  token: Token;
  currentStage: Stage;
  presetAmount: number;
  availablePaymentMethods: PaymentMethodType[];
  paymentCurrencies: PaymentCurrency[] = [];
  refCode: string;
  allPaymentMethods: PaymentMethod[] = [];

  checkoutFAQs = [
    {
      title: 'CHECKOUT.hint.kyc',
      stonlyKey: 'udesWOsgTJ'
    },
    {
      title: 'CHECKOUT.hint.pln',
      stonlyKey: 'Kghw7jhr3L'
    },
    {
      title: 'CHECKOUT.hint.metamask',
      stonlyKey: 'xSgRSY30bV'
    },
    {
      title: 'CHECKOUT.hint.mosaico',
      stonlyKey: '7GBhTEqFjY'
    }
  ]

  constructor(
    private translateService: TranslateService,
    private paymentMethodService: PaymentMethodService,
    private route: ActivatedRoute,
    private store: Store,
    private projectService: ProjectService, public stonlyService: StonlyService) {}

  ngOnInit(): void {
    if (!this.route.parent) {
      return;
    }
    this.getProject();
    this.getProjectSlug();
    this.getToken();
    this.getCurrentStage();
    this.fetchPaymentMethods();
    this.fetchPaymentCurrencies();
    this.refCode = localStorage.getItem('refCode');
    this.presetAmount = +this.route.snapshot.queryParamMap.get('amount');
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private getProjectSlug(): void {
    this.projectSlug = this.route.parent.snapshot.paramMap.get('projectId') as string;
  }

  private getProject(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.projectId = data.project.id;
      }
    });
  }

  private fetchPaymentMethods(): void {
    this.subs.sink = this.paymentMethodService.getPaymentMethods().subscribe((res) => {
      this.allPaymentMethods = res.data;
    });
    this.subs.sink = this.store
      .select(selectProjectPaymentMethods)
      .subscribe(response => {
        this.availablePaymentMethods = response;
        this.paymentGatewayType = this.defaultPaymentMethod(response);
      });
  }

  private defaultPaymentMethod(paymentMethods: PaymentMethodType[]): PaymentMethodType {
    const preferredPaymentMethod: PaymentMethodType = "MOSAICO_WALLET";
    const preferredPaymentMethodExist = paymentMethods.findIndex(x => x === preferredPaymentMethod) !== -1;
    return preferredPaymentMethodExist ? preferredPaymentMethod : paymentMethods[0];
  }

  private getToken(): void {
    this.subs.sink = this.store.select(selectProjectPreviewToken).subscribe((token) => {
      this.token = token;
    });
  }

  private getCurrentStage(): void {
    this.subs.sink = this.store.select(selectPreviewProjectActiveStage)
      .subscribe((response: Stage) => {
        if (response) {
          this.currentStage = response;
        } else {
          this.getProjectSaleReport();
        }
      });
  }

  private getProjectSaleReport(): void {
    this.subs.sink = this.projectService
      .getProjectSaleReport(this.projectSlug)
      .subscribe((res) => {
        if (res && res.data) {
          if (res.data.stage) {
            this.store.dispatch(setCurrentProjectActiveStage({ activeStage: res.data.stage }));
          }
          this.store.dispatch(setPaymentCurrenciesDetails({ paymentCurrencies: res.data.paymentCurrencies }));
          this.store.dispatch(setCurrentCompanyPaymentDetails({walletAddress: res.data.companyWalletAddress, network: res.data.companyWalletNetwork}));
        }
      });
  }

  kangaSelected(e: any): void {
    const isAvailable = this.isMethodAvailableGuard('KANGA_EXCHANGE');
    if(!isAvailable) {
      throw new Error("Method not supported");
    }
    this.paymentGatewayType = 'KANGA_EXCHANGE';
  }

  mosaicoSelected(e: any): void {
    const isAvailable = this.isMethodAvailableGuard('MOSAICO_WALLET');
    if(!isAvailable) {
      throw new Error("Method not supported");
    }
    this.paymentGatewayType = 'MOSAICO_WALLET';
  }

  metamaskSelected(e: any): void {
    const isAvailable = this.isMethodAvailableGuard('METAMASK');
    if(!isAvailable) {
      throw new Error("Method not supported");
    }
    this.paymentGatewayType = 'METAMASK';
  }

  cardSelected(e: any): void {
    const isAvailable = this.isMethodAvailableGuard('CREDIT_CARD');
    if(!isAvailable) {
      throw new Error("Method not supported");
    }
    this.paymentGatewayType = 'CREDIT_CARD';
  }

  bankSelected(e: any): void {
    const isAvailable = this.isMethodAvailableGuard('BANK_TRANSFER');
    if(!isAvailable) {
      throw new Error("Method not supported");
    }
    this.paymentGatewayType = 'BANK_TRANSFER';
  }

  binanceSelected(e: any): void {
    const isAvailable = this.isMethodAvailableGuard('BINANCE');
    if(!isAvailable) {
      throw new Error("Method not supported");
    }
    this.paymentGatewayType = 'BINANCE';
  }

  public isMethodAvailableGuard(method: string): boolean {
    return this.availablePaymentMethods.findIndex(x => x === method) !== -1;
  }

  private fetchPaymentCurrencies(): void {
    this.subs.sink = this.store.select(selectProjectPaymentCurrenciesDetails)
      .subscribe((response: PaymentCurrency[]) => {
        if (response && response.length > 0) {
          this.paymentCurrencies = response;
        }
      });
  }
}
