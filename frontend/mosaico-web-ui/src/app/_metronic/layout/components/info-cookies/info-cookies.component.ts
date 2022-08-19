import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';


@Component({
  selector: 'app-info-cookies',
  templateUrl: './info-cookies.component.html',
  styleUrls: ['./info-cookies.component.scss']
})
export class InfoCookiesComponent implements OnInit {

  showAlertCookie = false;

  NAME_COOKIE = 'allowcookiemos';

  constructor(
    private cookieService: CookieService,
  ) { }

  ngOnInit(): void {
    this.showAlertCookieFun();
  }

  private showAlertCookieFun(): void {

    if (localStorage.getItem(this.NAME_COOKIE) || this.cookieService.check(this.NAME_COOKIE)) {
      this.showAlertCookie = false;

    } else {

      setTimeout(() => {
        this.showAlertCookie = true;
      }, 1000);
    }
  }

  // run from view
  setCookie(): void {
    const expiresAt = new Date();
    expiresAt.setDate(expiresAt.getDate() + 365);

    this.cookieService.set(this.NAME_COOKIE, '1', expiresAt, '/');

    localStorage.setItem(this.NAME_COOKIE, '1');

    this.showAlertCookie = false;
  }
}
