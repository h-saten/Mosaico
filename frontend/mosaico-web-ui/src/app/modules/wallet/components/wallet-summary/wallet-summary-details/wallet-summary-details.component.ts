import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Blockchain, RampUserKYC, TransakUserKYC, WalletBalance, WalletService } from 'mosaico-wallet';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { BlockchainNetworkType } from 'mosaico-base';
import { SubSink } from 'subsink';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { selectBlockchain, selectCurrentActiveBlockchains, } from '../../../../../store/selectors';
import { setCurrentBlockchain } from '../../../../../store/actions';
import { setWalletInfo, BlockchainNetworks } from '../../../store';
import { setWalletBalance } from '../../../store/wallet.actions';
import { selectWalletTokenBalance, selectTotalKangaAssetValue } from '../../../store/wallet.selectors';

@Component({
  selector: 'app-wallet-summary-details',
  templateUrl: './wallet-summary-details.component.html',
  styleUrls: ['./wallet-summary-details.component.scss']
})
export class WalletSummaryDetailsComponent implements OnInit, OnDestroy {

  public summary: WalletBalance;
  isLoading = true;
  isLoaded = false;
  sub: SubSink = new SubSink();
  walletAddress: string;
  userId: string;
  networks: Blockchain[] = [];
  networksWalletAddresses: BlockchainNetworks[] = [];
  transakUserKyc: TransakUserKYC;
  rampUserKyc: RampUserKYC;
  kangaTotalAssetValue: number = 0;
  public selectedNetwork: BlockchainNetworkType;

  constructor(
    private store: Store,
    private toastr: ToastrService,
    private translateService: TranslateService
  ) { }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectUserInformation).subscribe((res) => {
      if (res?.id && res?.email?.length > 0) {
        this.userId = res.id;
        this.fetchCurrentBlockchainNetwork();
        this.getWalletSummary();
        this.getKangaSummary();
        this.transakUserKyc = {
          email: res.email,
          userData: {
            email: res.email,
            firstName: res.firstName,
            lastName: res.lastName,
            dob: res.dob?.toDateString(),
            address: {
              city: res.city,
              countryCode: res.country,
              addressLine1: res.street
            },
            mobileNumber: res.phoneNumber
          }
        };
        this.rampUserKyc = {
          email: res.email
        };
      }
    });
    this.sub.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((res) => {
      this.networks = res;
    });
  }

  private getKangaSummary(): void {
    this.sub.sink = this.store.select(selectTotalKangaAssetValue).subscribe((t) => {
      this.kangaTotalAssetValue = t;
    });
  }

  private getWalletSummary(): void {
    this.sub.sink = this.store.select(selectWalletTokenBalance).subscribe((res) => {
      if (res) {
        this.summary = res;
        this.walletAddress = this.summary.address;
        this.isLoading = false;
        this.isLoaded = true;
      }
    });
  }

  withdraw(): void {

  }

  private fetchCurrentBlockchainNetwork(): void {
    this.sub.sink = this.store.select(selectBlockchain).subscribe((network) => {
      if (network) {
        this.selectedNetwork = network;
        if (!this.selectedNetwork || this.selectedNetwork.length === 0) {
          this.selectedNetwork = 'Ethereum';
        }
      }
    });
  }

  saveCurrentBlockchain(network: BlockchainNetworkType): void {
    if (network) {
      this.store.dispatch(setCurrentBlockchain({ blockchain: network }));
    }
  }

  onCopied(): void {
    this.sub.sink = this.translateService.get('USER_WALLET.SUMMARY.MESSAGES.COPIED').subscribe((t) => {
      this.toastr.success(t);
    });
  }

  onNetworkChanged(network: BlockchainNetworkType): void {
    if (network) {
      this.saveCurrentBlockchain(network);
      this.getWalletSummary();
    }
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
