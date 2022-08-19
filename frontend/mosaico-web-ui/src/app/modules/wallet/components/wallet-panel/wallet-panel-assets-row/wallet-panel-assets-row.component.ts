import { Component, Input, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TokenBalance } from 'mosaico-wallet';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';

@Component({
  selector: 'app-wallet-panel-assets-row',
  templateUrl: './wallet-panel-assets-row.component.html',
  styleUrls: ['./wallet-panel-assets-row.component.scss']
})
export class WalletPanelAssetRowComponent implements OnInit {
  networks: any;

  @Input() token: TokenBalance;

  constructor(private store: Store) { }

  ngOnInit(): void {
    this.store.select(selectCurrentActiveBlockchains).subscribe((networks) => {
      this.networks = Object.fromEntries(networks.map((n) => {
        return [n.name, n.logoUrl];
      }));
    });
  }

}
