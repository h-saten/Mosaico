import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {AppConfigurationService} from "../../../services/app-configuration.service";
import {AuthService} from '../../../services/auth.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {

  private userId: string;
  private code: string;

  public confirmationSuccess = false;

  public confirmationRequestFinished: boolean;

  redirectUrl = '';

  constructor(
    private route: ActivatedRoute,
    private authClient: AuthService,
    private appConfigurationService: AppConfigurationService,
  ) {
    this.confirmationRequestFinished = true;
  }

  ngOnInit() {

    this.userId = this.route.snapshot.queryParamMap.get('userId');
    this.code = this.route.snapshot.queryParamMap.get('code');

    this.confirm();

  }

  confirm() {

    this.confirmationRequestFinished = false;

    this.authClient
      .confirmEmail(this.userId, this.code)
      .pipe(finalize(() => {
        this.confirmationRequestFinished = true;
      }))
      .subscribe(() => {
        this.confirmationSuccess = true;
        this.redirectUrl = this.appConfigurationService.appUrl();
      });
  }

}
