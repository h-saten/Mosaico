import {Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges} from '@angular/core';
import {selectPreviewProject, setCurrentProjectActiveStage} from "../../store";
import {SubSink} from "subsink";
import {Store} from "@ngrx/store";
import { Project, ProjectService, Stage } from 'mosaico-project';

@Component({
  selector: 'app-project-stage-progress',
  templateUrl: './project-stage-progress.component.html',
  styleUrls: ['./project-stage-progress.component.scss']
})
export class ProjectStageProgressComponent implements OnInit, OnChanges, OnDestroy {

  private sub: SubSink = new SubSink();

  @Input() projectId: string;

  project: Project;
  currentStage: Stage;
  soldTokens: number;
  saleProgress = 0;

  constructor(
    private projectService: ProjectService,
    private store: Store) {
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    if (this.projectId) {
      this.getProjectSaleReport();
      this.getProject();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.projectId) {
      this.ngOnInit();
    }
  }

  getProjectSaleReport(): void {
    this.sub.sink = this.projectService.getProjectSaleReport(this.projectId).subscribe((res) => {
      if (res && res.data) {
        this.soldTokens = res.data.soldTokens;
        if (res.data.stage) {
          this.currentStage = res.data.stage;
          this.saleProgress = Math.floor(this.soldTokens * 100 / this.currentStage.tokensSupply);
          this.store.dispatch(setCurrentProjectActiveStage({ activeStage: res.data.stage }));
        }
      }
    });
  }

  private getProject(): void {
    this.sub.sink = this.store.select(selectPreviewProject)
      .subscribe((response: Project) => {
        if (response) {
          this.project = response;
        }
      });
  }

}
