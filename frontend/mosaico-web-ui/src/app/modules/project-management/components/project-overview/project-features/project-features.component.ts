import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Token } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../../constants';
import { selectPreviewProjectPermissions, selectProjectPreviewToken } from '../../../store';
import { selectProjectPackages } from '../../../store/project.selectors';

@Component({
  selector: 'app-project-features',
  templateUrl: './project-features.component.html',
  styleUrls: ['./project-features.component.scss']
})
export class ProjectFeaturesComponent implements OnInit {
  subs: SubSink = new SubSink();
  canEdit: boolean;
  token: Token;
  hasPackages = false;
  stakingStartDate?: Date;
  vestingStartDate?: string;

  constructor(private store: Store) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });

    this.subs.sink = this.store.select(selectProjectPreviewToken).subscribe((res) => {
      this.token = res;
      if(this.token?.stakings && this.token.stakings.length > 0) {
        const stakingDates = this.token.stakings.map((s) => new Date(s.startsAt).getTime());
        this.stakingStartDate = new Date(Math.min(...stakingDates));
      }
      this.vestingStartDate = this.token?.vesting?.startsAt;

    });

    this.subs.sink = this.store.select(selectProjectPackages).subscribe((res) => {
      this.hasPackages = res && res.length > 0;
    });
  }

}
