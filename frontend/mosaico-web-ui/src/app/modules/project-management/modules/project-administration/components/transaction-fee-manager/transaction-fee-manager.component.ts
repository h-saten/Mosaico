import { Component, Input, OnDestroy, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { ErrorHandlingService } from 'mosaico-base';
import { ProjectTransactions } from 'mosaico-project';
import { SalesAgent, TransactionService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-transaction-fee-manager',
  templateUrl: './transaction-fee-manager.component.html',
  styleUrls: ['./transaction-fee-manager.component.scss']
})
export class TransactionFeeManagerComponent implements OnInit, OnDestroy, OnChanges {
  @Input() transaction: ProjectTransactions;
  subs = new SubSink();
  currentFee: number;
  @Input() salesAgents: SalesAgent[] = [];
  currentSalesAgentId: string;

  constructor(private transactionService: TransactionService, private errorHandler: ErrorHandlingService, private toastr: ToastrService) { }
  
  ngOnChanges(changes: SimpleChanges): void {
    if(changes.transaction) {
      this.currentFee = this.transaction?.feePercentage;
      this.currentSalesAgentId = this.transaction?.salesAgentId;
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
  }

  updateFee(): void {
    this.subs.sink = this.transactionService.updateFee(this.transaction.transactionId, this.currentFee).subscribe((res) => {
      this.toastr.success('Fee was updated. Refresh the the page to see changes');
    }, (error) => { this.errorHandler.handleErrorWithToastr(error);});
  }

  updateSalesAgent(): void {
    this.subs.sink = this.transactionService.updateAgent(this.transaction.transactionId, this.currentSalesAgentId).subscribe((res) => {
      this.toastr.success('Agent was updated');
    }, (error) => { this.errorHandler.handleErrorWithToastr(error);});
  }

}
