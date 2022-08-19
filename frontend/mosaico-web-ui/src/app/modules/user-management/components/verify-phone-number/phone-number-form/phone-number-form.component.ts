import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {Store} from '@ngrx/store';
import {SubSink} from 'subsink';
import {ToastrService} from 'ngx-toastr';
import {Router} from '@angular/router';
import {phoneNumberValidator} from "./phone-number.validator";
import {phoneNumberExistsValidator} from "./phone-existence.validator";
import {UserService} from "../../../services";
import { ErrorHandlingService, FormBase } from 'mosaico-base';

@Component({
  selector: 'app-phone-number-form',
  templateUrl: './phone-number-form.component.html',
  styleUrls: []
})
export class PhoneNumberFormComponent extends FormBase implements OnInit, OnDestroy {

  @Input() defaultPhoneNumber: string;
  @Output() phoneNumberAdded = new EventEmitter<any>();

  subs: SubSink = new SubSink();
  phoneNumber: FormControl;

  numberValidationRequestInProgress = false;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store,
    private errorHandler: ErrorHandlingService,
    private userService: UserService,
    private toastr: ToastrService,
    private router: Router
  ) {
    super();
    this.phoneNumber = new FormControl('', {
      validators: [Validators.required, phoneNumberValidator()],
      asyncValidators: [phoneNumberExistsValidator(this.userService)],
      updateOn: 'blur'
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    if (this.defaultPhoneNumber) {
      this.phoneNumber.setValue(this.defaultPhoneNumber);
    }
    this.createForm();
    this.phoneNumber.statusChanges
      .subscribe(() => {
        this.numberValidationRequestInProgress = this.phoneNumber.pending;
      });
  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      phoneNumber: this.phoneNumber
    });
  }

  cancel(): void {
    this.form.reset();
    this.router.navigateByUrl('/user/profile');
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }
    this.phoneNumberAdded.emit({phoneNumber: this.phoneNumber.value});
  }
}
