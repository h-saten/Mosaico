import { SubSink } from 'subsink';
import { Component, HostBinding, OnInit, OnDestroy} from '@angular/core';
import { Store } from '@ngrx/store';
import { BlockchainService } from 'mosaico-base';
import { selectMetamaskConnected, setMetamaskAddress, setMetamaskConnected } from 'src/app/modules/wallet';

@Component({
  selector: 'app-non-custodial-wallet-inner',
  templateUrl: './non-custodial-wallet-inner.component.html',
})
export class NonCustodialWalletInnerComponent implements OnInit, OnDestroy {
  @HostBinding('class') class = 'menu menu-sub menu-sub-dropdown menu-column w-350px w-lg-375px';
  @HostBinding('attr.data-kt-menu') dataKtMenu = 'true';
  isMetamaskAuthorized: boolean;
  private subs = new SubSink();
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

  loginWithMetamask() {
    if (!this.isMetamaskAuthorized) {
      this.blockchainService.authenticateToMetamask().then((account: string) => {
        if (account) {
          this.store.dispatch(setMetamaskConnected({ isConnected: true }))
          this.store.dispatch(setMetamaskAddress({ address: account }));
        }
      });
    }
  }

  async logout() {
    if (this.isMetamaskAuthorized) {
      this.store.dispatch(setMetamaskConnected({ isConnected: false }));
      await this.blockchainService.cleanup();
    }
  }
}
