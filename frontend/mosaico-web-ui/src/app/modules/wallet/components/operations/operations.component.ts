import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ErrorHandlingService, BlockchainService } from 'mosaico-base';
import { Operation, OperationsService, TransactionService } from 'mosaico-wallet';
import { LazyLoadEvent } from 'primeng/api';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-operations',
  templateUrl: './operations.component.html',
  styleUrls: ['./operations.component.scss']
})
export class OperationsComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  isLoading: boolean = true;
  operations: Operation[] = [];
  currentSkip: number = 0;
  currentTake: number = 10;
  page = 1;
  pageSize = 10;
  totalRecords: number;

  constructor(private operationService: OperationsService, private errorHandler: ErrorHandlingService, private store: Store,  private blockchainService: BlockchainService) { }

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
    this.subs.sink = this.operationService.getUserOperations(this.currentSkip, this.currentTake)
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
