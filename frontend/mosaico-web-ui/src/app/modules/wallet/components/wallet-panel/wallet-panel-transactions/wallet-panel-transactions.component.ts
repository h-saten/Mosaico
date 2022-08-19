import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { BlockchainNetworkType, BlockchainService } from 'mosaico-base';
import { Transaction, TransactionService, WalletInfo } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { selectUserWallet } from '../../../store';

@Component({
  selector: 'app-wallet-panel-transactions',
  templateUrl: './wallet-panel-transactions.component.html',
  styleUrls: ['./wallet-panel-transactions.component.scss']
})

export class WalletPanelTransactionsComponent implements OnInit, OnDestroy {

  sub: SubSink = new SubSink();
  isLoading = true;
  network: BlockchainNetworkType = 'Ethereum';
  transactions: Transaction[] = [];
  wallet: WalletInfo;
  isLoaded = false;
  totalCount = 0;
  page = 1;
  pageSize = 15;
  isLoadingMore = false;

  constructor(private transactionService: TransactionService, private store: Store) {}

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectUserWallet).subscribe((wallet) => {
      this.wallet = wallet;
      this.fetchTransactions();
    });
  }

  private getNextSkip(): number {
    return (this.page - 1) * this.pageSize;
  }

  public fetchTransactions(force = false): void {
    if (this.wallet?.address && this.wallet?.network) {
      if (!this.isLoaded || force === true) {
        this.page = 1;
        this.isLoading = true;
        this.sub.sink = this.transactionService.getTransactions(this.wallet.address, this.wallet.network, this.getNextSkip(), this.pageSize).subscribe((res) => {
          if (res?.data) {
            this.transactions = res.data.entities;
            this.totalCount = res.data.total;
          }
          this.isLoading = false;
          this.isLoaded = true;
        });
      }
    }
  }

  public fetchMore(): void {
    if(this.wallet && this.isLoaded){
      this.isLoadingMore = true;
      this.page++;
      this.sub.sink = this.transactionService.getTransactions(this.wallet.address, this.wallet.network, this.getNextSkip(), this.pageSize).subscribe((res) => {
        if(res?.data?.entities) {
          this.transactions.push(...res.data.entities);
        }
        this.isLoadingMore = false;
      });
    }
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
