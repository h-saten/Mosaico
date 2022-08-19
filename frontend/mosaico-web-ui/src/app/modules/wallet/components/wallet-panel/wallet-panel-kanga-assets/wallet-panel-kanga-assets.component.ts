import {Component, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {SubSink} from 'subsink';
import {KangaAsset, KangaWalletBalance, KangaWalletService, WalletService} from 'mosaico-wallet';
import {finalize} from 'rxjs/operators';
import {selectKangaAssets, setKangaWalletBalance} from "../../../store";

@Component({
  selector: 'app-wallet-panel-kanga-assets',
  templateUrl: './wallet-panel-kanga-assets.component.html',
  styleUrls: []
})

export class WalletPanelKangaAssetsComponent implements OnInit, OnDestroy {

  subs = new SubSink();
  isLoading = true;
  isLoaded = false;
  dataFetched = false;

  walletAssets?: KangaAsset[];

  constructor(private store: Store, private walletService: WalletService, private kangaWalletService: KangaWalletService) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectKangaAssets).subscribe(response => {
      if (response) {
        this.walletAssets = response;

        if (response.length === 0 && !this.dataFetched) {
          this.fetchUserKangaWalletBalance();
        } else {
          this.isLoaded = true;
          this.isLoading = false;
        }
      }
    });
  }

  private fetchUserKangaWalletBalance(): void {
    this.isLoaded = false;
    this.isLoading = true;

    this.subs.sink = this.subs.sink = this.kangaWalletService
      .getUserKangaBalance()
      .pipe(finalize(() => {
        this.isLoading = false;
        this.dataFetched = true;
      }))
      .subscribe(response => {
        this.store.dispatch(setKangaWalletBalance({kangaAssets: response.data.assets}));
        this.isLoaded = true;
      });
  }

  fetchAssets(force = false): void {
    if (this.walletAssets) {
      if (!this.isLoaded || force === true) {
        this.fetchUserKangaWalletBalance();
      }
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}
