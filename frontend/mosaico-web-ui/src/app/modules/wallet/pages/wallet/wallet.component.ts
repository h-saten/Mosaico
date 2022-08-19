import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { BlockchainNetworkType, BlockchainService, WalletConnectService } from 'mosaico-base';
import { KangaWalletService, WalletService } from 'mosaico-wallet';
import { take } from 'rxjs/operators';
import { UserInformation } from 'src/app/modules/user-management/models';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
import { clearWalletState, selectWalletConnectAddress, selectWalletConnected } from '../../store';
import { zip, Observable } from 'rxjs';
import { selectBlockchain } from '../../../../store/selectors';
import { setKangaWalletBalance, setWalletBalance, setWalletInfo, setKangaTotalAssetValue, setWalletConnected, setWalletConnectAddress } from '../../store/wallet.actions';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.scss']
})
export class WalletComponent implements OnInit, OnDestroy {
  private subs: SubSink = new SubSink();
  isConnected$: Observable<boolean>;
  walletAddress$: Observable<string>;

  constructor(
    private store: Store,
    private walletService: WalletService,
    private kangaWalletService: KangaWalletService,
    private walletConnect: WalletConnectService
  ) { }

  ngOnInit(): void {
    this.subs.sink = zip(this.store.select(selectUserInformation).pipe(take(1)), this.store.select(selectBlockchain).pipe(take(1))).subscribe((responses) => {
      if(responses && responses.length === 2 && responses[0]?.id) {
        this.getTokenBalance(responses[0]?.id, responses[1]);
        this.fetchUserKangaWalletBalance();
      }
    });
    this.isConnected$ = this.store.select(selectWalletConnected);
    this.walletAddress$ = this.store.select(selectWalletConnectAddress);
  }

  private getTokenBalance(userId: string, network: BlockchainNetworkType): void {
    this.subs.sink = this.walletService.getTokenBalance(userId, network).subscribe((res) => {
      this.store.dispatch(setWalletBalance(res?.data));
      this.store.dispatch(setWalletInfo({address: res?.data?.address, network}));
    });
  }

  ngOnDestroy(): void {
    this.store.dispatch(clearWalletState());
    this.subs.unsubscribe();
  }

  private fetchUserKangaWalletBalance(): void {
    this.subs.sink = this.subs.sink = this.kangaWalletService
      .getUserKangaBalance()
      .subscribe(response => {
        this.store.dispatch(setKangaWalletBalance({kangaAssets: response.data.assets}));
        this.store.dispatch(setKangaTotalAssetValue({totalAssetValue: response.data?.totalWalletValue}));
      });
  }

  async connectWallet(): Promise<void> {
    try{
      const web3 = await this.walletConnect.getWeb3();
      this.store.dispatch(setWalletConnected({isConnected: true}));
      const address = await this.walletConnect.getCurrentWallet();
      this.store.dispatch(setWalletConnectAddress({address}));
    }
    catch(e) {
      this.store.dispatch(setWalletConnected({isConnected: false}));
      this.store.dispatch(setWalletConnectAddress({address: null}));
    }
  }

  async disconnectWallet(): Promise<void> {
    try{
      this.walletConnect.cleanup();
      this.store.dispatch(setWalletConnected({isConnected: false}));
      this.store.dispatch(setWalletConnectAddress({address: null}));
    }
    catch(e) {

    }
  }
}
