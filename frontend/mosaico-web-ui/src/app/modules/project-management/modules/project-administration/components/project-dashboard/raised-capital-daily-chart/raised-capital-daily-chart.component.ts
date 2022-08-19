import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {SubSink} from "subsink";
import {RaisedCapitalDto, StatisticsService} from "mosaico-statistics";
import {TranslateService} from "@ngx-translate/core";
import {baseConfiguration} from "./raised-capital-daily-chart.config";
import moment from 'moment';
import {ApexOptions, ChartComponent} from "ng-apexcharts";

@Component({
  selector: 'app-raised-capital-daily-chart',
  templateUrl: './raised-capital-daily-chart.component.html',
  styleUrls: []
})
export class RaisedCapitalDailyChartComponent implements OnInit {

  @ViewChild("chart", { static: false }) chart: ChartComponent;

  private subs = new SubSink();

  @Input() projectId: string;

  chartConfigurationLoaded = false;
  dailyTransactions: ApexOptions;

  monthAgo = 0;
  currentDate: string;

  constructor(
    private statisticsService: StatisticsService,
    private translate: TranslateService
  ) {
    this.loadChartConfig();
  }

  ngOnInit(): void {
    this.fetchStatistics(this.projectId, this.monthAgo);
    this.updateMonthSelectors();
  }

  private loadChartConfig() {
    this.dailyTransactions = baseConfiguration;
    this.dailyTransactions.yaxis = {
      title: {
        text: this.translate.instant('DASHBOARD.DAILY_CAPITAL.chart_label')
      }
    };
    this.dailyTransactions.series = [{
      name: this.translate.instant('DASHBOARD.DAILY_CAPITAL.chart_label') + " (USDT)",
      data: []
    }];
  }

  private fetchStatistics(projectId: string, monthAgo = 0): void {
    this.subs.sink = this.statisticsService
      .dailyRaisedCapital(projectId, monthAgo)
      .subscribe(response => {
        if (response && response.data) {
          this.prepareChartData(response.data.statistics);
        }
      });
  }

  private updateMonthSelectors() {
    const currentDate = moment().add(-this.monthAgo, 'months').toDate();
    this.currentDate = currentDate.toLocaleString(this.translate.currentLang,{month:'long', year:'numeric'});
  }

  private prepareChartData(tokenPageVisits: RaisedCapitalDto[]): void {
    const chartConfiguration = baseConfiguration;
    const usdtValues = tokenPageVisits.map(x => x.usdtAmount);
    const dates = tokenPageVisits.map(x => x.date);
    const seriesData = [];
    dates.forEach((value, index) => {
      seriesData.push([value, usdtValues[index]]);
    });
    const series = {
      name: this.translate.instant('DASHBOARD.DAILY_CAPITAL.chart_label') + " (USDT)",
      data: seriesData
    };
    chartConfiguration.series = [series];
    this.dailyTransactions = chartConfiguration;

    this.chartConfigurationLoaded = true;
  }

  movePreviousMonth(): void {
    this.monthAgo += 1;
    this.updateMonthSelectors();
    this.fetchStatistics(this.projectId, this.monthAgo);
  }

  moveNextMonth(): void {
    if (this.monthAgo === 0) {
      return;
    }
    this.monthAgo -= 1;
    this.updateMonthSelectors();
    this.fetchStatistics(this.projectId, this.monthAgo);
  }

}
