import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { BlockchainNetworkType } from 'mosaico-base';
import { CompanyWalletInfo } from 'mosaico-dao';
import { Transaction, TransactionService } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { selectCompanyWallet } from '../../store';
import { selectBlockchain } from '../../../../store/selectors';
import { selectCompanyPreview } from '../../store/business.selectors';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-company-wallet-transactions',
  templateUrl: './company-wallet-transactions.component.html',
  styleUrls: ['./company-wallet-transactions.component.scss']
})
export class CompanyWalletTransactionsComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();
  isLoading = true;
  network: BlockchainNetworkType = 'Ethereum';
  transactions: Transaction[] = [];
  wallet: CompanyWalletInfo;
  isLoaded = false;
  totalCount = 0;
  page = 1;
  pageSize = 15;
  isLoadingMore = false;
  companyId: string;

  constructor(private transactionService: TransactionService, private store: Store) { }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectCompanyPreview).pipe(take(1)).subscribe((c) => {
      this.companyId = c?.id;
      this.network = c?.network;
      this.getWallet();
    });
  }

  getWallet(): void {
    this.sub.sink = this.store.select(selectCompanyWallet).subscribe((wallet) => {
      this.wallet = wallet;
      this.loadTransactions();
    });
  }

  private getNextSkip(): number {
    return (this.page - 1) * this.pageSize;
  }

  public loadTransactions(force = false): void {
    if (this.wallet) {
      //TODO: implement pagination
      if (!this.isLoaded || force === true) {
        this.page = 1;
        this.isLoading = true;
        this.sub.sink = this.transactionService.getCompanyTransactions(this.companyId, this.getNextSkip(), this.pageSize).subscribe((res) => {
          if (res && res.data) {
            this.transactions = res.data.entities;
            this.totalCount = res.data.total;
          }
          this.isLoading = false;
          this.isLoaded = true;
        });
      }
    }
  }

  public loadMore(): void {
    if(this.wallet && this.isLoaded){
      this.isLoadingMore = true;
      this.page++;
      this.sub.sink = this.transactionService.getCompanyTransactions(this.companyId, this.getNextSkip(), this.pageSize).subscribe((res) => {
        if(res && res.data && res.data.entities) {
          this.transactions.push(...res.data.entities);
        }
        this.isLoadingMore = false;
      });
    }
  }

}

