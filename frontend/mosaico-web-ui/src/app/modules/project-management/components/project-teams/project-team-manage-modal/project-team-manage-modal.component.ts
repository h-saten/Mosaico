import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ProjectService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { selectProjectPreview } from '../../../store';
import { ProjectTeamsAddEditComponent } from '../project-team-add-edit/project-team-add-edit.component';

@Component({
  selector: 'app-project-team-manage-modal',
  templateUrl: './project-team-manage-modal.component.html',
  styleUrls: ['./project-team-manage-modal.component.scss']
})
export class ProjectTeamManageModalComponent implements OnInit {
  @Input() team: any[];
  @Input() projectId: string;
  pageId: string;
  private subs: SubSink = new SubSink();
  constructor(private translateService: TranslateService, private toastrService: ToastrService, private store: Store, public activeModal: NgbActiveModal, private modalService: NgbModal, private projectService: ProjectService) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.pageId = data.project?.pageId;
      }
    })
  }

  addNewMember(): void {
    const modalRef = this.modalService.open(ProjectTeamsAddEditComponent, { size: 'lg' });
    modalRef.componentInstance.projectId = this.projectId;

    modalRef.closed.subscribe((res: any) => {
      this.getTeamMembers();
    });
  }

  getTeamMembers(): void {
    this.projectService.getProjectTeamMembers(this.projectId).subscribe((res) => {
      if (res && res.data) {
        this.team = res.data.entities;
      }
    });
  }

  deleteMember(memberId: any): void {
    this.projectService.deleteTeamMember(this.pageId, memberId).subscribe((res: any) => {
      if (res.data) {
        alert('Member Record Deleted');
        this.getTeamMembers();
      }
      else {
        alert(res.message);
      }
    })
  }

  editMember(member: any): void {
    const modalRef = this.modalService.open(ProjectTeamsAddEditComponent, { size: 'lg' });
    modalRef.componentInstance.projectId = this.projectId;
    modalRef.componentInstance.memberForm.setValue(member);
    modalRef.componentInstance.memberProfile = member.profileUrl ?? '';
    modalRef.componentInstance.memberPhoto = member.photoUrl;

    modalRef.closed.subscribe((res: any) => {
      this.getTeamMembers();
    });
  }

  moveDownOnce(member: any, index: number) {
    let memberRecord: any = member;
    if (index < this.team.length + 1) {
      memberRecord.order = member.order + 1;
      this.projectService.addUpdateTeamMember(memberRecord).subscribe((res: any) => {
        if (res.data) {
          this.translateService.get('MESSAGE.SUCCESS').subscribe((t) => {
            this.toastrService.success(t);
          });
          this.getTeamMembers();
        }
      })
    }
  }

  moveUpOnce(member: any, index: number) {
    let memberRecord: any = member;
    if (index != 0) {
      memberRecord.order = member.order - 1;
      this.projectService.addUpdateTeamMember(memberRecord).subscribe((res: any) => {
        if (res.data) {
          this.translateService.get('MESSAGE.SUCCESS').subscribe((t) => {
            this.toastrService.success(t);
          });
          this.getTeamMembers();
        }
      })
    }
  }

}
