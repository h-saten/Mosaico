import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../../../constants';
import { selectPreviewProjectPermissions, selectProjectPage, setPageShortDescription } from '../../../../store';
import { Page, TokenPageService } from 'mosaico-project';
import { FormBase, PatchModel, trim, validateForm } from 'mosaico-base';
import { selectUserInformation } from '../../../../../user-management/store/user.selectors';

@Component({
  selector: 'app-project-short-description',
  templateUrl: './project-short-description.component.html',
  styleUrls: ['./project-short-description.component.scss']
})
export class ProjectShortDescriptionComponent extends FormBase implements OnInit, OnDestroy {
  public page: Page;
  public subs: SubSink = new SubSink();
  public isLoading = false;
  public currentLang = "en";

  constructor(private store: Store, private pageService: TokenPageService,
    private translateService: TranslateService, private toastrService: ToastrService) {  super(); }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      shortDescription: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(350)])
    });
    this.subs.sink = this.store.select(selectProjectPage).subscribe((res) => {
      this.page = res;
      this.form.get('shortDescription')?.setValue(this.page?.shortDescription);
    });
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe((l) => {
      this.currentLang = l.lang;
    });
  }

  setCurrentLang(newLang: string): void {
    this.currentLang = newLang;
    this.subs.sink = this.pageService.getPage(this.page.id, this.currentLang).subscribe((res) => {
      this.page = res.data;
      this.form.get('shortDescription')?.setValue(this.page?.shortDescription);
    });
  }

  save(): void {
    if (validateForm(this.form) && this.page) {
      let command = this.form.getRawValue();
      this.isLoading = true;
      this.form.disable();
      command = trim(command);
      const updateExpression: PatchModel = {
        value: command.shortDescription,
        path: '/ShortDescription/' + this.currentLang,
        op: 'add'
      };
      this.subs.sink = this.pageService.patchPage(this.page.id, [updateExpression]).subscribe((patchRes) => {
        this.subs.sink = this.translateService.get('PROJECT_OVERVIEW.MESSAGES.SHORT_DESC_UPDATED').subscribe((res) => {
          this.toastrService.success(res);
        });
        this.isLoading = false;
        this.form.enable();
      }, (error) => {this.isLoading = false; this.form.enable();} );
    }
    else{
      this.subs.sink = this.translateService.get('PROJECT_OVERVIEW.MESSAGES.INVALID_FORM').subscribe((res) => {
        this.toastrService.error(res);
      });
    }
  }

}
