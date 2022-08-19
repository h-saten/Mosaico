import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { Observable, of, zip } from 'rxjs';

@Component({
  selector: 'app-wallet-summary',
  templateUrl: './wallet-summary.component.html',
  styleUrls: ['./wallet-summary.component.scss']
})

export class WalletSummaryComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();
  constructor() {}

  ngOnInit(): void {
      
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
