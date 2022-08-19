import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Stage } from 'mosaico-project';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../constants';
import { selectPreviewProjectPermissions, selectProjectPreview } from '../../store';

@Component({
  selector: 'app-project-stages',
  templateUrl: './project-stages.component.html',
  styleUrls: ['./project-stages.component.scss']
})
export class ProjectStagesComponent implements OnInit {
  canEdit = false;
  subs: SubSink = new SubSink();
  stages: Stage[] = [];
  constructor(private store: Store) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((project) => {
      if (project?.project?.stages) {
        this.stages = project.project.stages?.slice().sort((s1, s2) => s1.order - s2.order);
      }
    });
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
  }

}
