import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService } from 'mosaico-base';
import { Project, ProjectService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../constants';
import { selectPreviewProjectPermissions, setCurrentProject } from '../../store';
import { selectProjectPreview } from '../../store/project.selectors';

@Component({
  selector: 'app-project-review-status',
  templateUrl: './project-review-status.component.html',
  styleUrls: ['./project-review-status.component.scss']
})
export class ProjectReviewStatusComponent implements OnInit, OnDestroy {
  currentProject: Project;
  projectId = '';
  canEdit = false;
  loading = false;
  subs = new SubSink();
  isLoaded = false;
  score: number;
  animatedValue = 0;
  errors: string[] = [];
  public isCollapsed = true;
  constructor(private store: Store, private projectService: ProjectService, private toastr: ToastrService, private translateService: TranslateService,
    private errorHandler: ErrorHandlingService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((prj) => {
      this.currentProject = prj?.project;
    });
    this.getProjectPermissions();
  }

  private getProjectPermissions(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS];
      if (this.canEdit === true) {
        this.loadScore();
      }
    });
  }

  loadScore(force = false): void {
    if ((!this.isLoaded || force === true) && this.currentProject) {
      this.subs.sink = this.projectService.getProjectScore(this.currentProject.id).subscribe((res) => {
        if (res && res.data) {
          this.score = res.data.score;
          this.errors = res.data.errors;
          this.animateValue(0, this.score, 1000);
        }
        this.isLoaded = true;
      });
    }
  }

  submitForReview(): void {
    if (this.currentProject && !this.loading) {
      this.loading = true;
      this.subs.sink = this.projectService.submitForReview(this.currentProject.id).subscribe((res) => {
        if (res && res.ok) {
          this.toastr.success('Project was submitted to review');
          this.subs.sink = this.projectService.getProject(this.currentProject.id).subscribe((prjResponse) => {
            if (prjResponse && prjResponse.data && prjResponse.data.project) {
              this.store.dispatch(setCurrentProject(prjResponse.data.project));
            }
          }, (error) => this.errorHandler.handleErrorWithToastr(error));
        }
        this.loading = false;
      }, (error) => {this.errorHandler.handleErrorWithToastr(error); this.loading = false; });
    }
  }

  animateValue(start: number, end: number, duration: number): void {
    let startTimestamp = null;
    const step = (timestamp) => {
      if (!startTimestamp){
        startTimestamp = timestamp;
      }
      const progress = Math.min((timestamp - startTimestamp) / duration, 1);
      this.animatedValue = Math.floor(progress * (end - start) + start);
      if (progress < 1) {
        window.requestAnimationFrame(step);
      }
    };
    window.requestAnimationFrame(step);
  }

}
