import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, Validators} from '@angular/forms';
import {Store} from '@ngrx/store';
import {SubSink} from 'subsink';
import {ToastrService} from 'ngx-toastr';
import {Router} from '@angular/router';
import {GetUserProfilePermissionsResponse, UserInformation} from '../../models';
import {selectUserInformation, selectUserPermissions, setUserInformation} from '../../store';
import {UserService} from '../../services';
import {UpdateUserCommand} from '../../commands';
import {
  Country,
  CountryService,
  DEFAULT_MODAL_OPTIONS,
  ErrorHandlingService,
  FormBase,
  Timezone,
  TimezoneService,
  trim,
  USER_PERMISSIONS,
  validateForm
} from 'mosaico-base';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ModalChangeEmailComponent} from '../modal-change-email/modal-change-email.component';
import {formatDate} from '@angular/common';
import {VerifyPhoneNumberComponent} from '../verify-phone-number/verify-phone-number.component';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent extends FormBase implements OnInit, OnDestroy {
  subs: SubSink = new SubSink();
  countries: Country[] = [];
  timezones: Timezone[] = [];
  date: Date = new Date();
  userProfile: UserInformation | null | undefined;
  userDataSaved:string;
  invalidFormValue:string;

  permissions: GetUserProfilePermissionsResponse;

  canChangeEmail = false;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store,
    private errorHandler: ErrorHandlingService,
    private userService: UserService,
    private toastr: ToastrService,
    private modalService: NgbModal,
    private router: Router,
    private countryService: CountryService,
    private timezoneService: TimezoneService,
    private translateService: TranslateService,
  ) {
    super();
    this.userDataSaved = this.translateService.instant('EDIT_PROFILE.user.data.saved');
    this.invalidFormValue = this.translateService.instant('USERPROFILE.form.invalid.data')
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
    this.subs.sink = this.store.select(selectUserInformation).subscribe(
      (profile) => {
        this.userProfile = profile;
        this.updateFormValue(profile);
      }
    );
    this.subs.sink = this.countryService.getCountries().subscribe((res) => {
      this.countries = res?.data;
    });
    this.subs.sink = this.timezoneService.getTimezones().subscribe((res) => {
      this.timezones = res?.data;
    });
    this.subs.sink = this.store.select(selectUserPermissions).subscribe((res) => {
      this.permissions = res;
      if(res) {
        this.canChangeEmail = this.permissions[USER_PERMISSIONS.CAN_EDIT_EMAIL] === true;
      }
    });
  }

  updateDisabledInputs(): void {
    if(!this.canChangeEmail){
      this.form.get('email')?.disable();
    }
  }

  private refreshUser(id: string): void {
    this.subs.sink = this.userService.getUser(id).subscribe((res) => {
      if (res && res.data) {
        this.store.dispatch(setUserInformation(res.data));
      }
    }, (error) => this.errorHandler.handleErrorWithRedirect(error));

  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      email: [''],
      phoneNumber: [''],
      country: [null],
      timezone: [null],
      postalCode: [''],
      city: [''],
      street: [''],
      dob: [''],
    });
    this.updateDisabledInputs();
  }

  private updateFormValue(userProfile: UserInformation | null | undefined): void {
    if (userProfile) {
      this.form.setValue({
        firstName: (userProfile.firstName ? userProfile.firstName : ''),
        lastName: (userProfile.lastName ? userProfile.lastName : ''),
        email: (userProfile.email ? userProfile.email : ''),
        phoneNumber: (userProfile.phoneNumber ? userProfile.phoneNumber : ''),
        country: (userProfile.country ? userProfile.country : null),
        timezone: (userProfile.timezone ? userProfile.timezone : null),
        postalCode: (userProfile.postalCode ? userProfile.postalCode : ''),
        city: (userProfile.city ? userProfile.city : ''),
        street: (userProfile.street ? userProfile.street : ''),
        dob: (userProfile.dob ? formatDate(userProfile.dob, 'yyyy-MM-dd', 'en') : ''),
      });
      this.updateDisabledInputs();
    }
  }

  save(): void {
    if (validateForm(this.form) && this.userProfile) {
      let command = this.form.getRawValue() as UpdateUserCommand;
      command = trim(command);
      this.form.disable();
      command = trim(command);
      this.subs.sink = this.userService.updateUser(this.userProfile.id, command).subscribe((res) => {
        if (res) {
          this.toastr.success(this.userDataSaved);
          this.router.navigateByUrl('/user/profile');
        }
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.form.enable();  this.updateDisabledInputs(); });
    } else {
      this.toastr.error(this.invalidFormValue);
    }
  }

  openEmailDialog(): void {
    const modal = this.modalService.open(ModalChangeEmailComponent, DEFAULT_MODAL_OPTIONS)
    modal.componentInstance.isModal = true;
    this.subs.sink = modal.componentInstance.changeCanceled?.subscribe(() => {
      modal.close();
    });
    this.subs.sink = modal.closed.subscribe(() => {
      if (this.userProfile) {
        this.refreshUser(this.userProfile.id);
      }
    });
  }

  openChangePhoneDialog() {
    const modal = this.modalService.open(VerifyPhoneNumberComponent, DEFAULT_MODAL_OPTIONS);
    modal.componentInstance.isModal = true;
    this.subs.sink = modal.componentInstance.verificationCanceled?.subscribe(() => {
      modal.close();
    });
    this.subs.sink = modal.componentInstance.verificationSucceeded.subscribe((phoneNumber) => {
      const userProfile = this.userProfile;
      userProfile.phoneNumber = phoneNumber;
      this.store.dispatch(setUserInformation(userProfile));
      modal.close();
    })
  }

  cancel(): void {
    this.form.reset();
    this.router.navigateByUrl('/user/profile');
  }

}
