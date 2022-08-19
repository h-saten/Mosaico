import { Component, Input, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, PatchModel } from 'mosaico-base';
import { Project, ProjectService } from 'mosaico-project';
import { Token } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { selectPreviewProject, selectProjectPreviewToken, selectUserSubscribedProject, setProjectTitle } from '../../../store';

@Component({
  selector: 'app-project-title',
  templateUrl: './project-title.component.html',
  styleUrls: ['./project-title.component.scss']
})
export class ProjectTitleComponent implements OnInit {

  @Input() canEdit: boolean;

  private subs: SubSink = new SubSink();
  currentProject: Project;
  projectId = '';
  editingTitle = false;

  token: Token;

  isUserSubscribeProject = false;

  leftSymbolToken: number | null = null;

  constructor(
    private store: Store,
    private projectService: ProjectService,
    private toastr: ToastrService,
    private errorHandler: ErrorHandlingService,
    private translateService: TranslateService,
  ) { }

  ngOnInit(): void {

    this.subs.sink = this.store.select(selectPreviewProject).subscribe((prj) => {
      if (prj) {
        this.currentProject = prj;
        this.projectId = this.currentProject.id;
      }
    });

    this.subs.sink = this.store.select(selectUserSubscribedProject).subscribe((res) => {
      this.isUserSubscribeProject = res;
    });

    this.subs.sink = this.store.select(selectProjectPreviewToken).subscribe((t) => {
      if(t){
        this.token = t;
      }
    });
  }

}
