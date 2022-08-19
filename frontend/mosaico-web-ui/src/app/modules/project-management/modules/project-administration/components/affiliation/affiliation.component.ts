import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';
import { Affiliation, AffiliationPartner, AffiliationService } from 'mosaico-project';
import { Token } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { LazyLoadEvent } from 'primeng/api';
import { selectProjectPreview, selectProjectPreviewToken } from 'src/app/modules/project-management/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-affiliation',
  templateUrl: './affiliation.component.html',
  styleUrls: ['./affiliation.component.scss']
})
export class AffiliationComponent extends FormBase implements OnInit, OnDestroy {
  showFilters = false;
  isLoading: boolean = true;
  sub: SubSink = new SubSink();
  partners: AffiliationPartner[] = [];
  projectId: string;
  token: Token;
  expandedRow = -1;
  currentSkip: number = 0;
  currentTake: number = 10;
  page = 1;
  pageSize = 10;
  totalRecords: number;
  projectName: string;
  affiliation: Affiliation;

  constructor(
    private store: Store,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private affiliationService: AffiliationService,
    private errorHandler: ErrorHandlingService
  ) {
    super();
   }

  ngOnInit(): void {
    this.createForm();
    this.getProjectId();
  }

  createForm(): void {
    this.form = new FormGroup({
      isEnabled: new FormControl(false),
      rewardPercentage: new FormControl(0),
      rewardPool: new FormControl(0),
      includeAll: new FormControl(false),
      everybodyCanParticipate: new FormControl(false),
      partnerShouldBeInvestor: new FormControl(false)
    });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  getAffiliation(): void {
    this.sub.sink = this.affiliationService.getAffiliation(this.projectId).subscribe((res) => {
      this.affiliation = res?.data;
      this.form.setValue({
        isEnabled: this.affiliation?.isEnabled,
        rewardPercentage: this.affiliation?.rewardPercentage,
        rewardPool: this.affiliation?.rewardPool,
        includeAll: this.affiliation?.includeAll,
        everybodyCanParticipate: this.affiliation?.everybodyCanParticipate,
        partnerShouldBeInvestor: this.affiliation?.partnerShouldBeInvestor,
      });
    });
  }


  fetchPartners(event: LazyLoadEvent): void {
    this.isLoading = true;
    this.currentSkip = event.first;
    this.currentTake = event.rows;
    this.reloadPartners();
  }

  applyFilters(): void {
    this.currentSkip = 0;
    this.currentTake = 10;
    this.reloadPartners();
  }

  private reloadPartners(): void {
    this.sub.sink = this.affiliationService.getPartners(this.projectId, this.currentSkip, this.currentTake)
      .subscribe((res) => {
        this.partners = res?.data?.entities;
        this.totalRecords = res?.data?.total;
        this.isLoading = false;
    }, (error) => { this.errorHandler.handleErrorWithToastr(error); });
  }

  getProjectId(): void {
    this.sub.sink = this.store.select(selectProjectPreview).subscribe((project) => {
      if (project?.project?.id) {
        this.projectId = project.project?.id;
        this.projectName = project.project?.title;
        this.getAffiliation();
      }
    });
    this.sub.sink = this.store.select(selectProjectPreviewToken).subscribe((res) => {
      this.token = res;
    });
  }

  // onExportCsv() {
  //   var url = URL.createObjectURL(res?.body);
  //     var downloadLink = document.createElement("a");
  //     downloadLink.href = url;
  //     const now = new Date();
  //     downloadLink.download = `${this.projectName}_${now.getFullYear()}-${now.getMonth()}-${now.getDay()}-${now.getHours()}${now.getMinutes()}.csv`;

  //     document.body.appendChild(downloadLink);
  //     downloadLink.click();
  //     document.body.removeChild(downloadLink);
  // }

  toggleRow(index: number): void {
    if(index !== this.expandedRow) {
      this.expandedRow = index;
    }
    else {
      this.expandedRow = -1;
    }
  }

  save(): void {
    if(validateForm(this.form)) {
      let command = this.form.getRawValue();
      this.sub.sink = this.affiliationService.updateProjectAffiliation(this.projectId, command).subscribe((res) => {
        this.toastr.success('Affiliation updated');
      }, (error) => this.errorHandler.handleErrorWithToastr(error));
    }
    else{
      this.toastr.error('Form has invalid values');
    }
  }

  onPartnerEnable(id: string): void {
    if(id) {
      this.sub.sink = this.affiliationService.enablePartner(this.projectId, id).subscribe((res) => {
        this.toastr.success('Partner enabled');
        this.applyFilters();
      }, (error) => this.errorHandler.handleErrorWithToastr(error));
    }
  }

  onPartnerDisable(id: string): void {
    if(id) {
      this.sub.sink = this.affiliationService.disablePartner(this.projectId, id).subscribe((res) => {
        this.toastr.success('Partner disabled');
        this.applyFilters();
      }, (error) => this.errorHandler.handleErrorWithToastr(error));
    }
  }
}
