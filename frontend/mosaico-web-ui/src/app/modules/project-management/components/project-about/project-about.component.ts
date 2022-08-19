import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../constants';
import { selectPreviewProjectPermissions, selectProjectPreview } from '../../store';
import { TokenPageService, UpsertAboutPageCommand } from 'mosaico-project';
import { HttpErrorResponse } from '@angular/common/http';
import { ConfigService, ErrorHandlingService, TranslationService } from 'mosaico-base';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LanguageFlag, languages } from 'src/app/modules/shared/models';
import * as DecoupledEditor from '@ckeditor/ckeditor5-build-decoupled-document';
import { CKEditorComponent } from '@ckeditor/ckeditor5-angular';
import { take } from 'rxjs/operators';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { DEFAULT_CONTENT } from './default-content';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-project-about',
  templateUrl: './project-about.component.html',
  styleUrls: ['./project-about.component.scss']
})
export class ProjectCompanyDetailsComponent implements OnInit, OnDestroy {
  public canEdit: boolean;
  public subs: SubSink = new SubSink();
  public editEnabled = false;
  form: FormGroup;

  langs = languages;
  language: LanguageFlag;
  projectId:string;

  public htmlContent: SafeHtml;
  public isLoading = false;
  public Editor = DecoupledEditor;
  public pageId: string;
  public editableContent: string;

  editorConfig: any;

  @ViewChild('editor') editorComponent: CKEditorComponent;
  @ViewChild('editorWrapper') set editorWrapper(input) {
    this.activateEditor();
  }

  public currentAboutId: string;
  public currentPageId: string;

  constructor(private store: Store,
    private errorHandler: ErrorHandlingService,
    private tokenPageService: TokenPageService,
    private toastr: ToastrService,
    configService: ConfigService,
    private translationService: TranslationService,
    private translateService: TranslateService,
    private sanitizer: DomSanitizer
  ) {
    const licenseKey = configService.getConfig()?.ckEditorLicenseKey;
    this.editorConfig = {
      fontSize: { options: [9, 11, 12, 13, 'default', 17, 19, 21] },
      mediaEmbed: { previewsInData: true },
      licenseKey
    };
  }


  ngOnDestroy(): void {
    this.subs.unsubscribe();
    if(this.Editor && this.Editor.destroy) {
      this.Editor.destroy();
    }
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
    this.setLanguage(this.translationService.getSelectedLanguage());
    this.subs.sink = this.store.select(selectProjectPreview).pipe(take(1)).subscribe(data => {
      const pageId = data?.project?.pageId;
      this.projectId = data?.project.id;
      this.currentPageId = pageId;
      if (pageId && pageId.length > 0) {
        this.loadAboutPage();
      }
    });
    this.subs.sink = this.translateService.onLangChange.subscribe((l) => {
      this.selectLanguage(l.lang);
    });
  }

  loadAboutPage(): void {
    this.tokenPageService.getAboutPage(this.currentPageId, this.language.lang).subscribe((res) => {
      if (res?.data) {
        this.editableContent = res.data.content && res.data.content?.length > 0 ? res.data.content : DEFAULT_CONTENT;
        this.htmlContent =  this.sanitizer.bypassSecurityTrustHtml(this.editableContent);
        this.currentAboutId = res.data.id;
        if (this.canEdit && this.editEnabled) {
          this.Editor.setData(this.editableContent);
        }
      }
    }, (error: HttpErrorResponse) => {
      this.errorHandler.handleErrorWithRedirect(error, '/');
    });
  }

  selectLanguage(lang: string): void {
    this.setLanguage(lang);
    this.loadAboutPage();
  }

  setLanguage(lang: string): void {
    this.langs.forEach((language: LanguageFlag) => {
      if (language.lang === lang) {
        language.active = true;
        this.language = language;
      } else {
        language.active = false;
      }
    });
  }

  enableEditing(): void {
    if (this.canEdit) {
      this.editEnabled = true;
    }
  }

  activateEditor(): void {
    const editorEditable = document.querySelector('.document-editor__editable');
    if (editorEditable) {
      DecoupledEditor.create(editorEditable, this.editorConfig).then((editor: any) => {
        const toolbarContainer = document.querySelector('.document-editor__toolbar');
        toolbarContainer?.appendChild(editor.ui.view.toolbar.element);
        this.Editor = editor;
        if (this.canEdit && this.editEnabled) {
          this.Editor.setData(this.editableContent);
        }
      }).catch((err: any) => {
        console.error(err);
      });
    }
  }

  disableEditing(): void {
    this.editEnabled = false;
    if(this.Editor && this.Editor.destroy) {
      this.Editor.destroy();
    }
  }

  save(): void {
    const command: UpsertAboutPageCommand = {
      language: this.language.lang,
      content: this.Editor.getData(),
      pageId: this.currentPageId
    } as UpsertAboutPageCommand;
    if (this.currentPageId && this.currentPageId.length > 0) {
      this.subs.sink = this.tokenPageService.upsertAboutPage(this.currentPageId, command).subscribe((result) => {
        if (result && result.data) {
          this.translateService.get('PROJECT_OVERVIEW.MESSAGES.CONTENT_UPDATED').subscribe((t) => {
            this.toastr.success(t);
          });
          this.editableContent = this.Editor.getData();
          this.htmlContent = this.sanitizer.bypassSecurityTrustHtml(this.editableContent);
          this.disableEditing();
        }
      }, (error) => {
        if(error.error.code === 'NotEmptyValidator') {
          this.toastr.error(this.translateService.instant('PROJECT_OVERVIEW.MESSAGES.EMPTY_DATA'));
        } else {
          this.errorHandler.handleErrorWithToastr(error);
        }
      },
      );
    }
  }
}

