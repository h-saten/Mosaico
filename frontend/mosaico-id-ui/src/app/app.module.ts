import {BrowserModule} from '@angular/platform-browser';
import {APP_INITIALIZER, NgModule} from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AngularMaterialModule} from './angular-material.module';

import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {AuthComponent} from './layout/auth/auth.component';
import {SharedModule} from './shared/shared.module';
import {RouterModule} from '@angular/router';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import {ApiAccountClient} from './services/tokenizer-auth-api.service';
import {LoginResolve} from './resolvers/LoginResolve';

import {CookieService} from 'ngx-cookie-service';
import {DataAdministrationInfoComponent} from './layout/data-administration-info/data-administration-info.component';
import {AppConfigurationService} from "./services/app-configuration.service";
import {DefaultInterceptor} from "./interceptors/DefaultInterceptor";
import { RECAPTCHA_V3_SITE_KEY } from 'ng-recaptcha';

// required for AOT compilation
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

export function init_app_configuration(appInitService: AppConfigurationService): Function {
  return (): Promise<any> => {
    return appInitService.getConfiguration().toPromise();
  }
}

@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    DataAdministrationInfoComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AngularMaterialModule,
    AppRoutingModule,
    SharedModule,
    RouterModule.forRoot([], { relativeLinkResolution: 'legacy' }),
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    ApiAccountClient,
    LoginResolve,
    CookieService,
    AppConfigurationService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: DefaultInterceptor,
      multi: true
    },
    { provide: APP_INITIALIZER, useFactory: init_app_configuration, deps: [AppConfigurationService], multi: true },
    { provide: RECAPTCHA_V3_SITE_KEY, useFactory: (s: AppConfigurationService) => s.recaptchaSiteKey(), deps: [AppConfigurationService], multi: false }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
