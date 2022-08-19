import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { TokenBalance, WalletInfo, WalletService } from 'mosaico-wallet';
import { BlockchainNetworkType } from 'mosaico-base';
import { selectUserWallet } from '../../../store';
import { selectWalletTokenBalance } from '../../../store/wallet.selectors';
import { selectUserInformation } from '../../../../user-management/store/user.selectors';
import { filter, take } from 'rxjs/operators';
import { setWalletBalance } from '../../../store/wallet.actions';
import { selectBlockchain } from 'src/app/store/selectors';

@Component({
  selector: 'app-wallet-panel-assets',
  templateUrl: './wallet-panel-assets.component.html',
  styleUrls: ['./wallet-panel-assets.component.scss']
})

export class WalletPanelAssetsComponent implements OnInit, OnDestroy {

  wallet: WalletInfo;
  tokens: TokenBalance[] = [];
  isLoading = true;
  isLoaded = false;
  subs = new SubSink();
  selectedNetwork: BlockchainNetworkType;

  constructor(private store: Store, private walletService: WalletService) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectBlockchain).subscribe((res) => {
      if(this.selectedNetwork !== res) {
        this.isLoaded = false;
        this.isLoading = true;
      }
      this.selectedNetwork = res;
    });
    this.subs.sink = this.store.select(selectUserWallet).subscribe((wallet) => {
      this.wallet = wallet;
      this.getTokensFromStore();
    });
  }

  getTokensFromStore(): void {
    this.subs.sink = this.store.select(selectWalletTokenBalance).pipe(filter((v) => !this.isLoaded)).subscribe((res) => {
      if (res?.tokens) {
        this.isLoading = false;
        this.tokens = res.tokens;
        this.isLoaded = true;
      }
    });
  }

  fetchAssets(force = false): void {
    if (this.wallet) {
      if (!this.isLoaded || force === true) {
        this.subs.sink = this.store.select(selectUserInformation).pipe(take(1)).subscribe((ui) => {
          if (ui) {
            this.isLoading = true;
            this.subs.sink = this.walletService.getTokenBalance(ui.id, this.wallet.network).subscribe((res) => {
              this.isLoading = false;
              if (res?.data) {
                this.tokens = res.data.tokens;
                this.store.dispatch(setWalletBalance(res.data));
              }
              this.isLoaded = true;
            });
          }
        });
      }
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}
