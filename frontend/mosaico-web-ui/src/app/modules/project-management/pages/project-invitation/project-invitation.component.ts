import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SubSink } from 'subsink';
import { ToastrService } from 'ngx-toastr';
import { ProjectMemberService } from 'mosaico-project';
import { ErrorHandlingService } from 'mosaico-base';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-project-invitation',
  templateUrl: './project-invitation.component.html',
  styleUrls: ['./project-invitation.component.scss']
})
export class ProjectInvitationComponent implements OnInit, OnDestroy {
  isLoading = true;
  subs: SubSink = new SubSink();

  constructor(private service: ProjectMemberService, private router: Router, private activatedRoute: ActivatedRoute, private toastr: ToastrService, private errorHandler: ErrorHandlingService, private translateService: TranslateService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const authorizationCode = params['authorizationCode'];
      if(!authorizationCode || authorizationCode.length === 0){
        this.handleMissingParameterError();
      }
      else {
        this.subs.sink = this.service.acceptInvitation({authorizationCode}).subscribe((res) => {
          if(res && res.data){
            this.isLoading = false;
            this.toastr.success(this.translateService.instant('PROJECT_JOINED'));
            setTimeout(() => {
              this.router.navigate([`/project/${res.data}`]);
            }, 1500);
          }
          else {
            this.handleMissingParameterError();
          }
        }, (error) => this.errorHandler.handleErrorWithRedirect(error));
      }
    });
  }

  private handleMissingParameterError(): void {
    const error: any = {
      error: {
        code: 'INVALID_CODE',
        message: 'Received invalid authorization code in parameters',
        ok: false,
        extraData: null
      }
    };
    this.errorHandler.handleErrorWithRedirect(error);
  }

}
