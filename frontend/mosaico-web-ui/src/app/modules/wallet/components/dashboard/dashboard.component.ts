import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ErrorHandlingService } from 'mosaico-base';
import { GetProjectStagesResponse, ProjectService } from 'mosaico-project';
import { Token, TokenDistributionService, TokenService } from 'mosaico-wallet';
import { zip } from 'rxjs';
import { SubSink } from 'subsink';
import { TokenDashboardService } from '../../services/token-dashboard.service';
import { selectToken } from '../../store';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  dataLoaded = false;
  service: TokenDashboardService;
  token: Token;

  constructor(private store: Store, private tokenService: TokenDistributionService, private errorHandler: ErrorHandlingService,
    private projectService: ProjectService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectToken).subscribe((res) => {
      if (res && (!this.dataLoaded || !this.service)) {
        this.token = res;
        this.service = new TokenDashboardService(res);
        this.loadData(res);
      }
    });
  }

  loadData(token: Token): void {
    if (token) {
      const projectStageQueries = token?.projects?.map((p) => this.projectService.getProjectStages(p.id));
      token?.projects?.forEach((p) => this.service.setStages(p.id, []));
      this.subs.sink = zip([this.tokenService.getTokenDistribution(token.id), ...projectStageQueries]).subscribe((responses) => {
        this.service.tokenDistribution = responses[0]?.data;
        for (let i = 1; i < responses.length; i++) {
          const stages = (responses[i]?.data as GetProjectStagesResponse)?.stages;
          if (stages && stages.length > 0) {
            const projectId = stages[0]?.projectId;
            if (projectId && projectId.length > 0) {
              this.service.setStages(projectId, stages);
            }
          }
        }
        this.service.recalculateReserveTokenAmount();
        this.service.calculateOverallPercentage();
        this.dataLoaded = true;
      }, (error) => this.errorHandler.handleErrorWithToastr(error));
    }
  }

  onRefreshRequested(res: any): void {
    if (res === true) {
      this.dataLoaded = false;
      this.loadData(this.token);
    }
  }

}
