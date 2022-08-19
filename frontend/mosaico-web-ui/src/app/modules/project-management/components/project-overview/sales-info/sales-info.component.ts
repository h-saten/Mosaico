import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Stage } from 'mosaico-project';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../../constants';
import { selectPreviewProjectPermissions } from '../../../store';
import { selectPreviewProjectActiveStage, selectProjectPreview } from '../../../store/project.selectors';

@Component({
  selector: 'app-sales-info',
  templateUrl: './sales-info.component.html',
  styleUrls: ['./sales-info.component.scss']
})
export class SalesInfoComponent implements OnInit {
  subs = new SubSink();
  stage: Stage;
  canEdit = false;
  projectId: string;
  softCap?: number;
  hardCap?: number;
  totalSupply?: number;
  tokenSymbol?: string;

  constructor(private store: Store) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((res) => {
      this.projectId = res?.project?.id;
      this.stage = res?.project?.activeStage;
      this.softCap = res?.project?.softCap;
      this.hardCap = res?.project?.hardCap;
      this.totalSupply = res.token?.totalSupply;
      this.tokenSymbol = res.token?.symbol;
    });
  }

  enableEditing(): void {

  }
}
