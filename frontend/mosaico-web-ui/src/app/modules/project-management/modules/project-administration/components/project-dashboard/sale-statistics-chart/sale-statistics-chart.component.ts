import {Component, Input, OnInit} from '@angular/core';
import {baseConfiguration} from "./sale-statistics-chart.config";
import {SubSink} from "subsink";
import {RaisedFundsByCurrencyDto, StatisticsService, TopInvestorDto} from "mosaico-statistics";
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-sale-statistics-chart',
  templateUrl: './sale-statistics-chart.component.html',
  styleUrls: []
})
export class SaleStatisticsChartComponent implements OnInit {

  private subs = new SubSink();

  @Input() projectId: string;

  investors: TopInvestorDto[];

  salesStatistics: any;
  chartConfigurationLoaded: boolean;

  constructor(
    private statisticsService: StatisticsService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.salesStatistics = baseConfiguration
    this.fetchSaleStatistics(this.projectId);
  }

  private fetchSaleStatistics(projectId: string) {
    this.subs.sink = this.statisticsService
      .raisedFundsByCurrency(projectId)
      .subscribe(response => {
        if (response && response.data) {
          this.prepareChartConfiguration(response.data.statistics);
        }
      });
  }

  private prepareChartConfiguration(statistics: RaisedFundsByCurrencyDto[]): void {
    const chartConfiguration = baseConfiguration;
    chartConfiguration.series = statistics.map(x => x.usdtAmount.toFixed(2));
    chartConfiguration.labels = statistics.map(x => x.currency);

    this.chartConfigurationLoaded = true;
  }
}
