import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { BlockchainNetworkType } from 'mosaico-base';
import { Blockchain, CompanyWalletBalance, CompanyWalletService } from 'mosaico-wallet';
import { setCurrentBlockchain } from 'src/app/store/actions';
import { selectBlockchain, selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';
import { BlockchainNetworks, selectCompanyPreview, setCompanyWallet, setCompanyWalletBalance } from '../../store';

@Component({
  selector: 'app-company-wallet',
  templateUrl: './company-wallet.component.html',
  styleUrls: ['./company-wallet.component.scss']
})
export class CompanyWalletComponent implements OnInit {
  private subs: SubSink = new SubSink();
  companyId: string;
  selectedNetwork: BlockchainNetworkType;
  networks: Blockchain[] = [];
  isLoaded = false;
  walletAddress: string;
  networksWalletAddresses: BlockchainNetworks[] = [];
  isInternallyLoading = false;
  isLoading = false;

  constructor(
    private store: Store,
    private companyWalletService: CompanyWalletService
  ) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectCompanyPreview).subscribe((res) => {
      if(res) {
        this.companyId = res.id;
        this.fetchCurrentBlockchainNetwork();
        this.selectedNetwork = res?.network;
        if(this.selectedNetwork){
          this.getCompanyWalletSummary(this.selectedNetwork);
        }
      }
    });
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((res) => {
      this.networks = res;
    });
  }

  private getCompanyWalletSummary(network: BlockchainNetworkType, force = false): void {
    if ((!this.isLoaded || force === true) && this.companyId && !this.isInternallyLoading && network) {
      this.isLoaded = true;
      if(this.companyId)
      {
        this.subs.sink = this.companyWalletService.getCompanyWalletTokens(this.companyId).subscribe((res) => {
          if (res && res.data) {
            this.isLoaded = true;
            this.saveCompanyWalletInStore(res.data, network);
            this.saveCurrentBlockchain(network);
          }
          this.isInternallyLoading = false;
        }, (error) => { this.isInternallyLoading = false, this.isLoading = false; });
      }
    }
  }

  saveCompanyWalletInStore(balance: CompanyWalletBalance, network: BlockchainNetworkType): void {
    this.selectedNetwork = network;
    this.store.dispatch(setCompanyWallet({ network, address: balance.address }));
    this.store.dispatch(setCompanyWalletBalance(balance));
  }

  saveCurrentBlockchain(network: BlockchainNetworkType): void {
    if(network){
      this.store.dispatch(setCurrentBlockchain({ blockchain: network }));
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private fetchCurrentBlockchainNetwork(): void {
    this.subs.sink = this.store.select(selectBlockchain).subscribe((network) => {
      if(network)
      {
        this.selectedNetwork = network;
        if (!this.selectedNetwork || this.selectedNetwork.length === 0) {
          this.selectedNetwork = 'Ethereum';
        }
      }
    });
  }
}
