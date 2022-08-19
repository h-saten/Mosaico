import {Component, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {SubSink} from 'subsink';
import {WalletStaking, WalletService} from 'mosaico-wallet';
import { BlockchainNetworkType } from 'mosaico-base';
import { selectBlockchain } from '../../../../store/selectors';

type Tabs =
  | 'staking_withdrawals'
  | 'staking_stakes';

@Component({
  selector: 'app-staking-wallet',
  templateUrl: './staking-wallet.component.html',
  styleUrls: ['./staking-wallet.component.scss']
})
export class StakingWalletComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();
  isLoaded = false;
  stakings: WalletStaking[] = [];
  network: BlockchainNetworkType;
  activeTab: Tabs = 'staking_stakes';

  detailsRowIndex: number;

  setTab(tab: Tabs): void {
    this.activeTab = tab;
  }

  activeClass(tab: Tabs): string {
    return tab === this.activeTab ? 'show active' : '';
  }


  constructor(private walletService: WalletService, private store: Store) { }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  getWallet(): void {
    // this.sub.sink = this.store.select(selectUserWallets).subscribe((wallets) => {
    //   if(!this.isLoaded){
    //     if(wallets && wallets[0]){
    //       this.sub.sink = this.walletService.getStakings(wallets[0], this.network).subscribe((response) => {
    //         this.isLoaded = true;
    //         this.stakings = response.data.stakings;
    //       });
    //     }
    //   }
    // });
  }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectBlockchain).subscribe((b) => {
      this.network = b;
      if(!this.network || this.network.length === 0) {
        this.network = 'Ethereum';
      }
      this.getWallet();
    });
  }

  showStakingTransactionDetails(rowIndex: number): void {
    if (this.detailsRowIndex === rowIndex) {
      this.detailsRowIndex = -1;
    } else {
      this.detailsRowIndex = rowIndex;
    }
  }

  withdraw(stakingId: string): void {
    //this.walletService.withdrawStake(stakingId).toPromise().then();
  }
}
