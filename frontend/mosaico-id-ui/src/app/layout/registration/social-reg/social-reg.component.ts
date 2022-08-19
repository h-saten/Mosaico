import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder} from '@angular/forms';
import {catchError, finalize, first} from 'rxjs/operators';
import {ActivatedRoute, Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';
import {AuthService, CreateUserCommand} from '../../../services/auth.service';
import {SubSink} from "subsink";
import {throwError} from "rxjs";

export interface CoreUserRegistrationData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  terms: boolean;
  notForbiddenCitizenship: boolean;
  newsletterAgreement: boolean;
}

@Component({
  selector: 'app-social-reg',
  templateUrl: './social-reg.component.html',
  styleUrls: ['./social-reg.component.scss']
})
export class SocialRegComponent implements OnInit, OnDestroy {

  private subs: SubSink = new SubSink();

  coreData: CoreUserRegistrationData;

  dataSavingRequestInProgress = false;
  registrationServerError = false;
  errorMessage: string;

  constructor(
    private formBuilder: FormBuilder,
    private authClient: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslateService
  ) {
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  private register(): void {
    this.dataSavingRequestInProgress = true;

    this.subs.sink = this.authClient
      .register({
        firstName: this.coreData.firstName,
        lastName: this.coreData.lastName,
        email: this.coreData.email,
        password: this.coreData.password,
        confirmPassword: this.coreData.password,
        newsletterPersonalDataProcessing: this.coreData.newsletterAgreement,
        notForbiddenCitizenship: this.coreData.notForbiddenCitizenship,
        terms: this.coreData.terms,
        language: this.translate.currentLang
      } as CreateUserCommand)
      .pipe(finalize(() => {
        this.dataSavingRequestInProgress = false;
      }))
      .pipe(
        catchError(() => {
          this.registrationServerError = true;
          this.errorMessage = 'Invalid error. Try later.';
          return throwError(() => this.errorMessage);
        })
      )
      .pipe(first())
      .subscribe(() => this.router.navigateByUrl('/auth/registration/success').finally());
  }

  saveFirstStepCoreData(data: CoreUserRegistrationData): void {
    this.coreData = data;
    this.register();
  }
}
