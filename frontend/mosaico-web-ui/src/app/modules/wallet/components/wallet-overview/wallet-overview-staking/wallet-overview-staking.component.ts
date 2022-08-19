import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { WalletStaking, WalletService } from 'mosaico-wallet';
import { selectBlockchain } from '../../../../../store/selectors';
import { BlockchainNetworkType } from 'mosaico-base';
import { selectUserWallet } from '../../../store';
import _ from 'lodash';

@Component({
  selector: 'app-wallet-overview-staking',
  templateUrl: './wallet-overview-staking.component.html',
  styleUrls: ['./wallet-overview-staking.component.scss']
})

export class WalletOverviewStakingComponent implements OnInit, OnDestroy {
  network: BlockchainNetworkType;
  sub: SubSink = new SubSink();
  isLoaded = false;
  stakings: WalletStaking[] = [];
  stakingId: string | null;

  detailsRowIndex: number;

  constructor(private walletService: WalletService, private store: Store) { }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectBlockchain).subscribe((b) => {
      this.network = b;
      if (!this.network || this.network.length === 0) {
        this.network = 'Ethereum';
      }
    });
  }

  fetchStakings(): void {
    this.sub.sink = this.store.select(selectUserWallet).subscribe((walletInfo) => {
      if(!this.isLoaded){
        if(walletInfo?.address && walletInfo?.network){
          // this.sub.sink = this.walletService.getStakings(walletInfo.address, walletInfo.network).subscribe((response) => {
          //   if(response?.data?.stakings){
          //     this.stakings = response.data.stakings;
          //   }
          //   this.isLoaded = true;
          // });
        }
      }
    });
  }

  showStakingTransactionDetails(rowIndex: number): void {
    if (this.detailsRowIndex === rowIndex) {
      this.detailsRowIndex = -1;
    } else {
      this.detailsRowIndex = rowIndex;
    }
  }

  deposit(stakingId: string): void {
    // if(stakingId)
    // {
    //   const staking: WalletStaking = _.find(this.stakings, (item) => { return item?.stakingId == stakingId; });
    //   if(staking)
    //     this.sub.sink = this.walletService.depositStake({ ...staking }).subscribe((res) => {
    //     });
    // }
  }

  withdraw(stakingId: string) {
    // if(stakingId)
    // {
    //   this.sub.sink = this.walletService.withdrawStake(stakingId).subscribe((res) => {

    //   });
    // }
  }


  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
