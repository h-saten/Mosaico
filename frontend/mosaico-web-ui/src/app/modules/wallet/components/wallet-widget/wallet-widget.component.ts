import { Component, Input } from '@angular/core';
import { TokenBalance } from 'mosaico-wallet';

@Component({
  selector: 'app-wallet-widget',
  templateUrl: './wallet-widget.component.html',
})
export class WalletWidgetComponent {
  @Input() adr: string;
  @Input() color: string = 'primary';
  @Input() balance: TokenBalance[] = [];
  @Input() nativeBalance: number = 0;

  constructor() {}

  stake() {

  }
}
