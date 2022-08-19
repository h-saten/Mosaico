import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {ApiAccountClient, ForgotPasswordViewModel} from '../../../services/tokenizer-auth-api.service';
import {finalize} from 'rxjs/operators';
import { AuthService, InitiateForgotPassword } from '../../../services/auth.service';
import { ReCaptchaV3Service } from 'ng-recaptcha';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-forgot',
  templateUrl: './forgot.component.html',
  styleUrls: ['./forgot.component.scss']
})
export class ForgotComponent implements OnInit {

  forgotForm: FormGroup;

  private emailRegExpPattern = /^([a-z0-9]+(?:[.+_-][a-z0-9]+)*)@([a-z0-9]+(?:[.-][a-z0-9]+)*\.[a-z]{2,})$/;

  email: FormControl = new FormControl('', [
    Validators.required,
    Validators.email,
    Validators.pattern(this.emailRegExpPattern)
  ]);

  formSuccess = false;
  formError = false;
  formSubmitted = false;
  forgetPasswordError = false;
  errorMessage:string;
  captchaErrorMessage:string;

  public forgotPasswordFinished: boolean;

  constructor(
    private authClient: AuthService,
    private recaptchaV3Service: ReCaptchaV3Service,
    private translate: TranslateService,
  ) {
    this.forgotPasswordFinished = true;
    this.captchaErrorMessage=this.translate.instant('auth.login.captcha.error');
  }

  ngOnInit() {
    this.forgotForm = new FormGroup({
      email: this.email
    });
  }

  submitForm() {
    if (this.forgotForm.invalid) {
      return;
    }
    this.forgetPasswordError=false;
    this.recaptchaV3Service.execute('RegistrationSubmit').subscribe((token) => {
      this.authClient.verifyReCaptchaToken({response:token}).subscribe((res)=>{
        if(res.data["success"]==true && res.data["score"]>=.3){
          return this.forgotPassword();
        }
        else{
          this.forgetPasswordError=true;
          this.errorMessage = this.captchaErrorMessage;
        }
      })
    })


  }

  async forgotPassword(): Promise<void> {

    this.forgotPasswordFinished = false;
    await this.authClient
      .initiateForgotPassword({email: this.email.value})
      .pipe(finalize(() => {
        this.forgotPasswordFinished = true;
        this.formSubmitted = true;
      }))
      .subscribe(() => {
        this.formSuccess = true;
      }, () => {
        this.formError = true;
      });

  }

  // Zwróć TRUE, jeśli element FormControl nie jest poprawny po wprowadzeniu zmian
  hasError() {
    const e = this.email;
    return e && (e.dirty || e.touched) && !e.valid;
  }

}
