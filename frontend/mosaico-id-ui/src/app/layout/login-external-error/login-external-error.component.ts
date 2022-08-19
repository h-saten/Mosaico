import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Params} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-login-external-error',
  templateUrl: './login-external-error.component.html',
  styleUrls: ['./login-external-error.component.scss']
})
export class LoginExternalErrorComponent implements OnInit {

  public provider: string;
  public knownError: string;
  public otherError: string;
  public dataLoaded: boolean;

  private knownProvider = ['Facebook'];
  private knownErrorCodes = ['email_required'];

  constructor(
    private route: ActivatedRoute,
    private translate: TranslateService
  ) {
    console.error('Running constructor in LoginExternalErrorComponent');
    this.provider = null;
    this.knownError = null;
    this.otherError = null;
    this.dataLoaded = false;
  }

  ngOnInit() {
    this.route.queryParams.subscribe((params: Params) => {

      const provider = params.provider;

      if (provider != null && provider.length > 0 && this.knownProvider.find(o => o.toLowerCase() === provider.toLowerCase())) {
        this.provider = provider;
      }

      const error = params.error;

      if (error != null && error.length > 0) {
        if (this.knownErrorCodes.find(o => o.toLowerCase() === error.toLowerCase())) {
          this.knownError = error;
        } else {
          this.otherError = error;
        }
      }

      console.warn(this.provider);
      console.warn(this.knownError);
      console.warn(this.otherError);

      this.dataLoaded = true;

    });

  }

  getKnownErrorDescription(errorCode: string): string {
    if (errorCode === 'email_required') {
      return this.translate.instant('auth.login.external.error_email_required');
    }
  }


}
