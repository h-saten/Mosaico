import {Component, OnDestroy, OnInit} from '@angular/core';
import { NgbNavChangeEvent } from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';
import {SubSink} from 'subsink';
import {selectUserInformation} from "../../../user-management/store";

@Component({
  selector: 'app-wallet-panel',
  templateUrl: './wallet-panel.component.html',
  styleUrls: ['./wallet-panel.component.scss']
})

export class WalletPanelComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();
  constructor(private store: Store) {}

  ngOnInit(): void {
    
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
