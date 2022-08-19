import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { Store } from '@ngrx/store';
import { CompanyHolder, CompanyService } from 'mosaico-dao';
import { Token, TransactionService } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { finalize } from "rxjs/operators";

interface HolderListItem {
  walletAddress: string;
  tokenAmount: number;
  tokenSymbol: string;
  logoUrl: string;
}

@Component({
  selector: 'app-company-holders-list',
  templateUrl: './company-holders-list.component.html',
  styleUrls: ['./company-holders-list.component.scss']
})
export class CompanyHoldersListComponent implements OnInit, OnDestroy, OnChanges {

  private subs: SubSink = new SubSink();

  @Input() companyId: string;
  @Input() tokens: Token[];

  isLoading = true;
  isLoaded = false;
  totalCount = 0;
  page = 1;
  pageSize = 15;
  isLoadingMore = false;

  holderItems: HolderListItem[] = [];
  filterByTokenId: string;

  constructor(private transactionService: TransactionService, private store: Store, private companyService: CompanyService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void { }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.companyId && changes.companyId?.currentValue) {
      this.getHolders(changes.companyId.currentValue, this.tokens);
      this.isLoading = false;
    }
  }

  private getHolders(companyId: string, tokens: Token[]): void {
    this.isLoading = true;
    const pageNumber = this.page - 1;
    this.subs.sink = this.companyService
      .getHolders(companyId, this.filterByTokenId ?? '', pageNumber, this.pageSize)
      .pipe(finalize(() => {
        this.isLoading = false;
      }))
      .subscribe(response => {
        const composedHolders = this.composeHoldersWithToken(response.data.entities, tokens);
        if (pageNumber === 0) {
          this.holderItems = composedHolders;
        } else {
          this.holderItems.push(...composedHolders);
        }
        this.totalCount = response.data.total;
        this.isLoaded = true;
        this.isLoadingMore = false;
      });
  }

  private composeHoldersWithToken(holders: CompanyHolder[], tokens: Token[]): HolderListItem[] {
    const holdersList: HolderListItem[] = [];
    if (holders) {
      holders.forEach(holder => {
        const token = tokens.find(x => x.symbol === holder.tokenSymbol);
        if (token) {
          holdersList.push({
            logoUrl: token.logoUrl,
            tokenAmount: holder.tokenAmount,
            tokenSymbol: holder.tokenSymbol,
            walletAddress: holder.walletAddress
          });
        }
      });
    }
    return holdersList;
  }

  public loadMore(): void {
    if (this.holderItems.length > 0 && this.isLoaded) {
      this.isLoadingMore = true;
      this.page++;
      this.getHolders(this.companyId, this.tokens);
    }
  }

  filterByToken(id: string): void {
    if (this.filterByTokenId === id) {
      this.filterByTokenId = null;
      return;
    }

    this.filterByTokenId = id;
    this.getHolders(this.companyId, this.tokens);
  }
}

