import { setUserSubscribeProject } from '../../../project-management/store/project.actions';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';
import { ProjectService, SetUserSubscribedProjectCommand } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { UserInformation } from 'src/app/modules/user-management/models';
import { selectIsAuthorized, selectUserInformation } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-subscription-to-newsletter',
  templateUrl: './subscription-to-newsletter.component.html',
  styleUrls: ['./subscription-to-newsletter.component.scss']
})
export class SubscriptionToNewsletterComponent extends FormBase implements OnInit, OnDestroy {

  @Input() isUserSubscribeProject: boolean;
  @Input() projectId: string;

  isLoading = false;

  subs: SubSink = new SubSink();

  userEmail = '';

  isAuthorized: boolean = false;

  showConfirmationSubscribing = false;
  showConfirmationUnsubscribing = false;

  constructor(
    public activeModal: NgbActiveModal,
    private toastr: ToastrService,
    private errorHandler: ErrorHandlingService,
    private store: Store,
    private projectService: ProjectService,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectIsAuthorized).subscribe((res) => {
      this.isAuthorized = res;
    });
    if (this.isUserSubscribeProject === false) {
      this.subs.sink = this.store.select(selectUserInformation).subscribe(
        (profile) => {
          const userProfile: UserInformation = profile;
          if (userProfile) {
            this.userEmail = userProfile.email;
          }
        }
      );
    }
    this.createEmailForm();
  }

  private createEmailForm(): void {
    this.form = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
    });
  }

  subscribe(): void {
    if (this.projectId) {
      const command: SetUserSubscribedProjectCommand = {};

      if(this.isAuthorized === false) {
        if(!validateForm(this.form)){
          this.toastr.error('Invalid form values');
          return;
        }
        command.email = this.form.getRawValue().email;
      }

      this.subs.sink = this.projectService.setUserSubscribesProject(this.projectId, command).subscribe((res) => {
        this.store.dispatch(setUserSubscribeProject({ isSubscribed: true }));
        this.showConfirmationSubscribing = true;
      }, (error) => this.showConfirmationSubscribing = true);

    }
  }

  unsubscribe(): void {
    if (this.isAuthorized === true) {
      const command: SetUserSubscribedProjectCommand = {
        // email
      };

      this.subs.sink = this.projectService.deleteUserSubscribesProject(this.projectId, command).subscribe((updateRes) => {
        this.store.dispatch(setUserSubscribeProject({ isSubscribed: false }));
        this.showConfirmationUnsubscribing = true;
      }, (error) => this.errorHandler.handleErrorWithToastr(error));

    }
  }

  closeModal(): void {
    this.activeModal.close();
  }
}
