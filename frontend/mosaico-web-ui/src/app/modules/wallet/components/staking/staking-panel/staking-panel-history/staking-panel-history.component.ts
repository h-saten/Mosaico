import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-staking-panel-history',
  templateUrl: './staking-panel-history.component.html',
  styleUrls: ['./staking-panel-history.component.scss']
})
export class StakingPanelHistoryComponent implements OnInit {
  isLoading: boolean = false;
  stakings = [
    // {
    //   tokenName: "MOS",
    //   status: "Active",
    //   staked: {
    //     mos: "5239.00 MOS",
    //     amount: "258,00 USD",
    //   },
    //   rewarded: {
    //     mos: "21602.00 MOS",
    //     amount: "2567,00 USD",
    //   },
    //   apr: "12 %",
    //   startDate: "25.01.2022",
    //   endDate: "25.07.2022"
    // },
    // {
    //   tokenName: "MOS",
    //   status: null,
    //   staked: {
    //     mos: "5239.00 MOS",
    //     amount: "258,00 USD",
    //   },
    //   rewarded: {
    //     mos: "21602.00 MOS",
    //     amount: "2567,00 USD",
    //   },
    //   apr: "12 %",
    //   startDate: "25.01.2022",
    //   endDate: "25.07.2022"
    // }
  ];

  constructor() { }

  ngOnInit(): void {
  }
}
