import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { PaymentMethodService } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../../constants';
import {
  selectPreviewProjectPermissions,
  selectProjectPaymentCurrencies,
  selectProjectPreview
} from '../../../store';
import { finalize } from "rxjs/operators";
import { ToastrService } from "ngx-toastr";
import { PaymentCurrency } from "mosaico-wallet";
import { ErrorHandlingService } from 'mosaico-base';
import { setPaymentCurrencies } from '../../../store';
import {ChangeEvent} from "@ckeditor/ckeditor5-angular";

export interface PaymentCurrencyOption {
  id: string;
  name: string;
  symbol: string;
  logoUrl: string;
  isEnabled: boolean;
  contractAddress: string;
  isNativeChainCurrency: boolean;
}

@Component({
  selector: 'app-payment-currencies-list',
  templateUrl: './payment-currencies-list.component.html',
  styleUrls: ['./payment-currencies-list.component.scss']
})
export class PaymentCurrenciesListComponent implements OnInit, OnDestroy {

  subs = new SubSink();

  projectId: string;
  network: string;
  private projectPaymentCurrencies: string[] = [];
  paymentCurrencies: PaymentCurrencyOption[] = [];
  canEdit = false;

  requestInProgress = false;

  constructor(private store: Store, private paymentService: PaymentMethodService, private toastr: ToastrService, private errorHandler: ErrorHandlingService) { }

  ngOnInit(): void {
    this.fetchProject();
    this.fetchPermissions();
    this.fetchProjectPaymentCurrencies();
  }

  ngOnDestroy() {
    this.subs.sink?.unsubscribe();
  }

  private fetchProject(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((project) => {
      if (project?.project?.id) {
        let projectChanged = false;
        if (this.projectId !== project.project.id) {
          this.projectId = project.project.id;
          projectChanged = true;
        }
        if (this.network !== project.company?.network || projectChanged) {
          this.network = project.company?.network;
          if (this.network) {
            this.fetchAvailablePaymentCurrencies(this.network);
          }
        }
      }
    });
  }

  private fetchPermissions(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
  }

  private fetchAvailablePaymentCurrencies(network: string): void {
    this.subs.sink = this.paymentService
      .paymentCurrencies(network)
      .subscribe(response => {
        this.paymentCurrencies = this.processAvailableCurrenciesList(response.data);
        this.updateProjectEnabledCurrenciesList(this.projectPaymentCurrencies);
      });
  }

  private processAvailableCurrenciesList(currencies: PaymentCurrency[]): PaymentCurrencyOption[] {
    const paymentCurrenciesList = [];
    currencies.forEach((paymentCurrency) => {
      const entry: PaymentCurrencyOption = {
        id: paymentCurrency.id,
        name: paymentCurrency.name,
        symbol: paymentCurrency.ticker,
        logoUrl: paymentCurrency.logoUrl,
        contractAddress: paymentCurrency.contractAddress,
        isNativeChainCurrency: paymentCurrency.nativeChainCurrency,
        isEnabled: false
      } as PaymentCurrencyOption;
      paymentCurrenciesList.push(entry);
    });
    return paymentCurrenciesList;
  }

  private fetchProjectPaymentCurrencies(): void {
    this.subs.sink = this.store.select(selectProjectPaymentCurrencies)
      .subscribe(projectPaymentCurrencies => {
        if (projectPaymentCurrencies) {
          this.projectPaymentCurrencies = projectPaymentCurrencies;
          this.updateProjectEnabledCurrenciesList(projectPaymentCurrencies);
        }
      });
  }

  private updateProjectEnabledCurrenciesList(projectCurrencies: string[]): void {
    this.paymentCurrencies.forEach((paymentCurrency) => {
      paymentCurrency.isEnabled = projectCurrencies.findIndex(currencyAddress => currencyAddress === paymentCurrency.contractAddress) !== -1 || paymentCurrency.isNativeChainCurrency;
    });
  }

  togglePaymentCurrencyAvailability(id: string): void {
    if (this.projectId && this.projectId.length > 0) {
      const methodElement = this.paymentCurrencies.find(x => x.id === id);
      this.requestInProgress = true;
      this.subs.sink = this.paymentService
        .upsertPaymentCurrency(this.projectId, { paymentCurrencyAddress: methodElement.contractAddress, isEnabled: methodElement.isEnabled })
        .pipe(finalize(() => {
          this.requestInProgress = false;
        }))
        .subscribe(() => {
          this.toastr.success("Payment method updated.");
          this.updateAvailablePaymentMethodStorage();
        }, () => {
          this.toastr.error("Payment method cannot be updated");
          methodElement.isEnabled = !methodElement.isEnabled;
        });
    }
  }

  private updateAvailablePaymentMethodStorage(): void {
    const projectAvailablePaymentCurrencies: string[] = this.paymentCurrencies.filter(x => x.isEnabled === true && !x.isNativeChainCurrency).map(x => x.contractAddress);
    this.store.dispatch(setPaymentCurrencies({ paymentCurrencies: projectAvailablePaymentCurrencies }));
  }

  test(id: string) {
    const methodElement = this.paymentCurrencies.find(x => x.id === id);
    methodElement.isEnabled = !methodElement.isEnabled;
  }
}
