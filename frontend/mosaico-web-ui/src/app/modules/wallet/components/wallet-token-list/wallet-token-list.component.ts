import { Component, Input } from '@angular/core';
import { WalletBalance } from "mosaico-wallet";

@Component({
  selector: 'app-wallet-token-list',
  templateUrl: './wallet-token-list.component.html',
})
export class WalletTokenListComponent {
  @Input() balance: WalletBalance;

  constructor() { }
}
