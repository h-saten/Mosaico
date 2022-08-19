import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Language } from 'src/app/interfaces/language.enum';
import { LanguageCookieService } from 'src/app/services/language-cookie.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.scss']
})
export class LanguageSelectorComponent {

  // currentLangLanguage: Language = Language.PL;
  currentLangLanguageEnum: typeof Language = Language;

  constructor(
    private translate: TranslateService,
    private languageCookieService: LanguageCookieService
  ) { }


  public selectLang(lang: Language): void {

    if (lang === Language.PL) {
      this.translate.setDefaultLang('pl');
      this.translate.use('pl');
    }
    else if (lang === Language.EN) {
      this.translate.setDefaultLang('en');
      this.translate.use('en');
    }

    this.languageCookieService.setLangInCookie(lang);
  }

}
