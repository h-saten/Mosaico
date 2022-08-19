import {Component, Input, OnInit} from '@angular/core';
import {SubSink} from "subsink";
import {baseConfiguration} from "./visits-chart.config";
import {StatisticsService, VisitsDto} from "mosaico-statistics";
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-visits-chart',
  templateUrl: './visits-chart.component.html',
  styleUrls: []
})
export class VisitsChartComponent implements OnInit {

  private subs = new SubSink();

  @Input() projectId: string;

  visitStatistics: any;

  chartConfigurationLoaded = false;

  constructor(
    private statisticsService: StatisticsService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.fetchStatistics(this.projectId);
  }

  private fetchStatistics(projectId: string) {
    this.subs.sink = this.statisticsService
      .dailyVisitStatistics(projectId)
      .subscribe(response => {
        if (response && response.data) {
          this.prepareChartConfiguration(response.data.tokenPageVisits, response.data.fundPageVisits);
        }
      });
  }

  private prepareChartConfiguration(tokenPageVisits: VisitsDto[], fundPageVisits: VisitsDto[]): void {
    const chartConfiguration = baseConfiguration;
    chartConfiguration.series[0] = {
      name: this.translate.instant('DASHBOARD.VISITS_CHART.token_page_label'),
      data: tokenPageVisits.map(x => x.amount.toString())
    };
    chartConfiguration.series[1] = {
      name: this.translate.instant('DASHBOARD.VISITS_CHART.fund_page_label'),
      data: fundPageVisits.map(x => x.amount.toString())
    };
    chartConfiguration.xaxis.categories = tokenPageVisits.map(x => x.date);
    this.visitStatistics = chartConfiguration;
    this.chartConfigurationLoaded = true;
  }
}
