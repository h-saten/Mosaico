import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {Subscription} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';
import {AuthService} from "../../../../services/auth.service";
import {ExternalProviderLoginService} from "../../../../services/external-provider-login.service";
import {AppConfigurationService} from "../../../../services/app-configuration.service";
import {CoreUserRegistrationData} from "../social-reg.component";
import {emailExistsValidator} from "./email-existence.validator";
import { ReCaptchaV3Service } from 'ng-recaptcha';

@Component({
  selector: 'app-core-user-data-form',
  templateUrl: './core-user-data-form.component.html',
  styleUrls: ['core-user-data-form.component.scss']
})
export class CoreUserDataFormComponent implements OnInit, OnDestroy {

  @Input() inProgress = false;
  @Output() data = new EventEmitter<CoreUserRegistrationData>();

  public checkAllCheckboxes = false;

  regForm: FormGroup;

  // tslint:disable-next-line: max-line-length
  private emailRegExpPattern = /^([a-z0-9]+(?:[.+_-][a-z0-9]+)*)@([a-z0-9]+(?:[.-][a-z0-9]+)*\.[a-z]{2,})$/;

  firstName = new FormControl('', [
    Validators.required,
    Validators.minLength(2),
    Validators.maxLength(50),
  ]);

  lastName = new FormControl('', [
    Validators.required,
    Validators.minLength(2),
    Validators.maxLength(50),
  ]);

  email = new FormControl('', {
    validators: [Validators.required, Validators.email, Validators.pattern(this.emailRegExpPattern)],
    asyncValidators: [emailExistsValidator(this.authClient)],
    updateOn: 'blur'
  });

  password = new FormControl('', [
    Validators.required,
    Validators.minLength(6),
    Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,})/)
  ]);

  confirmPassword = new FormControl('', [
    Validators.required,
    Validators.minLength(6),
  ]);

  terms = new FormControl('', [
    Validators.requiredTrue
  ]);

  notForbiddenCitizenship = new FormControl('', [
    Validators.requiredTrue
  ]);

  checkAll = new FormControl(false)

  newsletterPersonalDataProcessing = new FormControl(false);

  private _emailValueChanges: Subscription;

  public dataSavingRequestInProgress: boolean;
  public registrationServerError: boolean;
  public errorMessage: string;
  public captchaErrorMessage:string;

  constructor(
    private formBuilder: FormBuilder,
    private authClient: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslateService,
    private externalLogin: ExternalProviderLoginService,
    private appConfigurationService: AppConfigurationService,
    private recaptchaV3Service: ReCaptchaV3Service
  ) {
    this.dataSavingRequestInProgress = false;
    this.regForm = this.createForm();
    this.registrationServerError = false;
    this.errorMessage = null;
    this.captchaErrorMessage=this.translate.instant('auth.login.captcha.error');
  }

  ngOnInit() {
    this.initFormData();
  }

  ngOnDestroy() {
    if (this._emailValueChanges) {
      this._emailValueChanges.unsubscribe();
    }
  }

  private createForm(): FormGroup {
    return this.formBuilder.group ({
      firstName: this.firstName,
      lastName: this.lastName,
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword,
      terms: this.terms,
      notForbiddenCitizenship: this.notForbiddenCitizenship,
      newsletterPersonalDataProcessing: this.newsletterPersonalDataProcessing,
      checkAll: this.checkAll
    });
  }

  private initFormData(): void {

    const email = this.route.snapshot.queryParamMap.get('email');

    if (email != null) {
      if (email.match(this.emailRegExpPattern)) {
        const emailField = this.regForm.get('email');
        emailField.setValue(email);
        emailField.updateValueAndValidity();
      }
    }
  }

  register() {
    if (!this.regForm.valid) {
      return;
    }
    this.dataSavingRequestInProgress = true;
    this.registrationServerError=false;

    this.recaptchaV3Service.execute('RegistrationSubmit').subscribe((token) => {
      this.authClient.verifyReCaptchaToken({response:token}).subscribe((res)=>{
        if(res.data["success"]==true && res.data["score"]>=.3){
          const outputData: CoreUserRegistrationData = {
            firstName: this.firstName.value,
            lastName: this.lastName.value,
            email: this.email.value,
            password: this.password.value,
            newsletterAgreement: this.newsletterPersonalDataProcessing.value,
            notForbiddenCitizenship: this.notForbiddenCitizenship.value,
            terms: this.terms.value,
          };

          this.data.emit(outputData);
        }
        else{
          this.dataSavingRequestInProgress = false;
          this.registrationServerError=true;
          this.errorMessage=this.captchaErrorMessage;
        }
      })
    })
  }

  checkAllCheck() {

    this.checkAllCheckboxes = !this.checkAllCheckboxes;
    const state = this.checkAllCheckboxes;

    const controls = this.regForm.controls;

    controls.terms.setValue(state);
    controls.terms.markAsTouched();

    controls.notForbiddenCitizenship.setValue(state);
    controls.notForbiddenCitizenship.markAllAsTouched();

    controls.newsletterPersonalDataProcessing.setValue(state);
  }

  togglePasswordFieldValueVisibility(e) {

    e.preventDefault();

    const passwordFields = document.querySelectorAll('.password-field');

    let fieldType = '';
    let isPasswordHidden = true;
    passwordFields.forEach((field) => {

      fieldType = field.getAttribute('type');

      isPasswordHidden = fieldType === 'password';

      if (isPasswordHidden) {
        field.setAttribute('type', 'text');
      } else {
        field.setAttribute('type', 'password');
      }
    });

    const togglerIcons = document.querySelectorAll('.password-toggler-icon');

    togglerIcons.forEach((element) => {

      isPasswordHidden = element.classList.contains('fa-eye-slash');

      if (isPasswordHidden) {
        element.classList.remove('fa-eye-slash');
        element.classList.add('fa-eye');
      } else {
        element.classList.remove('fa-eye');
        element.classList.add('fa-eye-slash');
      }
    });
  }

  getFormControl(name: string) {
    return this.regForm.get(name);
  }

  // Return TRUE, if FormControl element is correct
  isValid(name: string) {
    const e = this.getFormControl(name);
    return e && e.valid;
  }

  // Return TRUE, if FormControl element is not correct after value has been changed
  hasError(name: string): boolean {
    const e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
  }

  checkPasswordsValid() {
    const passwordValue = this.password.value;
    const passwordValid = this.password.valid;
    const confirmPasswordValue = this.confirmPassword.value;
    const confirmPasswordValid = this.confirmPassword.valid;

    if ((passwordValid && confirmPasswordValid) && (passwordValue !== confirmPasswordValue)) {
      return false;
    }
    return true;
  }

  isEngLang(): boolean {
    return this.translate.currentLang !== 'pl';
  }

  FacebookLogin() {
    return this.externalLogin.FacebookLogin();
  }

  openRegulations(event) {
    event.preventDefault();
    return window.open(this.appConfigurationService.mosaicoAppUrl() + '/assets/pdf/Regulamin_platformy_MOSAICO_11_2019.pdf');
  }

  openPrivacyPolicy(event) {
    event.preventDefault();
    return window.open(this.appConfigurationService.mosaicoAppUrl() + '/assets/pdf/Polityka_prywatnosci_09_2020.pdf');
  }

  changeConditions() {
    const controls = this.regForm.controls;
    if(this.terms.value && this.notForbiddenCitizenship.value && this.newsletterPersonalDataProcessing.value) {
      this.checkAllCheckboxes = true;
      controls.checkAll.setValue(true);
    } else {
      controls.checkAll.setValue(false);
      this.checkAllCheckboxes = false;
    }
  }
}
