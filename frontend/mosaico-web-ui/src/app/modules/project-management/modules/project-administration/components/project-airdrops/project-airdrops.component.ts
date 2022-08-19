import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ErrorHandlingService } from 'mosaico-base';
import { Airdrop, AirdropService } from 'mosaico-project';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { selectProjectPreview } from '../../../../store/project.selectors';

@Component({
  selector: 'app-project-airdrops',
  templateUrl: './project-airdrops.component.html',
  styleUrls: ['./project-airdrops.component.scss']
})
export class ProjectAirdropsComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  airdrops: Airdrop[] = [];
  projectId: string;
  tokenId: string;
  companyId: string;

  constructor(
    private toastr: ToastrService, 
    private store: Store,
    private translate: TranslateService,
    private service: AirdropService,
    private errorHandler: ErrorHandlingService
  ) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((p) => {
      if (p?.project?.id) {
        this.projectId = p.project.id;
        this.companyId = p.project.companyId;
        this.tokenId = p.project.tokenId;
        this.refreshData();
      }
    });
  }

  refreshData(): void {
    this.subs.sink = this.service.getProjectAirdrops(this.projectId).subscribe((response) => {
      this.airdrops = response.data;
    });
  }

  stopCampaign(a: Airdrop): void {
    if (a) {
      this.subs.sink = this.service.delete(this.projectId, a.id).subscribe((res) => {
        this.toastr.success(this.translate.instant('PROJECT_AIRDROPS.ALERTS.AIRDROP_WAS_SUSPENDED'));
        this.refreshData();
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); });
    }
  }

  onCopied(): void {
    this.toastr.success(this.translate.instant('PROJECT_AIRDROPS.ALERTS.URL_COPIED'));
  }

  getAirdropLink(a: Airdrop): string {
    const location = window.location.origin;
    return `${location}/airdrop/${a?.slug}`;
  }

}
