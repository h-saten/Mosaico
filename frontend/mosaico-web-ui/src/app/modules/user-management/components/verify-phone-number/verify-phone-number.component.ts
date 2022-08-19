import {Component, EventEmitter, Input, OnDestroy, OnInit, Optional, Output} from '@angular/core';
import {Store} from '@ngrx/store';
import {SubSink} from 'subsink';
import {ToastrService} from 'ngx-toastr';
import {Router} from '@angular/router';
import {UserService} from '../../services';
import {selectUserInformation} from "../../store";
import {finalize, take} from "rxjs/operators";
import {FormBase} from 'mosaico-base';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {TranslateService} from '@ngx-translate/core';
import {UpdatePhoneNumberCommand} from '../../commands';

@Component({
  selector: 'app-verify-phone-number',
  templateUrl: './verify-phone-number.component.html',
  styleUrls: ['./verify-phone-number.component.scss']
})
export class VerifyPhoneNumberComponent extends FormBase implements OnInit, OnDestroy {

  @Output() verificationSucceeded = new EventEmitter<string>();
  @Output() verificationCanceled = new EventEmitter();
  subs: SubSink = new SubSink();
  requestInProgress = false;
  userId: string;
  phoneNumber: string | null = null;
  confirmationCode: string | null = null;
  isEditing = false;
  @Input() isModal = false;

  phoneNumberVerified:string;
  codeVerificationFailed:string;
  phoneNumberUpdated:string;

  constructor(
    private store: Store,
    private userService: UserService,
    private toastr: ToastrService,
    private router: Router,
    private translateService: TranslateService,
    @Optional() private activeModal?: NgbActiveModal
  ) {
    super();
    this.phoneNumberVerified = this.translateService.instant('VERIFY_PHONE.phone.varified');
    this.codeVerificationFailed = this.translateService.instant('VERIFY_PHONE.phone.code.varification.failed');
    this.phoneNumberUpdated = this.translateService.instant('VERIFY_PHONE.phone.updated');
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectUserInformation).pipe(take(1)).subscribe((userInfo) => {
      if(userInfo){
        this.userId = userInfo.id;
        if(userInfo.phoneNumber && userInfo.phoneNumber.length > 0){
          this.isEditing = true;
        }
      }
    });
  }

  savePhoneNumber(payload: {phoneNumber: string}): void {
      this.phoneNumber = payload?.phoneNumber;
  }

  saveConfirmedCode(code: string): void {
    this.confirmationCode = code;
    this.verifyCode(code);
  }


  private verifyCode(code: string): void {
    if(this.isEditing){
      this.updatePhoneNumber(code);
    }
    else{
      this.createPhoneNumber(code);
    }
  }

  private createPhoneNumber(code: string): void {
    this.requestInProgress = true;
    this.userService
      .verifyPhoneNumber(this.userId, { phoneNumber: this.phoneNumber as string, confirmationCode: code })
      .pipe(finalize(() => {
        this.requestInProgress = false;
      }))
      .subscribe(() => {
        this.toastr.success(this.phoneNumberVerified);
        this.verificationSucceeded.emit(this.phoneNumber);
      }, () => {
        this.toastr.error(this.codeVerificationFailed);
      });
  }

  private updatePhoneNumber(code: string): void {
    this.requestInProgress = true;
    const command = {
      phoneNumber: this.phoneNumber,
      code
    } as UpdatePhoneNumberCommand;
    this.userService.updatePhoneNumber(command)
      .pipe(finalize(() => {
        this.requestInProgress = false;
        if(this.activeModal) {
          this.activeModal.close();
        }
      }))
      .subscribe(() => {
        this.toastr.success(this.phoneNumberUpdated);
        this.verificationSucceeded.emit(this.phoneNumber);
      }, () => {
        this.toastr.error(this.codeVerificationFailed);
      });
  }
}
