import { SubSink } from 'subsink';
import { Component, HostBinding, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectUserWallet, selectWalletTokenBalance } from '../../../../../../modules/wallet/store';
import { AuthService, BlockchainNetworkType, BlockchainService } from 'mosaico-base';
import { TokenBalance, WalletBalance, WalletService } from 'mosaico-wallet';
import { selectBlockchain } from 'src/app/store/selectors';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, Observable } from 'rxjs';
import { selectIsAuthorized } from 'src/app/modules/user-management/store';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-wallet-inner',
  templateUrl: './wallet-inner.component.html',
})
export class WalletInnerComponent implements OnInit, OnDestroy {
  @HostBinding('class') class = 'menu menu-sub menu-sub-dropdown menu-column w-350px w-lg-375px';
  @HostBinding('attr.data-kt-menu') dataKtMenu = 'true';

  private subs = new SubSink();
  selectedNetwork: BlockchainNetworkType;
  walletAddress: string;
  userId: string;
  balance: WalletBalance;
  isAuthorized$ = new Observable<boolean>();
  @Output() redirected = new EventEmitter();

  constructor(private store: Store, private toastr: ToastrService,
    private auth: AuthService, private router: Router,
    private translateService: TranslateService) {

  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.fetchCurrentBlockchainNetwork();
    this.isAuthorized$ = this.store.select(selectIsAuthorized);
  }

  private fetchCurrentBlockchainNetwork(): void {
    this.subs.sink = this.store.select(selectBlockchain).subscribe((network) => {
      if (network) {
        this.saveBlockchainNetwork(network);
        this.fetchUserWallets();
      }
    });
  }

  private saveBlockchainNetwork(network: BlockchainNetworkType): void {
    this.selectedNetwork = network;
    if (!this.selectedNetwork || this.selectedNetwork.length === 0) {
      this.selectedNetwork = 'Ethereum';
    }
  }

  private fetchUserWallets(): void {
    this.subs.sink = this.store.select(selectUserWallet).subscribe((res) => {
      if (res?.address && res?.network) {
        this.saveUserWallet(res.address, res.network);
        this.fetchTokens();
      }
    });
  }

  private saveUserWallet(address: string, network: BlockchainNetworkType): void {
    this.walletAddress = address;
    this.selectedNetwork = network;
  }

  private fetchTokens(): void {
    this.subs.sink = this.store.select(selectWalletTokenBalance).subscribe((res) => {
      if (res) {
        this.balance = res;
      }
    });
  }

  onCopied(): void {
    this.toastr.success(this.translateService.instant('ADDRESS_COPIED'));
  }

  login(): void {
    this.auth.loginWithRedirect(this.router.url);
  }

  redirectToWallet(): void {
    this.router.navigateByUrl('/wallet');
    this.redirected.next(true);
  }
}
