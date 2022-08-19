import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {SubSink} from 'subsink';
import {ToastrService} from 'ngx-toastr';
import {UserService} from "../../../services";
import {finalize} from "rxjs/operators";
import { FormBase } from 'mosaico-base';

@Component({
  selector: 'app-confirmation-code-form',
  templateUrl: './confirmation-code-form.component.html',
  styleUrls: []
})
export class ConfirmationCodeFormComponent extends FormBase implements OnInit, OnDestroy {

  @Input() inProgress = false;
  @Input() phoneNumber: string;
  @Input() userId: string;
  @Output() codeConfirmed = new EventEmitter<string>();

  subs: SubSink = new SubSink();

  confirmationInProgress = false;

  private readonly codeValidityInSecondsDefaultValue = 120;
  codeValidityInSeconds = this.codeValidityInSecondsDefaultValue;

  codeSendingRequestInProgress = true;
  codeSendingRequestError = false;

  codeSendingAttempts = 0;
  codeSendingAttemptsLimitReached = false;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private toastr: ToastrService
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.generateVerificationCode();
    this.createForm();
    const codeValidityInterval = setInterval(() => {
      if (this.codeValidityInSeconds == 0) clearInterval(codeValidityInterval);
      this.codeValidityInSeconds -= 1;
    }, 1000);
  }

  generateVerificationCode(): void {

    const oldCodeIsStillValid = this.codeSendingAttempts !== 0 && this.codeValidityInSeconds > 0;
    if (oldCodeIsStillValid || this.codeSendingAttemptsLimitReached) {
      return;
    }

    this.codeSendingAttempts += 1;
    this.codeSendingAttemptsLimitReached = this.codeSendingAttempts >= 3;

    this.codeSendingRequestInProgress = true;
    this.userService
      .sendPhoneNumberVerificationCode(this.userId, { phoneNumber: this.phoneNumber })
      .pipe(finalize(() => {
        this.codeSendingRequestInProgress = false;
      }))
      .subscribe(() => {
        this.resetCodeValidityExpiryCounter();
        this.toastr.success(`Code sent to: ${this.phoneNumber}.`);
      }, () => {
        this.toastr.error(`Errors occured while code sending to: ${this.phoneNumber}.`);
        this.codeSendingRequestError = true;
      });
  }

  private resetCodeValidityExpiryCounter() {
    this.codeValidityInSeconds = this.codeValidityInSecondsDefaultValue;
  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      confirmationCode: ['', Validators.required],
    });
  }

  save(): void {
    if (!this.form.valid) {
      return;
    }
    this.codeConfirmed.emit(this.form.controls.confirmationCode.value);
  }

}
