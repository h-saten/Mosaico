import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-wallet-overview',
  templateUrl: './wallet-overview.component.html',
  styleUrls: ['./wallet-overview.component.scss']
})

export class WalletOverviewComponent implements OnInit, OnDestroy {

  sub: SubSink = new SubSink();
  isLoaded: boolean = true;

  constructor() {}

  ngOnInit(): void {
      
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
