import { Observable, Subscription } from 'rxjs';
import { SubSink } from 'subsink';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectUserIsVerified } from '../../store';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalProfileComponent } from '../modal-profile/modal-profile.component';
import { UserInformation } from '../../models';
import { selectUserInformation, selectUserPermissions } from '../../store/user.selectors';
import { UserService } from '../../services';
import { setUserInformation } from '../../store/user.actions';
import { take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { ModalChangeEmailComponent } from '../modal-change-email/modal-change-email.component';
import { ModalChangePasswordComponent } from '../modal-change-password/modal-change-password.component';
import { AuthService, Country, CountryService, DEFAULT_MODAL_OPTIONS, ErrorHandlingService, FileParameters, Timezone, TimezoneService } from 'mosaico-base';
import { GetUserProfilePermissionsResponse } from '../../models/responses/get-user-profile-permissions.response';
import { USER_PERMISSIONS } from '../../../../../../projects/mosaico-base/src/lib/constants';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.scss']
})
export class ProfileDetailsComponent implements OnInit, OnDestroy {
  isVerified$: Observable<boolean> | undefined;
  user: UserInformation | null;
  timezones: Timezone[] = [];
  countries: Country[] = [];
  timezone: string;
  country: string;
  active = 1;
  canDisplayAddress = false;
  address: string = '';
  permissions: GetUserProfilePermissionsResponse;
  isLoaded = false;

  canChangeEmail = false;
  canChangePassword = false;
  canVerifyPhone = false;
  canVerifyAccount = false;

  private subs = new SubSink();

  constructor(
    private store: Store,
    private userService: UserService,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private errorHandler: ErrorHandlingService,
    private auth: AuthService,
    private timezoneService: TimezoneService,
    private countryService: CountryService,
    private translateService: TranslateService
  ) {}

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {

    this.isVerified$ = this.store.select(selectUserIsVerified);

    this.subs.sink = this.store.select(selectUserInformation).subscribe((userInfo) => {
      if(userInfo && userInfo.id && !this.isLoaded){
        this.refreshUser(userInfo.id);
        this.active = 1;
        this.isLoaded = true;
      }
    });
    this.subs.sink = this.timezoneService.getTimezones().subscribe((res) => {
      this.timezones = res?.data;
    });
    this.subs.sink = this.countryService.getCountries().subscribe((res) => {
      this.countries = res?.data;
    });
    this.subs.sink = this.store.select(selectUserPermissions).subscribe((res) => {
      this.permissions = res;
      if(res) {
        this.canChangeEmail = this.permissions[USER_PERMISSIONS.CAN_EDIT_EMAIL] === true;
        this.canChangePassword = this.permissions[USER_PERMISSIONS.CAN_EDIT_PASSWORD] === true;
        this.canVerifyPhone = this.permissions[USER_PERMISSIONS.CAN_EDIT_PHONE] === true;
        this.canVerifyAccount = this.permissions[USER_PERMISSIONS.CAN_VERIFY_ACCOUNT] === true;
      }
    });
  }

  private refreshUser(id: string): void {
    this.subs.sink = this.userService.getUser(id).subscribe((res) => {
      if (res && res.data) {
        this.user = res.data;
        this.timezone = this.timezones.find((t)=>t.code === this.user.timezone)?.name;
        this.country = this.countries.find((c)=>c.code === this.user.country)?.name;
        this.store.dispatch(setUserInformation(this.user));
        this.fillInAddress();
      }
    }, (error) => this.errorHandler.handleErrorWithRedirect(error));

  }

  private fillInAddress(): void {
    const countryName = this.countries.find((c) => c.code === this.user.country)?.name;
    const addressArray = [this.user.city, this.user.street, this.user.postalCode, countryName];
    this.address = addressArray.filter(a => a && a.length > 0).join(', ');
    this.canDisplayAddress = this.address && this.address.length > 0;
  }

  editPhotoModal(): void {
    if (this.user) {
      const modalRef = this.modalService.open(ModalProfileComponent, DEFAULT_MODAL_OPTIONS);
      modalRef.componentInstance.currentImgUrl = this.user.photoUrl;
      modalRef.componentInstance.userId = this.user.id;
      modalRef.closed.subscribe((result: FileParameters | null) => {
        setTimeout(() => {
          this.refreshUser(this.user.id);
        }, 1000);
      });
    }
  }

  openChangePasswordDialog(): void {
    this.modalService.open(ModalChangePasswordComponent);
  }

  openEmailDialog(): void {
    this.modalService.open(ModalChangeEmailComponent).closed.subscribe((res) => {
      if(this.user){
        this.refreshUser(this.user?.id);
      }
    });
  }

  deleteProfile(input: any): void {
    this.subs.sink = this.store.select(selectUserInformation).subscribe((user) => {
      if (user.id) {
        this.subs.sink = this.userService.sendDeleteUserRequest(user.id, input.password).subscribe((res) => {
          if(res){
            this.toastr.success('User successfully scheduled for removal, it will be completely deleted in 14 days');
            this.auth.logout();
          }
        }, () => { this.toastr.error(this.translateService.instant('INCORRECT_CREDENTIALS')) });
      }
    });
  }

  phoneNumberVerificationSucceeded(phoneNumber: string) {
    if (this.user?.id) {
      this.refreshUser(this.user.id);
      this.active = 1;
    }
  }
}
