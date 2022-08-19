import { Component, OnDestroy, OnInit } from '@angular/core';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { LanguageEnum } from 'src/app/modules/shared/models';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-footer-social-icon',
  templateUrl: './footer-social-icon.component.html',
  styleUrls: ['./footer-social-icon.component.scss']
})
export class FooterSocialIconComponent implements OnInit, OnDestroy {

  currentLang = '';
  languageEnum: typeof LanguageEnum = LanguageEnum;

  sub: SubSink = new SubSink();

  constructor(
    private translateService: TranslateService,
  ) { }

  ngOnInit(): void {
    this.getLanguage();
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  private getLanguage(): void {
    this.currentLang = this.translateService.currentLang;

    this.sub.sink = this.translateService.onLangChange
      .subscribe((langChangeEvent: LangChangeEvent) => {
        this.currentLang = langChangeEvent.lang;
      });
  }


}
