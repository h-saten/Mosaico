import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'mosaico-base';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {
  error: boolean;
  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute, private cookieStorage: CookieService) { }

  async ngOnInit() {
    // check for error
    if (this.route.snapshot?.fragment?.indexOf('error') >= 0) {
      this.error = true;
      return;
    }

    let redirectUrl = localStorage.getItem('LOGIN_REDIRECT_URL');
    if(!redirectUrl || redirectUrl.length === 0) {
      redirectUrl = '/';
    }

    try{
      await this.authService.tryLogin();
      if(!this.authService.isAuthenticated()) {
        this.authService.loginWithRedirect();
      }
      else{
        this.router.navigate([redirectUrl]);
      }
    }
    catch(e) {
      this.cookieStorage.deleteAll();
      await this.authService.tryLogin();
    }
  }

}
