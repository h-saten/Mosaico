import { Component, OnInit,  OnDestroy } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { COMPANY_PERMISSIONS, COMPANY_ROLES } from '../../constants';
import { ErrorHandlingService } from 'mosaico-base';
import { CompanyService, CreateTeamMemberCommand, TeamMember } from 'mosaico-dao';
import { SubSink } from 'subsink';
import { Store } from '@ngrx/store';
import { selectCompanyPermissions, selectCompanyPreview } from '../../store';

@Component({
  selector: 'app-team-member',
  templateUrl: './team-member.component.html',
  styleUrls: ['./team-member.component.scss']
})
export class CompanyTeamsComponent implements OnInit, OnDestroy {
  companyId: string;
  canEdit: boolean;
  team: TeamMember[] = [];
  subs = new SubSink();
  isLoaded = false;
  companyRoles = COMPANY_ROLES;

  constructor(private modalService: NgbModal,
    private toastr: ToastrService,
    private store: Store,
    private errorHandling: ErrorHandlingService,
    private companyService: CompanyService) {
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectCompanyPreview).subscribe((company) => {
      if (company) {
        this.companyId = company.id;
        this.getTeamMembers();
      }
    });
    this.store.select(selectCompanyPermissions).subscribe((res) => {
      this.canEdit = res && res[COMPANY_PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
  }

  getTeamMembers(force = false): void {
    if (this.companyId && (!this.isLoaded || force)) {
      this.subs.sink = this.companyService.getCompanyTeam(this.companyId).subscribe((res) => {
        if (res && res.data) {
          this.team = res.data.entities;
          this.isLoaded = true;
        }
      });
    }
  }

  resendInvitation(invitationId: any): void {
    this.subs.sink = this.companyService.resendInvitation(this.companyId, invitationId).subscribe((res: any) => {
      if (res.data) {
        this.toastr.success('Invitation was resent successfully');
      }
      else {
        this.toastr.error(res.message);
      }
    })
  }

  createTeamMember(command: CreateTeamMemberCommand): void {
    this.subs.sink = this.companyService.createTeamMember(this.companyId, command).subscribe((result) => {
      if (result && result.data) {
        this.toastr.success('Invitation sent to email ' + command.email);
        this.getTeamMembers(true);
      }
    }, (error) => {
      this.errorHandling.handleErrorWithToastr(error);
    });
  }

  changeUserRole(member: TeamMember): void {
    if(member != null){
      this.subs.sink = this.companyService.updateTeamMember(this.companyId, member.id, member.role).subscribe((res) => {
        if(res){
          this.toastr.success('Role was successfully changed');
          this.getTeamMembers(true);
        }
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
        this.getTeamMembers(true);
      });
    }
  }

  removeInvitation(invitationId: any): void {
    this.subs.sink = this.companyService.deleteTeamMember(this.companyId, invitationId).subscribe((res: any) => {
      if (res.data) {
        this.toastr.success('Invitation deleted successfully');
        this.getTeamMembers(true);
      }
    }, (error) => this.errorHandling.handleErrorWithToastr(error));
  }
}
