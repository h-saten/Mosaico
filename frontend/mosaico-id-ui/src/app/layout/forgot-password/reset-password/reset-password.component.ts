import {Component, Inject, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {CookieService} from 'ngx-cookie-service';
import {TranslateService} from '@ngx-translate/core';
import {AuthService} from '../../../services/auth.service';
import {DOCUMENT} from '@angular/common';
import {ReCaptchaV3Service} from 'ng-recaptcha';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  code = '';
  userId = '';
  private logoutId: string;

  resetPasswordForm: FormGroup;

  public resetError = false;
  public resetCaptchaError = false;
  public resetSuccess = false;
  public formSubmitted = false;

  email: FormControl = new FormControl('', [
    Validators.required,
    Validators.email
  ]);
  password: FormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(6),
    Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,})/)
  ]);
  confirmPassword: FormControl = new FormControl('', [
    Validators.required
  ]);

  usedForm: boolean;
  resetRequestFinished: boolean;
  passwordsHaveSameValue = false;

  constructor(
    private apiAccountClient: AuthService,
    private route: ActivatedRoute,
    private cookieService: CookieService,
    private translate: TranslateService,
    private recaptchaV3Service: ReCaptchaV3Service,
    @Inject(DOCUMENT) private document: Document
  ) {
    this.resetRequestFinished = true;
    this.usedForm = false;
  }

  ngOnInit() {
    this.logoutId = this.route.snapshot.queryParamMap.get('logoutId');
    this.code = this.route.snapshot.queryParamMap.get('code');
    this.userId = this.route.snapshot.queryParamMap.get('userId');
    if (this.cookieService.check('resPass' + this.code)) {
      if (this.cookieService.get('resPass' + this.code) === '1') {
        this.usedForm = true;
      } else {
        this.resetPasswordForm = new FormGroup({
          email: this.email,
          password: this.password,
          confirmPassword: this.confirmPassword
        });

      }
    } else {
      this.resetPasswordForm = new FormGroup({
        email: this.email,
        password: this.password,
        confirmPassword: this.confirmPassword
      });
    }
    this.checkingPasswordMatching();
  }

  private checkingPasswordMatching() {
    this.password.valueChanges.subscribe(() => {
      this.passwordsHaveSameValue = this.checkPasswordsValid();
    });
    this.confirmPassword.valueChanges.subscribe(() => {
      this.passwordsHaveSameValue = this.checkPasswordsValid();
    });
  }

  isEngLang(): boolean {
    return this.translate.currentLang !== 'pl';
  }

  submitForm() {

    if (this.resetPasswordForm.invalid) {
      return;
    }
    this.resetError = false;
    this.recaptchaV3Service.execute('RegistrationSubmit').subscribe((token) => {
      this.apiAccountClient.verifyReCaptchaToken({response:token}).subscribe((res)=>{
        if(res.data["success"]==true && res.data["score"]>=.3){
          this.formSubmitted = true;
          this.resetPassword();
          // zapisanie ciasteczka że skorzystano z formularza
          const expiresAt = new Date();
          expiresAt.setDate(expiresAt.getDate() + 365);
          this.cookieService.set('resPass' + this.code, '1', expiresAt, '/');
        }
        else{
          this.resetCaptchaError = true;
        }
      })
    })

  }

  resetPassword()
  {
    this.resetRequestFinished = false;

    this.apiAccountClient
      .resetPassword({
        email: this.email.value,
        code: this.code,
        confirmPassword: this.confirmPassword.value,
        password: this.password.value
      })
      .pipe(finalize(() => {
        this.resetRequestFinished = true;
      }))
      .subscribe((success) => {
        this.resetSuccess = true;
        this.logout();
      }, () => {
        this.resetError = true;
      });
  }

  //force logout after successful password change
  logout() {
    this.apiAccountClient
      .logout({
        logoutId: this.logoutId
      })
      .subscribe((response) => {
        if(response?.data?.postLogoutRedirectUri && response?.data?.postLogoutRedirectUri.length > 0){
          this.document.location.href = response.data.postLogoutRedirectUri;
        }
      });
  }

  // Pobierz FormControl
  getFormControl(name: string) {
    return this.resetPasswordForm.get(name);
  }


  // Zwróć TRUE, jeśli element FormControl jest poprawny
  isValid(name: string) {
    const e = this.getFormControl(name);
    return e && e.valid;
  }

  // Zwróć TRUE, jeśli element FormControl nie jest poprawny po wprowadzeniu zmian
  hasError(name: string) {
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

  showResetError() {
    if(this.resetError || this.resetCaptchaError){
      return true;
    }
  }

  show_hide_password(e) {

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

}
