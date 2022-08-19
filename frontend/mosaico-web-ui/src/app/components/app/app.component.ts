import { SubSink } from 'subsink';
import { UserService } from './../../modules/user-management/services/user.service';
import { RoleService } from './../../modules/role-manager/services/role.service';
import { setAuthorized } from 'src/app/modules/user-management/store';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { setUserInformation } from '../../modules/user-management/store/user.actions';
import { locale as enLangEn } from '../../modules/i18n/vocabs/en';
import { locale as enLangPl } from '../../modules/i18n/vocabs/pl';
import { filter } from "rxjs/operators";
import { ROLES } from '../../constants';
import { WalletService } from 'mosaico-wallet';
import { AuthService, TranslationService } from 'mosaico-base';
import { selectBlockchain } from 'src/app/store/selectors';
import { UserInformation } from 'src/app/modules/user-management/models';
import { setWalletBalance } from 'src/app/modules/wallet';
import { setWalletInfo } from '../../modules/wallet/store/wallet.actions';
import { NavigationEnd, NavigationError, NavigationStart, Router, ActivatedRoute } from '@angular/router';
import { setCurrentUrl } from 'src/app/store/actions';
import { StonlyService } from '../../services/stonly.service';
import { AffiliationService } from 'mosaico-project';
import Passbase from "@passbase/button";

@Component({
  selector: '[root]',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  private subs: SubSink = new SubSink();
  public isLoading = true;
  userId: string;
  userFetchAttempts = 0;
  userInformation: UserInformation;
  isAuthorized = false;

  constructor(
    private roleManager: RoleService,
    private store: Store,
    private userService: UserService,
    private translationService: TranslationService,
    private walletService: WalletService,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private stonly: StonlyService,
    private affiliationService: AffiliationService
  ) {
    this.translationService.loadTranslations(
      enLangEn, enLangPl
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  async ngOnInit(): Promise<void> {
    this.getCurrentUrlFromRouting();
    this.subscribeToRefCode();
    try {
      this.authService.sessionExpired$.subscribe((res) => {
        if(res === true) {
          this.authService.logout();
        }
      });
      await this.authService.init();
      await this.authService.tryLogin();
      this.subs.sink = this.authService.isAuthenticated$.subscribe((res) => {
        if (res === true) {
          this.roleManager.initRoles();
          this.store.dispatch(setAuthorized({ isAuthorized: true }));
          this.isAuthorized = true;
          const userRoles = this.authService.getUserRoles();
          if (userRoles && userRoles.includes("tokenizer.globaladmin")) {
            this.roleManager.addRole(ROLES.ADMIN);
          }
          this.getUser(this.authService.getUserId());
        }
        this.isLoading = false;
      });
    }
    catch (error) {
      this.store.dispatch(setAuthorized({ isAuthorized: false }));
      this.isLoading = false;
    }
  }

  private getWallets(userId: string): void {
    this.subs.sink = this.store.select(selectBlockchain).subscribe((b) => {
      this.subs.sink = this.walletService.getTokenBalance(userId, b).subscribe((res) => {
        if (res && res.data) {
          this.store.dispatch(setWalletBalance(res.data));
          this.store.dispatch(setWalletInfo({ address: res.data.address, network: b }));
        }
      });
    });
  }

  private getUser(id: string): void {
    this.subs.sink = this.userService.getUser(id).subscribe((res) => {
      if (res && res.data && res.data.id) {
        this.userInformation = res.data;
        this.store.dispatch(setUserInformation(res.data));
        this.stonly.identify(this.userInformation);
        this.getWallets(res.data.id);
        this.saveReferenceCode();
        if (!this.userInformation.evaluationCompleted) {
          this.router.navigateByUrl('/user/evaluation');
        }
        else {
          const url = localStorage.getItem('LOGIN_REDIRECT_URL');
          if (url && url.length > 0) {
            this.router.navigateByUrl(url);
            localStorage.removeItem('LOGIN_REDIRECT_URL');
          }
        }
      }
    }, (error) => {
      this.store.dispatch(setAuthorized({ isAuthorized: false }));
      this.isLoading = false;
    }, () => { this.isLoading = false; });
  }

  private getCurrentUrlFromRouting(): void {
    this.router.events.pipe(filter(ev => (ev instanceof NavigationEnd))).subscribe((res) => {
      if (res && res as NavigationEnd) {
        this.saveCurrentUrl((res as NavigationEnd).url);
      }
    });
  }

  private saveCurrentUrl(currentUrl: string): void {
    if (currentUrl) {
      this.store.dispatch(setCurrentUrl({ currentUrl }));
    }
  }

  private subscribeToRefCode(): void {
    try {
      this.route.queryParams.subscribe((r) => {
        const refCode = this.route.snapshot.queryParamMap.get('refCode');
        if (refCode && refCode.length > 0) {
          localStorage.setItem('refCode', refCode);
          this.saveReferenceCode();
        }
      });
    }
    catch (e) {

    }
  }

  private saveReferenceCode(): void {
    const refCode = localStorage.getItem('refCode');
    if (this.authService.isAuthenticated() === true && refCode?.length > 0) {
      this.affiliationService.addReference({ refCode }).subscribe();
    }
  }

}
