import { SubSink } from 'subsink';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { BlockchainService } from 'mosaico-base';
import { selectMetamaskConnected, setMetamaskConnected } from 'src/app/modules/wallet';

@Component({
  selector: 'app-confirm-wallet-disconnect',
  templateUrl: './confirm-wallet-disconnect.modal.html',
})
export class ConfirmWalletDisconnectModal implements OnInit, OnDestroy {
  private subs = new SubSink();
  isMetamaskAuthorized: boolean;

  constructor(private store: Store, private blockchainService: BlockchainService) {

  }
  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectMetamaskConnected).subscribe((res) => {
      this.isMetamaskAuthorized = res;
    });
  }

  async logout() {
    if (this.isMetamaskAuthorized) {
      this.store.dispatch(setMetamaskConnected({ isConnected: false }));
      await this.blockchainService.cleanup();
    }
  }
}
