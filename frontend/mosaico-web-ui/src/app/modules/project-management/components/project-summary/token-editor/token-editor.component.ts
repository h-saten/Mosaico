import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, PatchModel, SuccessResponse } from 'mosaico-base';
import { ProjectService } from 'mosaico-project';
import { Token, TokenService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { selectProjectPreviewToken, selectProjectPreview, setProjectToken } from '../../../store';
import { take, tap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-token-editor',
  templateUrl: './token-editor.component.html',
  styleUrls: ['./token-editor.component.scss']
})
export class TokenEditorComponent implements OnInit, OnDestroy {
  token: Token;
  projectId: string;
  availableTokens: Token[] = [];
  companyId: string;
  subs: SubSink = new SubSink();

  constructor(
    private store: Store,
    private tokenService: TokenService,
    private projectService: ProjectService,
    private translateService: TranslateService,
    private toastrService: ToastrService,
    private ErrorHandling: ErrorHandlingService
  ) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPreviewToken).subscribe((t) => {
      this.token = t;
    });
    this.subs.sink = this.store.select(selectProjectPreview).pipe(take(1)).subscribe((p) => {
      if(p && p.project){
        this.projectId = p.project.id;
        this.companyId = p.project.companyId;
        this.subs.sink = this.loadCompanyTokens().subscribe();
      }
    });
  }

  selectToken(id: string): void {
    if(id && id.length > 0) {
      const updateExpression: PatchModel = {
        value: id,
        path: '/TokenId',
        op: 'replace'
      };
      this.subs.sink = this.projectService.patchProject(this.projectId, [updateExpression]).subscribe((patchRes) => {
        this.subs.sink = this.translateService.get('PROJECT_OVERVIEW.MESSAGES.TOKEN_UPDATED').subscribe((res) => {
          this.toastrService.success(res);
        });
        this.setCurrentTokenInStore(id);
      }, (error) => {this.ErrorHandling.handleErrorWithToastr(error);});
    }
  }

  loadCompanyTokens(): Observable<SuccessResponse<Token[]>> {
    if(this.companyId){
      return this.tokenService.getCompanyTokens(this.companyId).pipe(tap((res) => {
        this.availableTokens = res.data;
      }));
    }
    return of();
  }

  onTokenCreated(id: string): void {
    this.subs.sink = this.loadCompanyTokens().subscribe((tokens) => {
      this.setCurrentTokenInStore(id);
    });
  }

  private setCurrentTokenInStore(id: string): void {
    if(this.availableTokens){
      const t = this.availableTokens.find((token) => token.id === id);
      if(t){
        this.store.dispatch(setProjectToken({token: t}));
      }
    }
  }

}
