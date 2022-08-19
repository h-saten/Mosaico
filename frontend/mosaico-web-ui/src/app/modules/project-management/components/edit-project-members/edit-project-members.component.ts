import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { selectProjectEditId, selectProjectPreview, selectPreviewProjectPermissions } from '../../store/project.selectors';
import { take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { PERMISSIONS, PROJECT_ROLES } from '../../constants';
import { AddProjectMemberCommand } from '../../../../../../projects/mosaico-project/src/lib/commands';
import { ProjectMember, ProjectMemberService } from 'mosaico-project';
import { ErrorHandlingService } from 'mosaico-base';

@Component({
  selector: 'app-edit-project-members',
  templateUrl: './edit-project-members.component.html',
  styleUrls: ['./edit-project-members.component.scss']
})
export class EditProjectMembersComponent implements OnInit, OnDestroy {
  members: ProjectMember[] = [];
  subs: SubSink = new SubSink();
  projectId: string;
  constructor(
    private service: ProjectMemberService, 
    private store: Store, 
    private toastr: ToastrService, 
    private translate: TranslateService,
    private errorHandling: ErrorHandlingService
  ) { }

  projectRoles = PROJECT_ROLES;
  canEdit: boolean;
  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
    this.subs.sink = this.store.select(selectProjectPreview).pipe(take(1)).subscribe((res) => {
      this.projectId = res?.project?.id;
      this.reloadMembers();
    });
  }

  reloadMembers(): void {
    this.subs.sink = this.service.getMembers(this.projectId).subscribe((res) => {
      if (res && res.data) {
        this.members = res.data.entities;
      }
    });
  }

  delete(id: string | undefined): void {
    if (id && id.length > 0) {
      this.subs.sink = this.service.deleteProjectMember(this.projectId, id).subscribe((res) => {
        this.toastr.success(this.translate.instant('EDIT_PROJECT.ALERTS.SUCCESSFULLY_REMOVED'));
        this.reloadMembers();
      }, (error) => {
        this.toastr.error(this.translate.instant('PROJECT_MEMBERS.' + error.error.code));
      });
    }
  }

  public changeUserRole(memberId: string): void {
    if(memberId && memberId.length > 0){
      const member = this.members.find((m) => m.id === memberId);
      if(member){
        this.subs.sink = this.service.updateMemberRole(this.projectId, member.id, {role: member.role?.key}).subscribe((res) => {
          if(res && res.ok){
            this.toastr.success(this.translate.instant('EDIT_PROJECT.ALERTS.SUCCESSFULLY_UPDATED'));
            this.reloadMembers();
          }
        }, (error) => this.errorHandling.handleErrorWithToastr(error));
      }
    }
  }

  public addNewMember(command: AddProjectMemberCommand): void {
    if(command){
      this.subs.sink = this.service.addProjectMember(this.projectId, command).subscribe((res) => {
        if(res && res.ok) {
          this.toastr.success(this.translate.instant('EDIT_PROJECT.ALERTS.SUCCESSFULLLY_ADDED'));
          this.reloadMembers();
        }
      }, (error) => this.toastr.error(this.translate.instant('EDIT_PROJECT.ALERTS.REGULAR_VALIDATOR'))
      )
    }
  }

  public cancelUserRole():void{
    this.reloadMembers();
  }
}
