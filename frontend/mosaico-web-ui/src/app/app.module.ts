import { RoleManagerModule } from './modules/role-manager/role-manager.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppComponent } from './components';
import { StoreModule } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import { InlineSVGModule } from 'ngx-svg-inline';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ClipboardModule } from 'ngx-clipboard';
import { MomentModule } from 'ngx-moment';
import { BlockchainInitializer, MoralisInitializer } from './initializers';
import { appReducers } from './store';
import { ToastrModule } from 'ngx-toastr';
import { SigninComponent } from './components/signin/signin.component';
import { registerLocaleData } from '@angular/common';
import localePl from '@angular/common/locales/pl';
import localePlExtra from '@angular/common/locales/extra/pl';
import '@angular/common/locales/global/pl';
import { LanguageInterceptor } from './interceptors/language.interceptor';
import { CookieService } from 'ngx-cookie-service';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { CookieStorage } from 'cookie-storage';
import { environment } from 'src/environments/environment';
import Passbase from "@passbase/button";

registerLocaleData(localePl, 'pl-PL', localePlExtra);

export function storageFactory(): OAuthStorage {
  return new CookieStorage();
}

@NgModule({
  declarations: [
    AppComponent,
    SigninComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    TranslateModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    RoleManagerModule.forRoot(),
    StoreModule.forRoot(appReducers),
    ClipboardModule,
    InlineSVGModule.forRoot(),
    NgbModule,
    MomentModule,
    ToastrModule.forRoot(),
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: environment.auth.secureRoutes,
        sendAccessToken: true
      }
    })
  ],
  providers: [
    CookieService,
    // { provide: LOCALE_ID, useValue: 'pl-PL' },
    { provide: HTTP_INTERCEPTORS, useClass: LanguageInterceptor, multi: true },
    MoralisInitializer,
    BlockchainInitializer,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    { provide: OAuthStorage, useFactory: storageFactory }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }


