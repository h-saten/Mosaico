import {Inject, Injectable} from '@angular/core';
import {ActivatedRoute, Params} from '@angular/router';
import {DOCUMENT} from '@angular/common';
import {HttpClient} from "@angular/common/http";
import {AppConfigurationService} from "./app-configuration.service";

@Injectable({
  providedIn: 'root'
})
export class ExternalProviderLoginService  {

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private appConfigurationService: AppConfigurationService,
    @Inject(DOCUMENT) private document: Document
  ) {
  }

  private static EncodeQueryData(data) {
    const ret = [];

    for (const d in data) {
      ret.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]));
    }
    return ret.join('&');
  }

  FacebookLogin() {
    if (!this.appConfigurationService.isFacebookEnabled()) return;
    this.route.queryParams.subscribe((params: Params) => {
      const returnUrl = params['ReturnUrl'] ? params['ReturnUrl'] : '';
      const data = { 'provider': 'Facebook', 'returnUrl': returnUrl };
      const querystring = ExternalProviderLoginService.EncodeQueryData(data);
      let identityProviderUrl = this.appConfigurationService.appUrl();
      const url = `${identityProviderUrl}/External/Challenge?${querystring}`;
      return this.document.location.replace(url);
    });
  }

  GoogleLogin() {
    if (!this.appConfigurationService.isGoogleEnabled()) return;
    this.route.queryParams.subscribe((params: Params) => {
      const returnUrl = params['ReturnUrl'] ? params['ReturnUrl'] : '';
      const data = { 'provider': 'Google', 'returnUrl': returnUrl };
      const querystring = ExternalProviderLoginService.EncodeQueryData(data);
      let identityProviderUrl = this.appConfigurationService.appUrl();
      const url = `${identityProviderUrl}/External/Challenge?${querystring}`;
      return this.document.location.replace(url);
    });
  }

  KangaLogin() {
    if (!this.appConfigurationService.isKangaEnabled()) return;
    return document.location.href = this.appConfigurationService.kangaLoginUrl();
  }

  isKangaLoginEnabled(): boolean {
    return this.appConfigurationService.isKangaEnabled();
  }

  isFacebookLoginEnabled(): boolean {
    return this.appConfigurationService.isFacebookEnabled();
  }

  isGoogleLoginEnabled(): boolean {
    return this.appConfigurationService.isGoogleEnabled();
  }

}
