import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AuthService } from 'mosaico-base';
import { SubSink } from 'subsink';
import { selectIsAuthorized } from '../../../user-management/store/user.selectors';

@Component({
  selector: 'app-button-buy-tokens',
  templateUrl: './button-buy-tokens.component.html',
  styleUrls: ['./button-buy-tokens.component.scss']
})
export class ButtonBuyTokensComponent implements OnInit, OnDestroy {
  @Input() buttonActive: boolean;
  subs = new SubSink();
  isAuthorized = false;

  constructor(private auth: AuthService, private store: Store, private router: Router) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectIsAuthorized).subscribe((r) => {
      this.isAuthorized = r;
    });
  }

  loginIfUnauthorized(): void {
    if (!this.isAuthorized) {
      this.auth.loginWithRedirect(this.router.url);
    }
  }

}