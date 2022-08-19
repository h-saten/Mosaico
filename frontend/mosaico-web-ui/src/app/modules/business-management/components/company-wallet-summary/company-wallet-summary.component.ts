import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { BlockchainNetworkType } from 'mosaico-base';
import { Blockchain, CompanyWalletBalance, CompanyWalletService, RampUserKYC, TransakUserKYC } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { selectCompanyPreview, selectCompanyWalletBalance } from '../../store/business.selectors';
import { selectBlockchain, selectCurrentActiveBlockchains } from '../../../../store/selectors';
import { setCurrentBlockchain } from 'src/app/store/actions';
import { setCompanyWallet, setCompanyWalletBalance } from '../../store';
import { take } from 'rxjs/operators';
import { selectUserInformation } from 'src/app/modules/user-management/store';

@Component({
  selector: 'app-company-wallet-summary',
  templateUrl: './company-wallet-summary.component.html',
  styleUrls: ['./company-wallet-summary.component.scss']
})
export class CompanyWalletSummaryComponent implements OnInit, OnDestroy {
  public walletBalance: CompanyWalletBalance;
  subs = new SubSink();
  selectedNetwork: BlockchainNetworkType;
  walletAddress: string;
  networks: Blockchain[] = [];
  isLoading = true;
  companyId: string;
  transakUserKyc: TransakUserKYC;
  rampUserKyc: RampUserKYC;
  userId: string;
  
  constructor(
    private store: Store,
    private companyWalletService: CompanyWalletService,
    private toastr: ToastrService,
    private translateService: TranslateService
  ) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectCompanyWalletBalance).subscribe((res) => {
      if (res) {
        this.walletAddress = res.address;
        this.walletBalance = res;
        this.isLoading = false;
      }
    });
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((res) => {
      this.networks = res;
    });
    this.subs.sink = this.store.select(selectCompanyPreview).pipe(take(1)).subscribe((c) => {
      this.companyId = c?.id;
      this.selectedNetwork = c?.network;
    });
    this.subs.sink = this.store.select(selectUserInformation).subscribe((res) => {
      if (res?.id) {
        this.userId = res.id;
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
  }

  getCompanyWalletSummary(network: BlockchainNetworkType): void {
    if (this.companyId && !this.isLoading) {
      this.isLoading = true;
      this.subs.sink = this.companyWalletService.getCompanyWalletTokens(this.companyId).subscribe((res) => {
        if (res && res.data) {
          this.saveCompanyWalletInStore(res.data, network);
          this.saveCurrentBlockchain(network);
        }
        this.isLoading = false;
      }, (error) => { this.isLoading = false; });
    }
  }

  onNetworkChanged(network: BlockchainNetworkType): void {
    if (network) {
      this.getCompanyWalletSummary(network);
    }
  }

  saveCompanyWalletInStore(balance: CompanyWalletBalance, network: BlockchainNetworkType): void {
    this.selectedNetwork = network;
    this.store.dispatch(setCompanyWallet({ network, address: balance.address }));
    this.store.dispatch(setCompanyWalletBalance(balance));
  }

  saveCurrentBlockchain(network: BlockchainNetworkType): void {
    if (network) {
      this.store.dispatch(setCurrentBlockchain({ blockchain: network }));
    }
  }

  onCopied(): void {
    this.subs.sink = this.translateService.get('COMPANY_WALLET.SUMMARY.MESSAGES.COPIED').subscribe((t) => {
      this.toastr.success(t);
    });
  }
}
