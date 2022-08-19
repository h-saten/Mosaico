import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { Company, CompanyService, Proposal, VoteResult } from 'mosaico-dao';
import { DaoCreationHubService, Token, TokenService } from 'mosaico-wallet';
import { BehaviorSubject, Observable } from 'rxjs';
import { SubSink } from 'subsink';
import { selectCompanyPreview } from '../../store/business.selectors';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlingService } from 'mosaico-base';
import { selectUserInformation, selectIsAuthorized } from '../../../user-management/store/user.selectors';

@Component({
  selector: 'app-company-voting',
  templateUrl: './company-voting.component.html',
  styleUrls: ['./company-voting.component.scss']
})
export class CompanyVotingComponent implements OnInit, OnDestroy {
  proposals: Proposal[] = [];
  isLoading$ = new BehaviorSubject<boolean>(true);
  private sub = new SubSink();
  company: Company;
  tokens: Token[] = [];
  today = new Date();
  isSubmitting$ = new BehaviorSubject<boolean>(false);
  isAuthorized$: Observable<boolean>;

  constructor(private store: Store, private companyService: CompanyService, private tokenService: TokenService, private hub: DaoCreationHubService,
    private translateService: TranslateService, private toastr: ToastrService, private errorHandler: ErrorHandlingService) { }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
    this.hub.removeListener();
    this.hub.resetObjects();
  }

  ngOnInit(): void {
    this.isAuthorized$ = this.store.select(selectIsAuthorized);
    this.sub.sink = this.store.select(selectCompanyPreview).subscribe((c) => {
      this.company = c;
      if(this.company) {
        this.loadVotes();
        this.loadTokens();
      }
    });
    this.hub.startConnection();
    this.hub.addVoteListeners();
    this.sub.sink = this.hub.voteSubmitted$.subscribe((v) => {
      if(v) {
        this.translateService.get('COMPANY_VOTING.MESSAGES.VOTE_SUCCESS').subscribe((t) => {
          this.toastr.success(t);
        });
        this.isSubmitting$.next(false);
        this.loadVotes();
      }
    });
    this.sub.sink = this.hub.voteFailed$.subscribe((v) => {
      if(v) {
        this.toastr.error(v);
        this.isSubmitting$.next(false);
      }
    });
  }

  loadTokens(): void {
    this.sub.sink = this.tokenService.getCompanyTokens(this.company.id).subscribe((res) => {
      this.tokens = res.data;
    });
  }

  loadVotes(): void {
    this.isLoading$.next(true);
    this.sub.sink = this.companyService.getProposals(this.company.id).subscribe((response) => {
      this.proposals = response?.data?.entities;
      this.isLoading$.next(false);
    }, (error) => this.isLoading$.next(true));
  }

  onModalClosed(reload: boolean): void {
    if(reload === true) {
      this.loadVotes();
    }
  }

  onCopied(): void {
    this.translateService.get('COMPANY_VOTING.MESSAGES.COPIED').subscribe((t) => {
      this.toastr.success(t);
    });
  }

  vote(result: VoteResult, id: string): void {
    this.isSubmitting$.next(true);
    this.sub.sink = this.companyService.vote(this.company.id, id, { result }).subscribe((res) => {
      this.toastr.success('Transaction is in progress. Please, wait...');
    }, (error) => { this.errorHandler.handleErrorWithToastr(error);});
  }

}
