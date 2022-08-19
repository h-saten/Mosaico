import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ErrorHandlingService } from 'mosaico-base';
import { Project, ProjectService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { selectPreviewProject } from 'src/app/modules/project-management/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-edit-project-fee',
  templateUrl: './edit-project-fee.component.html',
  styleUrls: ['./edit-project-fee.component.scss']
})
export class EditProjectFeeComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  currentProject: Project;
  projectId: string;
  currentProjectFee: number = 7;

  constructor(private store: Store, private projectService: ProjectService,
    private toastr: ToastrService, private errorHandler: ErrorHandlingService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProject).subscribe((prj) => {
      if (prj) {
        this.currentProject = prj;
        this.projectId = this.currentProject.id;
        this.loadProjectFee();
      }
    });
  }

  save(): void {
    this.subs.sink = this.projectService.updateProjectFee(this.projectId, this.currentProjectFee).subscribe((res) => {
      this.toastr.success('Fee was updated');
    }, (error) => {this.errorHandler.handleErrorWithToastr(error);});
  }

  loadProjectFee(): void {
    this.subs.sink = this.projectService.getProjectFee(this.projectId).subscribe((res) => {
      this.currentProjectFee = res?.data;
    }, (error) => this.errorHandler.handleErrorWithToastr(error)); 
  }

}
