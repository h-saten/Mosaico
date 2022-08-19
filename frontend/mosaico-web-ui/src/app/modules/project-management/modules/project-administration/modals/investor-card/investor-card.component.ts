import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorHandlingService, FormDialogBase } from 'mosaico-base';
import { ProjectInvestorQueryResponse, TransactionService } from 'mosaico-wallet';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';


export interface InvestorCardPayload {
  projectId: string;
  userId: string;
}

@Component({
  selector: 'app-investor-card',
  templateUrl: './investor-card.component.html',
  styleUrls: ['./investor-card.component.scss']
})
export class InvestorCardComponent extends FormDialogBase implements OnInit, OnDestroy {
  subs = new SubSink();
  isLoading$ = new BehaviorSubject<boolean>(false);
  payload: InvestorCardPayload;
  currentInvestor: ProjectInvestorQueryResponse;

  constructor(modalService: NgbModal, private translateService: TransactionService, private errorHandler: ErrorHandlingService) { 
    super(modalService);
    this.extraOptions = {
      modalDialogClass: "mosaico-payment-modal"
    };
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
  }

  open(payload?: InvestorCardPayload): void {
    this.payload = payload;
    if(this.payload) {
      this.isLoading$.next(true);
      this.subs.sink = this.translateService.getInvestorDetails(this.payload.projectId, this.payload.userId).subscribe((res) => {
        this.currentInvestor = res?.data;
        this.isLoading$.next(false);
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.isLoading$.next(false); });
      super.open();
    }
  }

  createForm(): void {
    this.form = new FormGroup({});
  }
}
