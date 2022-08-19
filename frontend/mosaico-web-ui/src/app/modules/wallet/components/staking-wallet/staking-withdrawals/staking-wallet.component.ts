import {Component, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {SubSink} from 'subsink';
import { StakingWithdrawal, WalletService } from 'mosaico-wallet';
import { BlockchainNetworkType } from 'mosaico-base';
import { selectBlockchain } from '../../../../../store/selectors';
@Component({
  selector: 'app-staking-withdrawals',
  templateUrl: './staking-wallet.component.html',
  styleUrls: []
})
export class StakingWithdrawalsComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();
  isLoaded = false;
  withdrawals: StakingWithdrawal[];
  network: BlockchainNetworkType;

  constructor(private walletService: WalletService, private store: Store) { }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  getWallet(): void {
    // this.sub.sink = this.store.select(selectUserWallets).subscribe((wallets) => {
    //   if(!this.isLoaded){
    //     if(wallets && wallets[0]){
    //       this.sub.sink = this.walletService.stakingWithdrawals(wallets[0], 'Ethereum').subscribe((response) => {
    //         this.isLoaded = true;
    //         this.withdrawals = response.data.withdrawals;
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
}
