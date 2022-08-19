import {Component, Input, OnInit} from '@angular/core';
import {SubSink} from "subsink";
import {StatisticsService, TopInvestorDto} from "mosaico-statistics";

@Component({
  selector: 'app-top-investors',
  templateUrl: './top-investors.component.html',
  styleUrls: []
})
export class TopInvestorsComponent implements OnInit {
  private subs = new SubSink();

  @Input() projectId: string;

  investors: TopInvestorDto[];

  constructor(
    private statisticsService: StatisticsService
  ) {}

  ngOnInit(): void {
    this.fetchTopInvestors(this.projectId);
  }

  private fetchTopInvestors(projectId: string) {
    this.subs.sink = this.statisticsService
      .topInvestors(projectId)
      .subscribe(response => {
        if (response && response.data) {
          this.investors = response.data.investors;
        }
      });
  }
}
