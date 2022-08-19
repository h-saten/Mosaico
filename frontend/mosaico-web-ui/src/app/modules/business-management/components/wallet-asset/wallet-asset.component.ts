import { Component, Input, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { CompanyTokenBalance } from 'mosaico-wallet';
import { selectCurrentActiveBlockchains } from '../../../../store/selectors';

@Component({
  selector: 'app-wallet-asset',
  templateUrl: './wallet-asset.component.html',
  styleUrls: ['./wallet-asset.component.scss']
})
export class WalletAssetComponent implements OnInit {
  networks: any;
  @Input() token: CompanyTokenBalance;

  constructor(private store: Store) { }

  ngOnInit(): void {
    this.store.select(selectCurrentActiveBlockchains).subscribe((networks) => {
      this.networks = Object.fromEntries(networks.map((n) => {
        return [n.name, n.logoUrl];
      }));
    });
  }
}
