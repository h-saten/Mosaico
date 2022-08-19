import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { Observable, of, zip } from 'rxjs';
import { selectUserWallet } from '../../../store';
import { PackagesWallet, WalletService } from 'mosaico-wallet';

@Component({
  selector: 'app-wallet-overview-packages',
  templateUrl: './wallet-overview-packages.component.html',
  styleUrls: ['./wallet-overview-packages.component.scss']
})

export class WalletOverviewPackagesComponent implements OnInit, OnDestroy {

  sub: SubSink = new SubSink();
  isLoaded: boolean = false;
  packages: PackagesWallet[];

  constructor(
    private store: Store,
    private walletService: WalletService
  ) {}

  ngOnInit(): void {
      
  }

  fetchPackages(): void {
    this.sub.sink = this.store.select(selectUserWallet).subscribe((walletInfo) => {
      if(!this.isLoaded){
        if(walletInfo?.address && walletInfo?.network){
          this.sub.sink = this.walletService.getPackages(walletInfo?.address, walletInfo?.network).subscribe((res) => {
            if(res?.data?.packages)
              this.packages = res.data.packages;
            this.isLoaded = true;
          });
        }
      }
    }); 
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
