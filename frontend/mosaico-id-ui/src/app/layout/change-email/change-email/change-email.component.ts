import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { finalize } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-change-email',
  templateUrl: './change-email.component.html',
  styleUrls: ['./change-email.component.scss']
})
export class ChangeEmailComponent implements OnInit {
  code = '';
  userId = '';
  email = '';
  private logoutId: string;
  public emailError = false;
  public emailSuccess = false;

  public emailChangeRequestFinished: boolean=false;

  constructor(
    private apiAccountClient: AuthService,
    private route: ActivatedRoute,
    private translate: TranslateService,
    @Inject(DOCUMENT) private document: Document
  ) {
  }

  ngOnInit(): void {
    this.logoutId = this.route.snapshot.queryParamMap.get('logoutId');
    this.code = this.route.snapshot.queryParamMap.get('code');
    this.userId = this.route.snapshot.queryParamMap.get('userId');
    this.email = this.route.snapshot.queryParamMap.get('email');

    this.changeEmail();

  }

  isEngLang(): boolean {
    return this.translate.currentLang !== 'pl';
  }

  changeEmail()
  {
    this.emailChangeRequestFinished = false;
    this.apiAccountClient
      .changeEmail({
        email: this.email,
        code: this.code,
        userId: this.userId
      })
      .pipe(finalize(() => {
        setTimeout(() => {
          this.emailChangeRequestFinished = true;
        }, 1000);
      }))
      .subscribe((success) => {
        setTimeout(() => {
          this.emailSuccess = true;
          this.logout();
        }, 1000);
      }, (error) => {
        setTimeout(() => {
          this.emailError = true;
        }, 1000);
      });
  }

  logout() {
    this.apiAccountClient
      .logout({
        logoutId: this.logoutId
      })
      .subscribe((response) => {
        if(response?.data?.postLogoutRedirectUri && response?.data?.postLogoutRedirectUri.length > 0){
          this.document.location.href = response.data.postLogoutRedirectUri;
        }
      });
  }

}
