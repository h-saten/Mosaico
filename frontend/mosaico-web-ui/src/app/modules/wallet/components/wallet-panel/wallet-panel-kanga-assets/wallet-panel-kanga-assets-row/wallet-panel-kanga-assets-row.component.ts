import {Component, Input, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {KangaAsset} from 'mosaico-wallet';

@Component({
  selector: 'app-wallet-panel-kanga-assets-row',
  templateUrl: './wallet-panel-kanga-assets-row.component.html',
  styleUrls: ['./wallet-panel-kanga-assets-row.component.scss']
})
export class WalletPanelKangaAssetRowComponent implements OnInit {
  networks: any;

  @Input() token: KangaAsset;

  constructor(private store: Store) { }

  ngOnInit(): void {}

}
