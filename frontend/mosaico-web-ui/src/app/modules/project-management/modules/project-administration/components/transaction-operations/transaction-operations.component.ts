import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { BlockchainService, ErrorHandlingService } from 'mosaico-base';
import { Operation, TransactionService } from 'mosaico-wallet';
import { LazyLoadEvent } from 'primeng/api';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-transaction-operations',
  templateUrl: './transaction-operations.component.html',
  styleUrls: ['./transaction-operations.component.scss']
})
export class TransactionOperationsComponent implements OnInit, OnDestroy {
  @Input() transactionId: string;
  subs = new SubSink();
  isLoading: boolean = true;
  operations: Operation[] = [];
  currentSkip: number = 0;
  currentTake: number = 10;
  page = 1;
  pageSize = 10;
  totalRecords: number;

  constructor(private transactionService: TransactionService, private errorHandler: ErrorHandlingService, private store: Store, private blockchainService: BlockchainService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
  }

  fetchOperations(event: LazyLoadEvent): void {
    this.isLoading = true;
    this.currentSkip = event.first;
    this.currentTake = event.rows;
    this.reloadOperations();
  }

  reloadOperations(): void {
    this.subs.sink = this.transactionService.getOperations(this.transactionId, this.currentTake, this.currentSkip)
      .subscribe((res) => {
        this.operations = res?.data?.entities;
        this.totalRecords = res?.data?.total;
        this.isLoading = false;
    }, (error) => { this.errorHandler.handleErrorWithToastr(error); });
  }

  redirectToEtherscan(o: Operation): void {
    if (o) {
      this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((chains) => {
        const chain = chains.find((c) => c.name === o.network);
        if (chain) {
          const link = this.blockchainService.getTransactionLink(o.transactionHash, chain.etherscanUrl);
          window.open(link, "_blank", 'noopener');
        }
      });
    }
  }

}
