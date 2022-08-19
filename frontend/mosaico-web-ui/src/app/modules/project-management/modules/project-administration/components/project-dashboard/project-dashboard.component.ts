import {Component, OnDestroy, OnInit} from '@angular/core';
import {StatisticsService} from "mosaico-statistics";
import {SubSink} from "subsink";
import {selectProjectPreview} from "../../../../store";
import {Store} from "@ngrx/store";
import {Project} from "mosaico-project";

@Component({
  selector: 'app-project-dashboard',
  templateUrl: './project-dashboard.component.html',
  styleUrls: ['./project-dashboard.component.scss']
})
export class ProjectDashboardComponent implements OnInit, OnDestroy {

  private subs = new SubSink();
  projectId: string;
  project: Project;

  constructor(private statisticsService: StatisticsService, private store: Store) {
  }

  ngOnInit(): void {
    this.getProject();

  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private getProject(): void {
    this.store.select(selectProjectPreview).subscribe((res) => {
      if (res && res.project) {
        this.projectId = res.project.id;
        this.project = res.project;
      }
    });
  }

}
