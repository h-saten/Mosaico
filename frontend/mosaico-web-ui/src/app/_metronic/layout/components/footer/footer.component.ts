import { Component, OnDestroy, OnInit } from '@angular/core';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { SubSink } from 'subsink';
// import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
// import { LanguageEnum } from 'src/app/modules/shared/models';
// import { SubSink } from 'subsink';
import { LayoutService } from '../../core/layout.service';

interface DataA {
  titleA: string;
  urlA: string;
  external?: boolean;
}

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
})
export class FooterComponent implements OnInit, OnDestroy {

  footerContainerCssClasses = '';
  currentDateStr: string = new Date().getFullYear().toString();

  currentLanguage = '';
  // languageEnum: typeof LanguageEnum = LanguageEnum;

  sub: SubSink = new SubSink();

  showDescription: boolean[] = new Array<boolean>();
  expandedRow: string[] = new Array<string>();

  title = '';
  description = '';
  i: number;

  menuCompany: DataA[] = null;
  menuInvestors: DataA[] = null;
  menuPartners: DataA[] = null;
  menuHelp: DataA[] = null;
  menuOurOffice: DataA[] = null;

  constructor(
    private layout: LayoutService,
    private translateService: TranslateService,
  ) {}

  ngOnInit(): void {
    this.footerContainerCssClasses = this.layout.getStringCSSClasses('footerContainer');

    this.getLanguage();
    this.getMenus();
  }

  ngOnDestroy(): void {
    // this.sub.unsubscribe();
  }

  private getLanguage(): void {
    this.currentLanguage = this.translateService.currentLang;

    this.sub.sink = this.translateService.onLangChange
      .subscribe((langChangeEvent: LangChangeEvent) => {
        this.currentLanguage =  this.translateService.currentLang;
      });
  }

  showHideDescription(i: number): void {
    this.showDescription[i] = !this.showDescription[i];
    if (this.showDescription[i] === true) {
      this.expandedRow[i] = 'expanded';

    } else {
      this.expandedRow[i] = '';
    }
  }

  private getMenus(): void {
    this.menuCompany = [
      {
        titleA: 'FOOTER.career',
        external: true,
        urlA: 'https://holdingsapiency.traffit.com/career/'
      },
      {
        titleA: 'FOOTER.news',
        external: true,
        urlA: 'https://news.mosaico.ai/'
      }
    ];

    this.menuInvestors = [
      {
        titleA: 'MENU.MARKETPLACE',
        urlA: '/projects'
      },
      {
        titleA: 'MENU.DAO',
        urlA: '/dao'
      },
      {
        titleA: 'MENU.EXCHANGE',
        urlA: '/dex'
      },
    ];

    this.menuPartners = [
      {
        titleA: 'MENU.NEW_PROJECT',
        urlA: '/project/create'
      },
      {
        titleA: 'MENU.FUND',
        urlA: '/portfolio'
      }
    ];

    this.menuHelp = [
      {
        titleA: 'FOOTER.dictionary',
        external: true,
        urlA: 'https://advisor.mosaico.ai'
      },
      {
        titleA: 'FOOTER.report',
        urlA: ''
      },
      {
        titleA: 'FOOTER.suggest',
        urlA: ''
      }
    ];
  }
}
