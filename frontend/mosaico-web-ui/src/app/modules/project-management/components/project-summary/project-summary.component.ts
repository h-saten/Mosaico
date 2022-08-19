import { selectUserSubscribedProject } from './../../store/project.selectors';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../constants';
import { selectPreviewProject, selectPreviewProjectPermissions, selectProjectCompany, selectProjectPreviewToken, selectProjectPage } from '../../store/project.selectors';
import { Page, Project, ProjectPageHubService, TokenPageService } from 'mosaico-project';
import { DEFAULT_MODAL_OPTIONS } from 'mosaico-base';
import { Token } from 'mosaico-wallet';
import { Company } from 'mosaico-dao';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { setProjectPage } from '../../store/project.actions';
import {
  ProjectCoverUploadComponent,
  EditMainColorsComponent
} from '../../modals';
import { ViewportScroller } from '@angular/common';


@Component({
  selector: 'app-project-summary',
  templateUrl: './project-summary.component.html',
  styleUrls: ['./project-summary.component.scss']
})
export class ProjectSummaryComponent implements OnInit, OnDestroy {
  private subs: SubSink = new SubSink();
  currentProject: Project;
  projectId = '';
  canEdit = false;
  canBuy = false;
  loading = false;
  statusProject: string | undefined = '';
  startDate: Date;
  endDate: Date;
  tokenPrice = 0;
  tokenSymbol = '';
  softCapAmount = 0;
  hardCapAmount = 0;
  raisedCapital = 0;
  raisedCapitalPercentage = 0;
  numberOfBuyers = 0;
  token: Token;
  page: Page;
  company: Company;
  backgroundImage = '';
  isUserSubscribeProject = false;
  showScrollingButton = false;

  constructor(
    private store: Store,
    private modalService: NgbModal,
    private tokenPageService: TokenPageService,
    private hub: ProjectPageHubService
  ) { }

  async ngOnDestroy(): Promise<void> {
    this.subs.unsubscribe();
    await this.hub.removeListenerAsync();
  }

  @HostListener('window:scroll', ['$event']) // for window scroll events
  onScroll(event) {
    this.showScrollingButton = window.pageYOffset >= 600;
  }

  private async startListeners(): Promise<void> {
    await this.hub.startConnectionAsync();
    this.hub.addListener();
    this.subs.sink = this.hub.coverUpdated$.subscribe((coverUrl) => {
      if (this.page) {
        this.page.coverUrl = coverUrl;
        this.backgroundImage = this.page.coverUrl && this.page.coverUrl.length > 0 ? this.page.coverUrl : '/assets/media/tokenpage/default_header.png';
      }
    });
    this.subs.sink = this.hub.logoUpdated$.subscribe((logoUrl) => {
      if (this.currentProject) {
        this.currentProject.logoUrl = logoUrl;
      }
    });

  }

  ngOnInit(): void {
    //this.startListeners();
    this.subs.sink = this.store.select(selectProjectCompany).subscribe((res) => {
      if (res) {
        this.company = res;
      }
    });

    this.subs.sink = this.store.select(selectProjectPage).subscribe((res) => {
      if (res) {
        this.page = res;
        this.backgroundImage = this.page.coverUrl && this.page.coverUrl.length > 0 ? this.page.coverUrl : '/assets/media/tokenpage/default_header.png';
      }
    });

    this.subs.sink = this.store.select(selectPreviewProject).subscribe((prj) => {
      if (prj) {
        this.currentProject = prj;
        this.projectId = this.currentProject.id;

        if (this.currentProject.activeStage) {

          this.statusProject = this.currentProject.activeStage.status;
          this.tokenPrice = this.currentProject.activeStage.tokenPrice;

          this.startDate = new Date(this.currentProject.activeStage.startDate);
          this.endDate = new Date(this.currentProject.activeStage.endDate);

        } else {
          this.statusProject = this.currentProject.status;
          // status Approved - to be changed
        }

        this.softCapAmount = this.currentProject.softCapInUserCurrency;
        this.hardCapAmount = this.currentProject.hardCapInUserCurrency;
        this.raisedCapital = this.currentProject.raisedCapital;
        this.raisedCapitalPercentage = this.currentProject.raisedCapitalPercentage;
        this.numberOfBuyers = this.currentProject.numberOfBuyers;
      }
    });

    this.subs.sink = this.store.select(selectUserSubscribedProject).subscribe((res) => {
      this.isUserSubscribeProject = res;
    });

    this.subs.sink = this.store.select(selectProjectPreviewToken).subscribe((t) => {
      if (t) {
        this.token = t;
        this.tokenSymbol = this.token.symbol;
      }
    });

    this.getProjectPermissions();
  }

  openMainColorsModal(): void {
    const modalRef = this.modalService.open(EditMainColorsComponent, DEFAULT_MODAL_OPTIONS);
    this.subs.sink = modalRef.closed.subscribe(() => {
      setTimeout(() => {
        this.reloadPage();
      }, 1000);
    });
  }

  openCoverEditingModal(): void {
    if (this.canEdit && this.page) {
      const modalRef = this.modalService.open(ProjectCoverUploadComponent, { "modalDialogClass": "page-into-modal" });
      modalRef.componentInstance.currentImgUrl = this.page.coverUrl;
      modalRef.componentInstance.pageId = this.page.id;
    }
  }

  private reloadPage(): void {
    if (this.page) {
      this.subs.sink = this.tokenPageService.getPage(this.page.id).subscribe((response) => {
        if (response && response.data) {
          this.store.dispatch(setProjectPage({ page: response.data }));
        }
      });
    }
  }

  private getProjectPermissions(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canBuy = res && res[PERMISSIONS.CAN_PURCHASE] === true;
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
  }
}
