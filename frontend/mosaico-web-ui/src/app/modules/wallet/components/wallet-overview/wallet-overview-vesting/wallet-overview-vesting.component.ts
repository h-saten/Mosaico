import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import {VestingWallet, WalletService} from 'mosaico-wallet';
import { BlockchainNetworkType } from 'mosaico-base';
import { selectBlockchain } from '../../../../../store/selectors';

@Component({
  selector: 'app-wallet-overview-vesting',
  templateUrl: './wallet-overview-vesting.component.html',
  styleUrls: ['./wallet-overview-vesting.component.scss']
})

export class WalletOverviewVestingComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();
  isLoaded = false;
  vestings: VestingWallet[] = [];
  network: BlockchainNetworkType = 'Ethereum';

  constructor(private walletService: WalletService, private store: Store) {}

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectBlockchain).subscribe((b) => {
      this.network = b;
      if (!this.network || this.network.length === 0) {
        this.network = 'Ethereum';
      }
    });
  }

  fetchVesting(): void {
    // this.sub.sink = this.store.select(selectUserWallets).subscribe((wallets) => {
    //   if(!this.isLoaded){
    //     if(wallets && wallets[0]){
    //       this.sub.sink = this.walletService.getVestings(wallets[0], this.network).subscribe((response) => {
    //         this.isLoaded = true;
    //         this.vestings = response.data.vestings;
    //       });
    //     }
    //   }
    // });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
