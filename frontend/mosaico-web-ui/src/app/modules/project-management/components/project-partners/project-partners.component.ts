import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { Partner, ProjectService } from 'mosaico-project';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../constants';
import { selectPreviewProjectPermissions, selectProjectPreview } from '../../store';
import { ProjectPartnerAddEditComponent } from './project-partner-add-edit/project-partner-add-edit.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-project-partners',
  templateUrl: './project-partners.component.html',
  styleUrls: ['./project-partners.component.scss']
})
export class ProjectPartnersComponent implements OnInit {

  @Input() projectId: string;
  pageId: string;
  partners: Partner[] = [];
  private subs: SubSink = new SubSink();
  canEdit = false;
  isLoaded = false;

  constructor(
    private modalService: NgbModal,
    private store: Store,
    private projectService: ProjectService,
    private translateService: TranslateService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.pageId = data.project?.pageId;
        this.getPartners();
      }
    });
  }

  getPartners(force = false): void {
    if (this.pageId && (!this.isLoaded || force === true)) {
      this.subs.sink = this.projectService.getProjectPartners(this.pageId).subscribe((res) => {
        if (res && res.data) {
          this.partners = res.data.entities;
        }
        this.isLoaded = true;
      }, (error) => this.isLoaded = false);
      this.isLoaded = true;
    }
  }

  addNewPartner(): void {
    if (this.canEdit === true) {
      const modalRef = this.modalService.open(ProjectPartnerAddEditComponent, { size: 'lg', windowClass: 'mosaico-modal' });
      modalRef.componentInstance.projectId = this.projectId;

      this.subs.sink = modalRef.closed.subscribe((res: any) => {
        this.getPartners(true);
      });
    }
  }

  deletePartner(partnerId: any): void {
    if (this.canEdit === true) {
      this.subs.sink = this.projectService.deleteProjectPartner(this.pageId, partnerId).subscribe((res: any) => {
        if (res.data) {
          this.subs.sink = this.translateService.get('MESSAGE.PARTNER_DELETED').subscribe((t) => {
            this.toastr.success(t);
          });
          this.getPartners(true);
        }
      });
    }
  }

  editPartner(partner: any): void {
    const modalRef = this.modalService.open(ProjectPartnerAddEditComponent, { size: 'lg', windowClass: 'mosaico-modal' });
    modalRef.componentInstance.projectId = this.projectId;
    modalRef.componentInstance.form.setValue(partner);
    modalRef.componentInstance.partnerProfile = partner.profileUrl ?? '';
    modalRef.componentInstance.partnerPhoto = partner.photoUrl;

    this.subs.sink = modalRef.closed.subscribe((res: any) => {
      this.getPartners(true);
    });
  }

  moveRight(partner: any, index: number): void {
    let partnerRecord: any = partner;
    if (index < this.partners.length + 1) {
      partnerRecord.order = partner.order + 1;
      this.subs.sink = this.projectService.addUpdatePartner(partnerRecord).subscribe((res: any) => {
        if (res.data) {
          this.subs.sink = this.translateService.get('MESSAGE.SUCCESS').subscribe((t) => {
            this.toastr.success(t);
          });
          this.getPartners(true);
        }
      });
    }
  }

  moveLeft(partner: any, index: number): void {
    let partnerRecord = partner;
    if (index !== 0) {
      partnerRecord.order = partner.order - 1;
      this.subs.sink = this.projectService.addUpdatePartner(partnerRecord).subscribe((res: any) => {
        if (res.data) {
          this.subs.sink = this.translateService.get('MESSAGE.SUCCESS').subscribe((t) => {
            this.toastr.success(t);
          });
          this.getPartners(true);
        }
      });
    }
  }

}
