import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { CompanyWalletService, CompanyWalletBalance } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { selectCompanyWalletBalance, setCompanyWalletBalance } from '../../store';
import { selectBlockchain } from '../../../../store/selectors';
import { selectCompanyPreview } from '../../store/business.selectors';


@Component({
  selector: 'app-company-wallet-assets',
  templateUrl: './company-wallet-assets.component.html',
  styleUrls: ['./company-wallet-assets.component.scss']
})
export class CompanyWalletAssetsComponent implements OnInit, OnDestroy {
  balance: CompanyWalletBalance;
  isLoading = true;
  subs = new SubSink();
  address: string;
  network: string;
  companyId: string;

  constructor(private store: Store, private companyWalletService: CompanyWalletService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectCompanyWalletBalance).subscribe((wallet) => {
      if (wallet) {
        this.balance = wallet;
        this.address = this.balance?.address;
        this.isLoading = false;
      }
    });
    this.subs.sink = this.store.select(selectCompanyPreview).subscribe((c) => {
      this.companyId = c?.id;
      this.network = c?.network;
    });
  }

  loadAssets(): void {
    if (this.network && this.companyId && !this.isLoading) {
      this.isLoading = true;
      this.subs.sink = this.companyWalletService.getCompanyWalletTokens(this.companyId).subscribe((res) => {
        if (res?.data) {
          this.store.dispatch(setCompanyWalletBalance(res.data));
        }
        this.isLoading = false;
      });
    }
  }

}
