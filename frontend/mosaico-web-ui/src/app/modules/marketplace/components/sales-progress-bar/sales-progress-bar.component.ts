import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-sales-progress-bar',
  templateUrl: './sales-progress-bar.component.html',
  styleUrls: ['./sales-progress-bar.component.scss']
})
export class SalesProgressBarComponent implements OnInit {

  @Input() softCapAmount: number;
  @Input() hardCapAmount: number;
  @Input() raisedCapitalInUSD: number;
  @Input() raisedCapital: number;
  @Input() raisedCapitalPercentage: number;
  raisedCapitalPercentageForDisplay = 0;

  @Input() numberOfBuyers: number;

  @Input() tokenSymbol?: string;

  colorOfProgressBar = '';

  constructor() { }

  ngOnInit(): void {
    this.raisedCapitalPercentageForDisplay = this.raisedCapitalPercentage;

    if (this.raisedCapitalPercentage > 100) {
      this.raisedCapitalPercentage = 100;
    }
  }
}
