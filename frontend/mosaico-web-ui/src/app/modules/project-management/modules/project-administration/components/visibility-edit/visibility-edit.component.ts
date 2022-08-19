import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { ErrorHandlingService, FormBase } from 'mosaico-base';
import { Project, ProjectService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { selectPreviewProject } from 'src/app/modules/project-management/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-visibility-edit',
  templateUrl: './visibility-edit.component.html',
  styleUrls: ['./visibility-edit.component.scss']
})
export class VisibilityEditComponent extends FormBase implements OnDestroy, OnInit {
  subs = new SubSink();
  currentProject: Project;
  projectId: string;
  subscribed = false;

  constructor(private store: Store, private service: ProjectService, private toastr: ToastrService, private errorHandler: ErrorHandlingService) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      isPublic: new FormControl(false),
    });
    this.subs.sink = this.store.select(selectPreviewProject).subscribe((prj) => {
      if (prj) {
        this.currentProject = prj;
        this.projectId = this.currentProject.id;
        this.form.get('isPublic')?.setValue(this.currentProject.isPublic);
        this.subscribeOnChange();
      }
    });
  }

  subscribeOnChange(): void {
    if(!this.subscribed) {
      this.form.get('isPublic').valueChanges.subscribe((res) => {
        if(res !== this.currentProject.isPublic) {
          this.updatePublicity();
        }
      });
    }
  }

  private updatePublicity(): void {
    this.subs.sink = this.service.updatePublicity(this.projectId, this.form.get('isPublic').value).subscribe((res) => {
      this.toastr.success('Visibility updated');
      this.currentProject.isPublic = this.form.get('isPublic').value;
    }, (error) => {
      this.errorHandler.handleErrorWithToastr(error);
    });
  }
}
