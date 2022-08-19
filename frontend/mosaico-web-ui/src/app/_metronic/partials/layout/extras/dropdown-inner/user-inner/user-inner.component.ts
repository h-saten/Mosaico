import { UserInformation } from './../../../../../../modules/user-management/models/user-information';
import { DOCUMENT } from '@angular/common';
import { Component, HostBinding, Inject, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { selectUserInformation, selectIsAuthorized } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
import { LanguageFlag, languages } from '../../../../../../modules/shared/models';
import { UserService } from 'src/app/modules/user-management/services';
import { AuthService, Counter, CounterService, CountersHubService, TranslationService, BlockchainService } from 'mosaico-base';
import { setMetamaskConnected } from 'src/app/modules/wallet';
import { StonlyService } from 'src/app/services/stonly.service';

@Component({
  selector: 'app-user-inner',
  templateUrl: './user-inner.component.html',
})
export class UserInnerComponent implements OnInit, OnDestroy {
  @HostBinding('class')
  class = `menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg menu-state-primary fw-bold py-4 fs-6 w-275px`;
  @HostBinding('attr.data-kt-menu') dataKtMenu = 'true';

  language: LanguageFlag;
  user: UserInformation;
  langs = languages;
  isAuthorized$: Observable<boolean>;
  isLoaded = false;
  interval: NodeJS.Timeout;

  private subs = new SubSink();
  counters = {
    projects: 0,
    invitations: 0,
    companies: 0
  };

  constructor(
    private store: Store,
    public auth: AuthService,
    public userService: UserService,
    @Inject(DOCUMENT) private doc: Document,
    private translationService: TranslationService,
    private counterService: CounterService,
    private hubService: CountersHubService,
    private blockchainService: BlockchainService,
    private stonlyService: StonlyService
  ) {}

  ngOnInit(): void {
    this.isAuthorized$ = this.store.select(selectIsAuthorized);
    this.store.select(selectUserInformation).subscribe((res) => {
      if (res && res.id && res.id.length > 0) {
        this.user = res;
        this.getCounters();
      }
    }); 
    this.hubService.startConnection();
    this.hubService.addListener();
    this.subs.sink = this.hubService.counter$.subscribe((c) => {
      if(c) {
        this.counters[c.key] = c.value;
      }
    });
    this.setLanguage(this.translationService.getSelectedLanguage());
  }

  async logout() {
    try{
      // this.store.dispatch(setMetamaskConnected({ isConnected: false }));
      // await this.blockchainService.cleanup();
      this.auth.logout();
    }
    catch {
      this.auth.logout();
    }
  }

  selectLanguage(lang: string): void {
    this.translationService.setLanguage(lang);
    this.setLanguage(lang);
    this.stonlyService.setLanguage(lang);
  }

  setLanguage(lang: string) {
    this.langs.forEach((language: LanguageFlag) => {
      if (language.lang === lang) {
        language.active = true;
        this.language = language;
      } else {
        language.active = false;
      }
    });
  }

  getCounters(force = false): void {
    if (!this.isLoaded || force === true) {
      this.subs.sink = this.counterService.get().subscribe((res) => {
        if (res && res.data) {
          if (res.data.counters) {
            res.data.counters.forEach(counter => {
              this.counters[counter.key] = counter.value;
            });
          }
          this.isLoaded = true;
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    this.hubService.removeListener();
  }
}
