import {Component, OnInit} from '@angular/core';
import {NavigationEnd, Router} from '@angular/router';

import {TranslateService} from '@ngx-translate/core';
import {LanguageCookieService} from './services/language-cookie.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Mosaico';

  constructor(
    private router: Router,
    private translate: TranslateService,
    // private appConfigurationService: AppConfigurationService,
    private languageCookieService: LanguageCookieService
  ) {
    // this language will be used as a fallback when a translation isn't found in the current language
    translate.setDefaultLang('pl');

    // the lang to use, if the lang isn't available, it will use the current loader to get them
    translate.use('pl');

    this.languageCookieService.initLang();
  }

  ngOnInit() {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });

    // this.appConfigurationService.init();
  }
}
