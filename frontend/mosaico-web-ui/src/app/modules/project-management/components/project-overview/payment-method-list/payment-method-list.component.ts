import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { PaymentMethod, PaymentMethodService,BankDetailsService, PaymentMethodType } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../../constants';
import {
  selectPreviewProjectId,
  selectPreviewProjectPermissions,
  selectProjectPaymentMethods,
  selectProjectPreview,
  setPaymentMethods
} from '../../../store';
import { finalize, map } from "rxjs/operators";
import { combineLatest, BehaviorSubject } from 'rxjs';
import { ToastrService } from "ngx-toastr";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DEFAULT_MODAL_OPTIONS, ErrorHandlingService } from 'mosaico-base';
import { EditBankDataComponent } from '../../../modals';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-payment-method-list',
  templateUrl: './payment-method-list.component.html',
  styleUrls: ['./payment-method-list.component.scss']
})
export class PaymentMethodListComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  projectId: string;
  paymentMethods: PaymentMethod[] = [];
  canEdit = false;
  chosenMethods: { [key: string]: boolean } = {};
  projectMethods: any;
  requestInProgress$ = new BehaviorSubject<boolean>(false);

  constructor(
    private store: Store,
    private modalService: NgbModal,
    private paymentService: PaymentMethodService,
    private toastr: ToastrService,
    private errorHandler: ErrorHandlingService,
    private bankDetailsSrvc: BankDetailsService,
    private translateService: TranslateService
  ) { }

  ngOnInit(): void {
    this.fetchProjectId();
    this.fetchPermissions();
    this.fetchPaymentMethodConfiguration();
  }

  ngOnDestroy(): void {
    this.subs.sink?.unsubscribe();
  }

  private fetchProjectId(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((project) => {
      this.projectId = project?.project?.id;
    });
  }

  private fetchPermissions(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
  }

  private fetchPaymentMethodConfiguration(): void {
    this.subs.sink = combineLatest(this.paymentService.getPaymentMethods(), this.store.select(selectProjectPaymentMethods)).pipe(
      map(([allPaymentMethods$, projectEnabledMethods$]) => ({
        allPaymentMethods: allPaymentMethods$,
        projectEnabledMethods: projectEnabledMethods$,
      }))
    ).subscribe(response => {
      if (response && response.allPaymentMethods) {
        this.chosenMethods = {};
        this.projectMethods = response.projectEnabledMethods;
        this.paymentMethods = response.allPaymentMethods.data;
        this.store.dispatch(setPaymentMethods({ paymentMethods: this.projectMethods }));
        this.setSelectors();
      }
    });
  }

  private setSelectors(): void {
    if (this.paymentMethods) {
      if (this.projectMethods) {
        this.paymentMethods?.forEach((method) => {
          this.chosenMethods[method.key] = !!this.projectMethods.find(x => x === method.key);
        });
      }
    }
  }

  togglePaymentMethodAvailability(methodKey: string): void {
    if (this.projectId && this.projectId.length > 0) {
      if(methodKey === "BANK_TRANSFER"){
        this.subs.sink = this.bankDetailsSrvc.getBankDetails(this.projectId).subscribe((result) => {
          if (result && result.data) {
            if(result.data.account !== "" || result.data.accountAddress !== "" || result.data.bankName !== "" || result.data.swift !== "")
            {
              this.updatePaymentMethod(methodKey);
            }
            else
            {
              this.translateService.get('PROJECT.PAYMENT_METHOD.BANK_DETAILS.NOT_ADDED').subscribe((t) => this.toastr.error(t));
              this.setSelectors(); 
            }
          }
          else
            {
              this.translateService.get('PROJECT.PAYMENT_METHOD.BANK_DETAILS.NOT_ADDED').subscribe((t) => this.toastr.error(t));
              this.setSelectors(); 
            }
        });
      }
      else{
        this.updatePaymentMethod(methodKey);
      }
    }
  }

  updatePaymentMethod(methodKey: string): void{
    const methodElement = this.paymentMethods.find(x => x.key === methodKey);
        this.requestInProgress$.next(true);
        this.subs.sink = this.paymentService
          .upsertPaymentMethod(this.projectId, { paymentMethodKey: methodElement.key, isEnabled: this.chosenMethods[methodElement.key] })
          .subscribe(() => {
            if (!this.projectMethods.includes(methodKey)) {
              this.projectMethods = [...this.projectMethods, methodKey];
            } else {
              this.projectMethods = this.projectMethods.filter(key => key !== methodKey);
            }
            this.translateService.get('PROJECT.PAYMENT_METHOD.UPDATE.SUCCESS').subscribe((t) => this.toastr.success(t));
            this.updateAvailablePaymentMethodStorage();
            this.setSelectors();
            this.requestInProgress$.next(false);
          }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.requestInProgress$.next(false); this.setSelectors(); });
  }

  checkIsMethodIncluded(methodKey: string): void {
    return this.projectMethods.includes(methodKey);
  }

  private updateAvailablePaymentMethodStorage(): void {
    this.store.dispatch(setPaymentMethods({ paymentMethods: this.projectMethods }));
  }

  startEditingBankData(): void {
    const modalRef = this.modalService.open(EditBankDataComponent, DEFAULT_MODAL_OPTIONS);
    modalRef.componentInstance.projectId = this.projectId;
  }

}
