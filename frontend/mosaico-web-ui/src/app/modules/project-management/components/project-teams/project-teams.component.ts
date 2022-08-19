import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProjectService, TeamMember } from 'mosaico-project';
import { SubSink } from 'subsink';
import { Store } from "@ngrx/store";
import { ProjectTeamManageModalComponent } from './project-team-manage-modal/project-team-manage-modal.component';
import { selectPreviewProjectPermissions, selectProjectPreview } from '../../store';
import { PERMISSIONS } from '../../constants';
import { ProjectTeamsAddEditComponent } from './project-team-add-edit/project-team-add-edit.component';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-project-teams',
  templateUrl: './project-teams.component.html',
  styleUrls: ['./project-teams.component.scss']
})
export class ProjectTeamsComponent implements OnInit {
  @Input() projectId: string;
  pageId: string;
  team: TeamMember[] = [];
  private subs: SubSink = new SubSink();
  canEdit = false;
  isLoaded = false;
  constructor(private modalService: NgbModal, private translateService: TranslateService,
    private store: Store, private toastr: ToastrService,
    private projectService: ProjectService) {
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.pageId = data.project?.pageId;
        if (this.pageId) {
          this.getTeamMembers();
        }
      }
    });
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
  }

  getTeamMembers(force = false): void {
    if (this.pageId && (!this.isLoaded || force === true)) {
      this.subs.sink = this.projectService.getProjectTeamMembers(this.pageId).subscribe((res) => {
        if (res && res.data) {
          this.team = res.data.entities;
        }
        this.isLoaded = true;
      }, (error) => this.isLoaded = false);
      this.isLoaded = true;
    }
  }

  openEditTeamsModal(): void {
    const modalRef = this.modalService.open(ProjectTeamManageModalComponent, { size: 'lg', windowClass: 'mosaico-modal' });
    modalRef.componentInstance.team = this.team;
    modalRef.componentInstance.projectId = this.projectId;
    this.subs.sink = modalRef.closed.subscribe((res: any) => {
      this.getTeamMembers(true);
    });
  }

  addNewMember(): void {
    const modalRef = this.modalService.open(ProjectTeamsAddEditComponent, { size: 'lg', windowClass: 'mosaico-modal' });
    modalRef.componentInstance.projectId = this.projectId;
    this.subs.sink = modalRef.closed.subscribe((res: any) => {
      this.getTeamMembers(true);
    });
  }

  deleteMember(memberId: any): void {
    this.subs.sink = this.projectService.deleteTeamMember(this.pageId, memberId).subscribe((res: any) => {
      if (res.data) {
        this.toastr.success(this.translateService.instant('PROJECT_TEAM.DELETED'));
        this.getTeamMembers(true);
      }
    });
  }

  editMember(member: any): void {
    const modalRef = this.modalService.open(ProjectTeamsAddEditComponent, { size: 'lg', windowClass: 'mosaico-modal' });
    modalRef.componentInstance.projectId = this.projectId;
    modalRef.componentInstance.form.setValue(member);
    modalRef.componentInstance.memberProfile = member.profileUrl ?? '';
    modalRef.componentInstance.memberPhoto = member.photoUrl;

    this.subs.sink = modalRef.closed.subscribe((res: any) => {
      this.getTeamMembers(true);
    });
  }

  moveRight(member: any, index: number): void {
    let memberRecord: any = member;
    if (index < this.team.length + 1) {
      memberRecord.order = member.order + 1;
      this.subs.sink = this.projectService.addUpdateTeamMember(memberRecord).subscribe((res: any) => {
        if (res.data) {
          this.subs.sink = this.translateService.get('MESSAGE.SUCCESS').subscribe((t) => {
            this.toastr.success(t);
          });
          this.getTeamMembers(true);
        }
      });
    }
  }

  moveLeft(member: any, index: number): void {
    let memberRecord = member;
    if (index !== 0) {
      memberRecord.order = member.order - 1;
      this.subs.sink = this.projectService.addUpdateTeamMember(memberRecord).subscribe((res: any) => {
        if (res.data) {
          this.subs.sink = this.translateService.get('MESSAGE.SUCCESS').subscribe((t) => {
            this.toastr.success(t);
          });
          this.getTeamMembers(true);
        }
      });
    }
  }

}
