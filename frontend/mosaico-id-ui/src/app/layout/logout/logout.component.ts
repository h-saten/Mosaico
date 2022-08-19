import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ApiAccountClient, LogoutViewModel} from '../../services/tokenizer-auth-api.service';
import {finalize} from 'rxjs/operators';
import {AppConfigurationService} from "../../services/app-configuration.service";
import {DOCUMENT} from "@angular/common";
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {
  private logoutId: string;
  logoutSuccess = false;
  logoutError = false;
  logoutRequestFinished: boolean;
  urlReturn: string;
  constructor(
    private authClient: AuthService,
    private route: ActivatedRoute,
    private appConfigurationService: AppConfigurationService,
    @Inject(DOCUMENT) private document: Document
  ) {
    this.logoutRequestFinished = true;
    this.urlReturn = this.appConfigurationService.afterLogoutUrl();
  }

  ngOnInit() {
    this.logoutId = this.route.snapshot.queryParamMap.get('logoutId');
    this.logout();
  }

  logout() {
    this.logoutRequestFinished = false;
    this.authClient
      .logout({
        logoutId: this.logoutId,
        returnUrl: this.urlReturn
      })
      .pipe(finalize(() => {
        this.logoutRequestFinished = true;
      }))
      .subscribe((response) => {
        this.logoutSuccess = true;
        if(response?.data?.postLogoutRedirectUri && response?.data?.postLogoutRedirectUri.length > 0){
          this.document.location.href = response.data.postLogoutRedirectUri;
        }
      });
  }

}
