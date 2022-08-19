import {Component, Inject, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {DOCUMENT} from '@angular/common';
import {ActivatedRoute, Router} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';
import {ExternalProviderLoginService} from '../../../services/external-provider-login.service';
import {
  AuthService,
  DeviceAuthorizationType,
  LoginCommandResponse,
  LoginResponseType
} from '../../../services/auth.service';
import {ReCaptchaV3Service} from 'ng-recaptcha';
import {SubSink} from 'subsink';
import {DeviceAuthorizationComponent} from "./device-authorization/device-authorization.component";

@Component({
  selector: 'app-social-login',
  templateUrl: './social-login.component.html',
  styleUrls: ['./social-login.component.scss']
})
export class SocialLoginComponent implements OnInit, OnDestroy {

  @ViewChild('deviceAuthorizationModal') public deviceAuthorizationModal: DeviceAuthorizationComponent;

  private subs: SubSink = new SubSink();

  public invalidLoginData = false;
  public deactivatedLogin = false;
  public loginError = false;
  public captchaError = false;
  public lockedAccount = false;
  public dataSavingRequestInProgress: boolean;

  public showFacebookLogin = true;
  public showGoogleLogin = true;

  email = new FormControl('', [
    Validators.required,
    Validators.email,
    Validators.pattern(/^([a-z0-9]+(?:[.+_-][a-z0-9]+)*)@([a-z0-9]+(?:[.-][a-z0-9]+)*\.[a-z]{2,})$/)
  ]);

  password = new FormControl('', [
    Validators.required,
  ]);

  returnUrl: string;

  loginForm: FormGroup;

  kangaEnabled = false;
  googleEnabled = false;
  facebookEnabled = false;

  canInitiateLoginAction = true;
  deviceAuthorizationFailureReason: string;

  deviceAuthorizationType: DeviceAuthorizationType;

  constructor(
    private formBuilder: FormBuilder,
    private authClient: AuthService,
    @Inject(DOCUMENT) private document: Document,
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient,
    private recaptchaV3Service: ReCaptchaV3Service,
    private externalLogin: ExternalProviderLoginService
  ) {
    this.loginForm = this.formBuilder.group({
      email: this.email,
      password: this.password
    });
    this.dataSavingRequestInProgress = false;
  }
  ngOnInit() {
    const endOfUriPosition = this.router.url.indexOf('?');
    let routeUri = '';
    if (endOfUriPosition === -1) {
      routeUri = this.router.url.slice(0);
    } else {
      routeUri = this.router.url.slice(0, endOfUriPosition);
    }

    if (routeUri === '/fb') { this.showFacebookLogin = true; }
    if (routeUri === '/google') { this.showGoogleLogin = true; }
    this.getReturnAfterLoginUrl();
    this.returnUrl = this.route.snapshot.queryParamMap.get('ReturnUrl');

    this.kangaEnabled = this.externalLogin.isKangaLoginEnabled();
    this.facebookEnabled = this.externalLogin.isFacebookLoginEnabled();
    this.googleEnabled = this.externalLogin.isGoogleLoginEnabled();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  private decodeQueryParam(p): string {
    return decodeURI(p);
  }
  private getParameterByName(name, url): string {
    if (!url) { url = window.location.href; }
    name = name.replace(/[\[\]]/g, '\\$&');
    const regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)');
    const results = regex.exec(url);
    if (!results) { return null; }
    if (!results[2]) { return ''; }
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
  }

  getReturnAfterLoginUrl(): string {
    const doubleDecodedUrl = this.decodeQueryParam(this.decodeQueryParam(this.router.url));
    const loginFlowReturnUrl = this.getParameterByName('ReturnUrl', doubleDecodedUrl);
    let returnUrlValue: string = this.getParameterByName('returnUrlValue', doubleDecodedUrl);
    if (!returnUrlValue && loginFlowReturnUrl) {
      returnUrlValue = this.getParameterByName('returnUrlValue', loginFlowReturnUrl);
    }
    return returnUrlValue;
  }

  login(deviceAuthorizationCode?: string) {
    if (!this.loginForm.valid)
    {
      return;
    }
    this.dataSavingRequestInProgress = true;
    this.captchaError=false;
    this.subs.sink = this.recaptchaV3Service.execute('loginSubmit').subscribe((token) => {
      this.authClient.verifyReCaptchaToken({response:token}).subscribe((res)=>{
        if(res.data["success"] == true && res.data["score"] >= .3){
          this.authClient.login({
            email: this.email.value,
            password: this.password.value,
            remember: false,
            returnUrl: this.returnUrl,
            authorizeDeviceCode: deviceAuthorizationCode
          })
          .pipe(finalize(() => {
            this.dataSavingRequestInProgress = false;
          }))
          .subscribe((response) => {
            this.userFindedAction(response?.data);
          }, (error) => {
            console.log(error);
          });
        }
        else{
          this.dataSavingRequestInProgress = false;
          this.captchaError=true;
        }
      })
    });

  }

  userFindedAction(response: LoginCommandResponse)
  {
    if(response){
      this.deactivatedLogin = false;
      this.invalidLoginData = false;
      this.lockedAccount = false;
      const urlToRedirect = response.result.defaultRedirect;
      switch (response.result.type) {
        case LoginResponseType.Succeeded:
          this.document.location.href = urlToRedirect;
          break;
        case LoginResponseType.InvalidData:
          this.invalidLoginData = true;
          break;
        case LoginResponseType.Deactivated:
          this.deactivatedLogin = true;
          break;
        case LoginResponseType.LockedOut:
          this.lockedAccount = true;
          break;
        case LoginResponseType.RequiresTwoFactor:
          this.deviceAuthorizationType = response.result.deviceAuthorization.deviceVerificationType;
          this.deviceAuthorizationModal.openDialog(response.result.deviceAuthorization);
          this.canInitiateLoginAction = response.result.deviceAuthorization.canGenerateNewCode;
          this.deviceAuthorizationFailureReason = response.result.deviceAuthorization.failureReason;
          break;
      }
    }
  }

  show_hide_password (e)
  {
    e.preventDefault();
    const i_pass = document.getElementById('password');
    const b_name = document.getElementById('password-toggler');
    if (i_pass && b_name)
    {
      if (i_pass.getAttribute('type') === 'password') {
        i_pass.setAttribute('type', 'text');
        b_name.innerHTML = '<i class=\'fas fa-eye\'></i>';
      } else {
        i_pass.setAttribute('type', 'password');
        b_name.innerHTML = '<i class=\'fas fa-eye-slash\'></i>';
      }
    }
  }

  getFormControl(name: string) {
    return this.loginForm.get(name);
  }

  isValid(name: string) {
    const e = this.getFormControl(name);
    return e && e.valid;
  }

  hasError(name: string) {
    const e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
  }

  showLoginError(): boolean {
    return (this.invalidLoginData || this.loginError || this.deactivatedLogin || this.lockedAccount || this.captchaError || this.deviceAuthorizationFailureReason?.length > 0);
  }

  FacebookLogin() {
    return this.externalLogin.FacebookLogin();
  }

  GoogleLogin() {
    return this.externalLogin.GoogleLogin();
  }

  KangaLogin() {
    return this.externalLogin.KangaLogin();
  }

  logInWithDeviceAuthorizationCode(authorizationCode: string): void {
    this.login(authorizationCode);
  }
}
