import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { selectPreviewProject, selectPreviewProjectPermissions } from '../../store/project.selectors';
import { PERMISSIONS } from '../../constants';
import { Project } from 'mosaico-project';

@Component({
  selector: 'app-project-overview',
  templateUrl: './project-overview.component.html',
  styleUrls: ['./project-overview.component.scss']
})
export class ProjectOverviewComponent implements OnInit, OnDestroy {
  public project: Project;
  public subs: SubSink = new SubSink();
  public canEdit = false;
  
  constructor(private store: Store) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProject).subscribe((res) => {
      this.project = res;
    });
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
  }
}
