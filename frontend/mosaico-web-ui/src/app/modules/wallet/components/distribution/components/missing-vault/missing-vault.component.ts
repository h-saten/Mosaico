import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Token } from 'mosaico-wallet';

@Component({
  selector: 'app-missing-vault',
  templateUrl: './missing-vault.component.html',
  styleUrls: ['./missing-vault.component.scss']
})
export class MissingVaultComponent implements OnInit {
  @Input() token: Token;
  @Output() vaultDeployed = new EventEmitter<boolean>(false);

  constructor() { }

  ngOnInit(): void {
  }

  onDeployed(res: boolean): void {
    if(res === true){
      this.vaultDeployed.emit(true);
    }
  }

}
