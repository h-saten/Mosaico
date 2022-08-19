import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { selectIsAuthorized } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
import { LayoutService } from '../../core/layout.service';
import { selectUserPhotoUrl } from '../../../../modules/user-management/store/user.selectors';
import { AuthService } from 'mosaico-base';
import { MenuComponent } from 'src/app/_metronic/kt/components';
import { LanguageFlag } from 'src/app/modules/shared/models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss'],
})
export class TopbarComponent implements OnInit, OnDestroy {
  @Input() isClosed: Observable<boolean>;
  isAuthorized$: Observable<boolean>;

  private subs: SubSink = new SubSink();
  
  headerLeft = 'menu';
  userPhotoUrl: string;
  isCollapse = false;

  language: LanguageFlag;

  constructor(
    private layout: LayoutService,
    private store: Store,
    public auth: AuthService,
    private router: Router
  ) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.headerLeft = this.layout.getProp('header.left') as string;
    this.isAuthorized$ = this.store.select(selectIsAuthorized);
    this.subs.sink = this.store.select(selectUserPhotoUrl).subscribe((res) => {
      this.userPhotoUrl = res;
    });

    this.subs.sink = this.isClosed.subscribe(_ => {
      this.isCollapse = false
    })
  }

  // html
  loginWithRedirect(): void {
    this.auth.loginWithRedirect(this.router.url);
  }


  toggleButton(): void {
    this.isCollapse = !this.isCollapse;
  }

  hideMenu(): void {
    MenuComponent.hideDropdowns(null);
  }

}
