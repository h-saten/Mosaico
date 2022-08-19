import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';
import { CompanyService, SetUserSubscribedCompanyCommand } from 'mosaico-dao';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { UserInformation } from 'src/app/modules/user-management/models';
import { selectIsAuthorized, selectUserInformation } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
import { setUserSubscribedCompany } from '../../store';

@Component({
  selector: 'app-subscription-to-newsletter',
  templateUrl: './subscription-to-newsletter.component.html',
  styleUrls: ['./subscription-to-newsletter.component.scss']
})
export class SubscriptionToNewsletterComponent  extends FormBase implements OnInit, OnDestroy {

  @Input() isUserSubscribeDao: boolean;
  @Input() daoId: string;

  isLoading = false;

  subs: SubSink = new SubSink();

  userEmail = '';

  isAuthorized$: Observable<boolean>;

  showConfirmationSubscribing = false;
  showConfirmationUnsubscribing = false;

  constructor(
    public activeModal: NgbActiveModal,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private errorHandler: ErrorHandlingService,
    private formBuilder: FormBuilder,
    private store: Store,
    private CompanyService: CompanyService,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
  ngOnInit(): void {

    this.isAuthorized$ = this.store.select(selectIsAuthorized);

    if (this.isUserSubscribeDao === true) {

    } else {
      this.subs.sink = this.store.select(selectUserInformation).subscribe(
        (profile) => {
          const userProfile: UserInformation = profile;
          if (userProfile) {
            this.userEmail = userProfile.email;
            this.emailForm();
          }
        }
      );
    }

  }

  private emailForm(): void {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  subscribe(): void {
    if (this.daoId) {

      const command: SetUserSubscribedCompanyCommand = {
        email:this.userEmail
      };

      this.subs.sink = this.CompanyService.setUserSubscribedCompany(this.daoId, command).subscribe((res) => {
        this.store.dispatch(setUserSubscribedCompany({ isSubscribed: true }));
        this.showConfirmationSubscribing = true;
      }, (error) => this.errorHandler.handleErrorWithToastr(error));

    } else {
      this.toastr.info('');
      this.form.enable();
    }

  }

  unsubscribe(): void {
    this.subs.sink = this.isAuthorized$.subscribe((res: boolean) => {
      if (res === true) {
        const command: SetUserSubscribedCompanyCommand = {
          email:this.userEmail
        };
        this.subs.sink = this.CompanyService.deleteUserSubscribedCompany(this.daoId, command).subscribe((updateRes) => {
          this.store.dispatch(setUserSubscribedCompany({ isSubscribed: false }));
          this.showConfirmationUnsubscribing = true;
        }, (error) => this.errorHandler.handleErrorWithToastr(error));
      }
    });
  }
  closeModal(): void {
    this.activeModal.close();
  }
}
