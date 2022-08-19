import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { TranslationService } from 'mosaico-base';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { LanguageFlag, languages } from 'src/app/modules/shared/models';
import { UserService } from 'src/app/modules/user-management/services';
import { selectIsAuthorized } from 'src/app/modules/user-management/store';
import { MenuComponent } from 'src/app/_metronic/kt/components';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-language-switch',
  templateUrl: './language-switch.component.html',
  styleUrls: ['./language-switch.component.scss']
})
export class LanguageSwitchComponent implements OnInit {

  language: LanguageFlag;
  langs = languages;
  subs = new SubSink();
  isAuthorized$: Observable<boolean>;

  constructor(private translationService: TranslationService, private translateService: TranslateService, private store: Store, private userService: UserService) { }

  ngOnInit(): void {
    this.isAuthorized$ = this.store.select(selectIsAuthorized);
    this.getLanguage();
  }

  private getLanguage(): void {

    this.language = this.translationService.getLanguage(this.translateService.currentLang);

    this.subs.sink = this.translateService.onLangChange
      .subscribe((langChangeEvent: LangChangeEvent) => {
        this.language = this.translationService.getLanguage(langChangeEvent.lang);
      });
  }

  // htmnl
  selectLanguage(lang: string): void {
    this.translationService.setLanguage(lang);
    this.hideMenu();
    try{
      this.subs.sink = this.isAuthorized$.pipe(take(1)).subscribe((res) => {
        if(res === true){
          this.subs.sink = this.userService.updateLanguage(lang).subscribe(() => {
          });
        }
      });
    }
    catch(error){
      // ignore
    }
  }

  hideMenu(): void {
    MenuComponent.hideDropdowns(null);
  }
}
