import { Injectable } from '@angular/core';
import { StorageMap } from '@ngx-pwa/local-storage';
import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { Language } from '../interfaces/language.enum';

@Injectable({
  providedIn: 'root'
})
export class LanguageCookieService {

  private initedLangFromCookie = false;

  constructor(
    private storage: StorageMap,
    private cookieService: CookieService,
    private translate: TranslateService,
  ) { }


  initLang (): void {
    this.getLangFromCookie();
  }

  setLangInCookie(lang: Language): void {
    this.storage.set( 'lang', lang).subscribe(() => {});
    this.setCookie(lang);
  }

  private getLangFromCookie (): void {
    if (this.initedLangFromCookie === false) {
      this.translate.setDefaultLang('pl');
      this.translate.use('pl');
      this.initLangFromCookie();
      this.initedLangFromCookie = true;
    }
  }

  private initLangFromCookie(): void {
    this.storage.get('lang', { type: 'string'}).subscribe((result) => {
      if (result === Language.PL || result === Language.EN) {
        this.translate.setDefaultLang(result.toLowerCase());
        this.translate.use(result.toLowerCase());
      }
      else {
        const langFromCookie: string = this.getCookie();
        if (langFromCookie === Language.PL || langFromCookie === Language.EN) {
          this.translate.setDefaultLang(langFromCookie.toLowerCase());
          this.translate.use(langFromCookie.toLowerCase());
        }
        else {
          this.translate.setDefaultLang('en');
          this.translate.use('pl');
        }
      }

    }, (error) => {
      console.error('Status error', error);
    });
  }

  private setCookie(lang: Language): void {
    const expiresAt = new Date();
    expiresAt.setDate(expiresAt.getDate() + 365);
    this.cookieService.set('lang', lang, expiresAt, '/');
  }

  private getCookie(): string {
    if (this.cookieService.check('lang')) {
      return this.cookieService.get('lang');
    }
    return null;
  }
}
