import {Component, Input, OnInit} from '@angular/core';
import {SubSink} from "subsink";
import {StatisticsService, StatisticsSummaryResponse} from "mosaico-statistics";
import {Project, Stage} from "mosaico-project";

@Component({
  selector: 'app-statistics-tiles',
  templateUrl: './statistics-tiles.component.html',
  styleUrls: []
})
export class StatisticsTilesComponent implements OnInit {
  private subs = new SubSink();

  @Input() project: Project;

  statistics: StatisticsSummaryResponse;

  saleProgressPercentage: number;

  constructor(
    private statisticsService: StatisticsService
  ) {}

  ngOnInit(): void {
    this.fetchStatistics(this.project);
  }

  private fetchStatistics(project: Project): void {
    this.subs.sink = this.statisticsService
      .summary(project.id)
      .subscribe(response => {
        if (response && response.data) {
          this.statistics = response.data;
          this.saleProgressPercentage = this.calculateSaleProgress(response.data.raisedFunds, project.stages);
        }
      });
  }

  private calculateSaleProgress(raisedFunds: number, stages: Stage[]): number {
    if (raisedFunds === 0) return 0;
    let softCapsSum = 0;
    stages.forEach(stage => {
      const stageSoftCap = stage.tokensSupply * stage.tokenPrice * 0.1;
      softCapsSum += stageSoftCap;
    });
    return raisedFunds * 100 / softCapsSum;
  }
}
