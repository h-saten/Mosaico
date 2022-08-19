import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-exchange',
  templateUrl: './exchange.component.html',
  styleUrls: ['./exchange.component.scss']
})
export class ExchangeComponent implements OnInit {

  // for example
  mos = [
    {
      name: 'MOS',
      key: 1,
      logoUrl: '/assets/media/logos/mos-token.png',
      disabled: false
    },
    {
      name: 'USDT',
      key: 2,
      logoUrl: '/assets/media/icons/currencies/usdt.png',
      disabled: false
    },
  ];

  // for example
  token = [
    {
      name: 'MOS',
      key: 1,
      logoUrl: '/assets/media/logos/mos-token.png',
      disabled: false
    },
    {
      name: 'USDT',
      key: 2,
      logoUrl: '/assets/media/icons/currencies/usdt.png',
      disabled: false
    },
  ];

  startToExchange: boolean = true;
  isExchanged: boolean = false;
  isApproved: boolean = false;
  isConfirmed: boolean = false;
  isInProcess: boolean = false;
  isFailed: boolean = false;
  isSuccessed: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  onExchange() {
    this.isExchanged = true;
  }

  onCancelExchange() {
    this.isExchanged = false;
  }

  onApprove() {
    this.isApproved = true;
    this.isExchanged = false;
    this.startToExchange = false;
  }

  onCancelConfirm() {
    this.isApproved = false;
    this.isExchanged = true; 
    this.startToExchange = true;
  }

  onConfirm() {
    this.isConfirmed = true;
    this.isApproved = false;
    this.isExchanged = false; 
  }

  onContinueConfirm() {
    this.isInProcess = true;
    this.isConfirmed = false;
  }

}
