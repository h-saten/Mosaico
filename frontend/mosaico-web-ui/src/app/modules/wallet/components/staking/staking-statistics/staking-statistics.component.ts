import { Component, OnInit, OnDestroy } from '@angular/core';
import { SubSink } from 'subsink';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexYAxis,
  ApexDataLabels,
  ApexTooltip,
  ApexStroke,
  ApexTheme,
  ApexLegend,
  ApexOptions
} from "ng-apexcharts";
import { StakingService, StakingStatisticsResponse } from 'mosaico-wallet';

export type ChartOption = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  dataLabels: ApexDataLabels;
  theme: ApexTheme;
  legend: ApexLegend;
};

@Component({
  selector: 'app-staking-statistics',
  templateUrl: './staking-statistics.component.html',
  styleUrls: ['./staking-statistics.component.scss']
})
export class StakingStatisticsComponent implements OnInit, OnDestroy {
  dailyTransactions: ApexOptions;
  subs = new SubSink();
  public diagramOptions: Partial<ChartOption>;
  stats: StakingStatisticsResponse;
  constructor(private stakingService: StakingService) {
    this.diagramOptions = {
      chart: {
        height: 260,
        type: "area"
      },
      dataLabels: {
        enabled: false
      },
      series: [
        {
          name: "Series 1",
          data: [45, 52, 38, 45, 19]
        }
      ],
      xaxis: {
        categories: [
          "01 Dec",
          "08 Dec",
          "16 Dec",
          "24 Dec",
          "31 Dec"
        ]
      }
    };
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.stakingService.getStakingStatistics().subscribe((res) => {
      this.stats = res?.data;
    });
  }

}
