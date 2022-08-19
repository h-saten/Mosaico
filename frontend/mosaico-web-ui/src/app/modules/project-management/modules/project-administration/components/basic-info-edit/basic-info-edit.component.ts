import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormBase, PatchModel, SuccessResponse, validateForm } from 'mosaico-base';
import { Project, ProjectService } from 'mosaico-project';
import { Token, TokenService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { Observable, of, tap } from 'rxjs';
import { selectPreviewProject, selectProjectPreviewToken, setProjectTitle, setProjectToken } from 'src/app/modules/project-management/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-basic-info-edit',
  templateUrl: './basic-info-edit.component.html',
  styleUrls: ['./basic-info-edit.component.scss']
})
export class BasicInfoEditComponent extends FormBase implements OnInit, OnDestroy {
  subs = new SubSink();
  currentProject: Project;
  projectId: string;
  token: Token;
  availableTokens: Token[] = [];
  companyId: string;

  constructor(private store: Store, private projectService: ProjectService,
    private translateService: TranslateService, private toastr: ToastrService,
    private tokenService: TokenService,
    private errorHandler: ErrorHandlingService) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      tokenId: new FormControl(null, [Validators.required]),
      title: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(50)])
    });
    this.subs.sink = this.store.select(selectProjectPreviewToken).subscribe((t) => {
      this.token = t;
      this.form.get('tokenId')?.setValue(this.token?.id);
    });
    this.subs.sink = this.store.select(selectPreviewProject).subscribe((prj) => {
      if (prj) {
        this.currentProject = prj;
        this.projectId = this.currentProject.id;
        this.companyId = this.currentProject.companyId;
        this.loadCompanyTokens();
        this.form.get('title')?.setValue(this.currentProject.title);
      }
    });
  }

  loadCompanyTokens(): void {
    if (this.companyId) {
      this.subs.sink = this.tokenService.getCompanyTokens(this.companyId).subscribe((res) => {
        this.availableTokens = res.data;
      });
    }
  }

  onTokenCreated(id: string): void {
    this.loadCompanyTokens();
  }

  private setCurrentTokenInStore(id: string): void {
    if (this.availableTokens) {
      const t = this.availableTokens.find((token) => token.id === id);
      if (t) {
        this.store.dispatch(setProjectToken({ token: t }));
      }
    }
  }

  save(): void {
    if (validateForm(this.form)) {
      const title = this.form.get('title')?.value;
      const tokenId = this.form.get('tokenId')?.value;
      const updateTitleExpression: PatchModel = {
        value: title,
        path: '/Title',
        op: 'replace'
      };
      const updateTokenExpression: PatchModel = {
        value: tokenId,
        path: '/TokenId',
        op: 'replace'
      };
      this.subs.sink = this.projectService.patchProject(this.currentProject.id, [updateTitleExpression, updateTokenExpression]).subscribe((updateRes) => {
        this.subs.sink = this.translateService.get('PROJECT_OVERVIEW.MESSAGES.TITLE_UPDATED').subscribe((res) => {
          this.toastr.success(res);
        });
        this.subs.sink = this.translateService.get('PROJECT_OVERVIEW.MESSAGES.TOKEN_UPDATED').subscribe((res) => {
          this.toastr.success(res);
        });
        this.setCurrentTokenInStore(tokenId);
        this.store.dispatch(setProjectTitle({ title }));
      }, (error) => this.errorHandler.handleErrorWithToastr(error));
    }
    else {
      this.translateService.get('PROJECT_OVERVIEW.MESSAGES.INVALID_FORM').subscribe((res) => {
        this.toastr.success(res);
      });
    }
  }

}
