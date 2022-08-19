import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {SubSink} from 'subsink';
import { TokenStaking, WalletService } from 'mosaico-wallet';
import { BlockchainNetworkType } from 'mosaico-base';
import { selectBlockchain } from '../../../../../store/selectors';

@Component({
  selector: 'app-token-stakings',
  templateUrl: './token-stakings.component.html',
  styleUrls: []
})
export class TokenStakingsComponent implements OnInit, OnDestroy {

  @Input() stakingId: string;
  network: BlockchainNetworkType;
  sub: SubSink = new SubSink();
  isLoaded = false;
  stakings: TokenStaking[];
  tokenSymbol: string;

  constructor(private walletService: WalletService, private store: Store) { }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  getWallet(): void {
    // this.sub.sink = this.store.select(selectUserWallets).subscribe((wallets) => {
    //   if(!this.isLoaded){
    //     if(wallets && wallets[0]){
    //       this.sub.sink = this.walletService.stakingDeposits(wallets[0], this.network, this.stakingId).subscribe((response) => {
    //         this.isLoaded = true;
    //         this.stakings = response.data.stakings;
    //         this.tokenSymbol = response.data.tokenSymbol;
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
