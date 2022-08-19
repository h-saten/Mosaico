import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { TranslationService } from 'mosaico-base';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-lang',
  templateUrl: './lang.component.html',
  styleUrls: ['./lang.component.scss']
})
export class LangComponent implements OnInit, OnDestroy {
  currentLang = '';
  subs = new SubSink();
  supportedLanguages = ['pl', 'en'];

  constructor(private translateService: TranslateService, private translationService: TranslationService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.getLanguage();
    this.router.events.subscribe((val) => {
      if(val instanceof NavigationStart) {
        const navigationStartEvent = val as NavigationStart;
        this.currentLang = this.translateService.currentLang;
        if(this.currentLang && this.currentLang.length > 0 && this.supportedLanguages.includes(this.currentLang)) {
          const initialLang = navigationStartEvent.url?.split('/')[1];
          if((!initialLang || initialLang.length === 0 || !this.supportedLanguages.includes(initialLang))){
            let url = this.currentLang;
            if(navigationStartEvent.url !== '/') {
              url += navigationStartEvent.url;
            }
            this.router.navigateByUrl(url);
          }
        }
      }
    });
  }

  private getLanguage(): void {
    this.currentLang = this.translateService.currentLang;
    const data = this.activatedRoute.snapshot.data;
    if(data?.lang && data?.lang.length > 0 &&  this.supportedLanguages.includes(data.lang)) {
      this.translationService.setLanguage(data.lang);
    }
    this.subs.sink = this.translateService.onLangChange
      .subscribe((langChangeEvent: LangChangeEvent) => {
        this.currentLang = langChangeEvent.lang;
        //this.tpTagsService.getMetaTagsOtherSite (this.currentLang, this.activatedRouteData);
      });
  }

}
