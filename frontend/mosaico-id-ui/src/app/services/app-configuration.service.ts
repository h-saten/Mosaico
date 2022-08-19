import {Injectable, OnDestroy} from '@angular/core';
import {AppConfigurationDto} from './tokenizer-auth-api.service';
import {SubSink} from 'subsink';
import {Observable, tap} from 'rxjs';
import {DefaultHeaders, SuccessResponse} from '../utils';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppConfigurationService implements OnDestroy {
  private subs: SubSink = new SubSink();
  private _kangaLoginUrl: string;
  private _kangaBaseUrl: string;
  private _mosaicoAppUrl: string;
  private baseUrl: string;
  private logoutReturnUrl: string;
  private facebookAuthenticationEnabled: boolean;
  private googleAuthenticationEnabled: boolean;
  private kangaAuthenticationEnabled: boolean;
  private _recaptchaSiteKey: string;
  private _gatewayUrl: string;

  constructor(
    private httpClient: HttpClient
  ) {
    this.baseUrl = '';
  }

  public getConfiguration(): Observable<SuccessResponse<AppConfigurationDto>> {
    return this.httpClient.get<SuccessResponse<AppConfigurationDto>>(`${this.baseUrl}/api/configurations`, DefaultHeaders).pipe(tap((res) => {
        if(res && res.data){
          this.updateProps(res.data);
        }
    }));
  }

  private updateProps(response: AppConfigurationDto){
    this._mosaicoAppUrl = response.mainUrl;
    this._kangaBaseUrl = response.kangaBaseUrl;
    this._kangaLoginUrl = response.kangaBaseUrl + '/oauth/' + response.kangaAppId;
    this.logoutReturnUrl = response.afterLogoutUrl;
    this.facebookAuthenticationEnabled = response.facebookAuthenticationEnabled;
    this.googleAuthenticationEnabled = response.googleAuthenticationEnabled;
    this.kangaAuthenticationEnabled = response.kangaAuthenticationEnabled;
    this._recaptchaSiteKey = response.recaptchaSiteKey;
    this._gatewayUrl = response.gatewayUrl;
  }

  public gatewayUrl(): string {
    return this._gatewayUrl;
  }

  public recaptchaSiteKey(): string {
    return this._recaptchaSiteKey;
  }

  public afterLogoutUrl(): string {
    return this.logoutReturnUrl;
  }

  public kangaLoginUrl(): string {
    return this._kangaLoginUrl;
  }

  public mosaicoAppUrl(): string {
    return this._mosaicoAppUrl;
  }

  public appUrl(): string {
    return this.baseUrl;
  }

  public isFacebookEnabled(): boolean {
    return this.facebookAuthenticationEnabled;
  }

  public isGoogleEnabled(): boolean {
    return this.googleAuthenticationEnabled;
  }

  public isKangaEnabled(): boolean {
    return this.kangaAuthenticationEnabled;
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

}
