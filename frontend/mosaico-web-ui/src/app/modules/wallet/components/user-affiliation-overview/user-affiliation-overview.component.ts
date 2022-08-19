import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AffiliationProject, AffiliationService } from 'mosaico-project';
import { SubSink } from 'subsink';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlingService } from 'mosaico-base';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-user-affiliation-overview',
  templateUrl: './user-affiliation-overview.component.html',
  styleUrls: ['./user-affiliation-overview.component.scss']
})
export class UserAffiliationOverviewComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  projects: AffiliationProject[] = [];
  accessCode: string;
  isLoading$ = new BehaviorSubject<boolean>(false);
  accessCodeLink: string;

  constructor(private affiliationService: AffiliationService, private store: Store, private translateService: TranslateService, private toastr: ToastrService,
    private errorHandler: ErrorHandlingService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.isLoading$.next(true);
    this.subs.sink = this.affiliationService.getUserAffiliation().subscribe((res) => {
      this.accessCode = res?.data?.accessCode;
      this.projects = res?.data?.projects;
      this.accessCodeLink = window.location.origin + `?refCode=${this.accessCode}`;
      this.isLoading$.next(false);
    }, (error) => {
      this.isLoading$.next(false);
      this.errorHandler.handleErrorWithToastr(error);
    });
  }

  onCopied(): void {
    this.toastr.success('Code copied');
  }

}
