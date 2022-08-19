import { Token } from 'mosaico-wallet';
import { Component, Input, OnInit } from '@angular/core';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { SubSink } from 'subsink';
import { LanguageEnum } from 'src/app/modules/shared/models';

@Component({
  selector: 'app-project-info',
  templateUrl: './project-info.component.html',
  styleUrls: ['./project-info.component.scss']
})
export class ProjectInfoComponent implements OnInit {

  @Input() tokenPrice: number;
  @Input() token: Token;

  @Input() startDate: Date;
  @Input() endDate: Date;

  private subs: SubSink = new SubSink();

  showPolishDate = false;

  currentLang = '';
  languageEnum: typeof LanguageEnum = LanguageEnum;

  constructor(
    private translateService: TranslateService
  ) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.getLanguage();
  }

  private getLanguage(): void {
    this.currentLang = this.translateService.currentLang;

    this.showPolishDateFunc(this.currentLang);

    this.subs.sink = this.translateService.onLangChange
      .subscribe((langChangeEvent: LangChangeEvent) => {
        this.currentLang = langChangeEvent.lang;

        this.showPolishDateFunc(this.currentLang);
      });
  }


  /**
   * TODO create a separate language service and switch from country selection
   * https://refactoring.guru/design-patterns/adapter/typescript/example
   * @param lang
   */
  private showPolishDateFunc(lang: string): void {
    if (lang === 'pl') {
      this.showPolishDate = true;
    } else {
      this.showPolishDate = false;
    }
  }

}
