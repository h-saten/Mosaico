import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationStart, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, of, zip } from 'rxjs';
import { SubSink } from 'subsink';
import * as DecoupledEditor from '@ckeditor/ckeditor5-build-decoupled-document';
import { ToastrService } from 'ngx-toastr';
import { CKEditorComponent } from '@ckeditor/ckeditor5-angular';
import { DocumentService } from 'src/app/modules/document-management/services';
import { EditProjectDocumentContentsCommand, GetExportedPdfCommand, GetTemplateContentsCommand, ModifyProjectDocumentCommand, ProjectDocumentContent, ProjectDocumentTemplate, ProjectService } from 'mosaico-project';
import { ConfigService, DEFAULT_MODAL_OPTIONS, EditorMode, ErrorHandlingService, trim } from 'mosaico-base';
import { selectProjectDocumentTemplate, selectProjectPreview } from '../../store';
import { clearProjectTemplate } from '../../store/project.actions';
import { TranslateService } from '@ngx-translate/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-document-template',
  templateUrl: './document-template.component.html',
  styleUrls: ['./document-template.component.scss']
})
export class DocumentTemplateComponent implements OnInit, OnDestroy, AfterViewInit {

  sub: SubSink = new SubSink();
  templateContents: ProjectDocumentTemplate;
  currentDocument: ProjectDocumentContent;
  localContent: string;
  documentExported: boolean = false;
  editorModeEnum: typeof EditorMode = EditorMode;
  currentEditorMode = EditorMode.text;
  editor = DecoupledEditor;
  isTemplateEditing = false;
  isLoading = false;
  projectId: string;
  slug: string;

  toolbar = {
    items: [
      'previousPage', 'nextPage', 'pageNavigation', '|',
      '-',
      'heading', 'bold', 'italic', 'fontSize', 'fontFamily', '|',
      'underline', 'strikethrough', 'subscript', 'superscript', '|',
      'alignment', 'numberedList', 'bulletedList', 'indent', 'outdent', '|',
      'undo', 'redo', 'fontBackgroundColor', 'fontColor', '|',
      'uploadImage', 'insertTable', 'blockQuote', 'code', 'specialCharacters']
  };

  heading = {
    options: [
      { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
      { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
      { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' },
      { model: 'heading3', view: 'h3', title: 'Heading 3', class: 'ck-heading_heading3' },
      { model: 'heading4', view: 'h4', title: 'Heading 4', class: 'ck-heading_heading4' },
      { model: 'heading5', view: 'h5', title: 'Heading 5', class: 'ck-heading_heading5' },
      { model: 'heading6', view: 'h6', title: 'Heading 6', class: 'ck-heading_heading6' }
    ]
  };
  pagination = {
    // Page width and height correspond to A4 format
    pageWidth: '21cm',
    pageHeight: '29.7cm',

    pageMargins: {
      top: '20mm',
      bottom: '20mm',
      right: '12mm',
      left: '12mm'
    }
  };
  licenseKey: string;

  constructor(
    private projectService: ProjectService,
    private router: Router,
    private errorHandler: ErrorHandlingService,
    private translateService: TranslateService,
    private store: Store,
    private toastr: ToastrService,
    private documentService: DocumentService,
    configService: ConfigService
  ) {
    this.licenseKey = configService.getConfig()?.ckEditorLicenseKey;
  }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectProjectPreview).subscribe((res) => {
      if (res && res.project) {
        this.projectId = res.project.id;
        this.slug = res.project.slug;
        this.sub.sink = this.store.select(selectProjectDocumentTemplate).subscribe((template) => {
          if (template && template.language && template.documentId && template.templateKey) {
            this.isTemplateEditing = false;
            this.loadDocumentContent(this.projectId, template.templateKey, template.language);
          }
          else if (template && template.language && template.templateKey) {
            this.isTemplateEditing = true;
            this.loadTemplate(template.templateKey, template.language);
          }
          else {
            this.router.navigateByUrl(`/project/${res.project.slug}`);
          }
        });
      }
      else {
        this.router.navigateByUrl('/');
      }
    });
  }

  ngAfterViewInit(): void {
    DecoupledEditor.create(document.querySelector('.document-editor__editable'), {
      fontSize: { options: [9, 11, 12, 13, 'default', 17, 19, 21] },
      toolbar: this.toolbar,
      heading: this.heading,
      pagination: this.pagination,
      licenseKey: this.licenseKey
    }).then((editor: any) => {
      const toolbarContainer = document.querySelector('.document-editor__toolbar');
      toolbarContainer?.appendChild(editor.ui.view.toolbar.element);
      this.editor = editor;
    }).catch((err: any) => {
      console.error(err);
    });
  }


  public loadDocumentContent(id: string, type: string, lang: string): void {
    if (!this.isLoading) {
      this.isLoading = true;
      this.sub.sink = this.projectService.getProjectDocumentContent(id, type, lang).subscribe((res) => {
        if (res && res.data) {
          this.currentDocument = res.data;
          this.localContent = res.data.content;
          this.editor.setData(this.localContent);
        }
        this.isLoading = false;
      }, (error: HttpErrorResponse) => {
        this.isLoading = false;
      });
    }
  }

  public loadTemplate(templateKey: string, lang: string): void {
    if (!this.isLoading) {
      this.isLoading = true;
      const command: GetTemplateContentsCommand = {
        key: templateKey,
        language: lang
      };
      this.sub.sink = this.projectService.getTemplateContent(command).subscribe((res) => {
        if (res && res.data) {
          this.templateContents = res.data.template;
          this.localContent = res.data.template.content;
          this.editor.setData(this.localContent);
        }
        this.isLoading = false;
      }, (error: HttpErrorResponse) => {
        this.isLoading = false;
      });
    }
  }

  public onReady(editor: any): void {
    editor.ui.getEditableElement().parentElement.insertBefore(
      editor.ui.view.toolbar.element,
      editor.ui.getEditableElement()
    );
  }

  public reset(): void {
    if (this.isTemplateEditing) {
      this.localContent = this.templateContents.content;
      this.editor.setData(this.localContent);
    }
    else {
      this.localContent = this.currentDocument.content;
      this.editor.setData(this.localContent);
    }
  }


  contentModified(): void {

  }

  // exportPdf(): void {
  //   const content = this.editor.getData();
  //   const commandGet: GetExportedPdfCommand = {
  //     html: content,
  //     css: "",
  //     options: { margin_top: "2cm" }
  //   };
  //   this.sub.sink = this.projectService.getExportedPdf(commandGet).subscribe((data) => {
  //     if (data) {
  //       var title = this.templateContents.key + "_" + this.templateContents.language;
  //       var file = new File([data], title + ".pdf", { type: 'application/pdf', lastModified: Date.now() });
  //       const url = window.URL.createObjectURL(file);
  //       window.open(url);
  //       var dt = new DataTransfer();
  //       dt.items.add(file);
  //       var files = dt.files;
  //       const command: ModifyProjectDocumentCommand = {
  //         title
  //       };
  //       this.sub.sink = this.documentService.storeFile(files).subscribe((result) => {
  //         if (result && result.data) {
  //           this.toastr.success('Document was created and exported as PDF successfully');
  //           this.documentExported = true;
  //         }
  //       }, (error) => {
  //         this.errorHandler.handleErrorWithToastr(error);
  //       });
  //     }
  //   });
  // }

  save(): void {
    if (this.projectId && this.editor) {
      if (!this.localContent || this.localContent.length === 0) {
        this.sub.sink = this.translateService.get('EDITOR.MESSAGES.INVALID_VALUE').subscribe((t) => {
          this.toastr.error(t);
        });
      }
      this.editor.isReadOnly = this.isLoading = true;
      if (this.isTemplateEditing) {
        let command: EditProjectDocumentContentsCommand = {
          content: this.editor.getData(),
          language: this.templateContents.language,
          type: this.templateContents.key
        };
        command = trim(command);
        this.sub.sink = this.projectService.editProjectDocumentContents(this.projectId, command).subscribe((result) => {
          if (result) {
            this.sub.sink = this.translateService.get('EDITOR.MESSAGES.SUCCESS').subscribe((t) => {
              this.toastr.success(t);
            });
          }
          this.editor.isReadOnly = this.isLoading = false;
        }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.editor.isReadOnly = this.isLoading = false; });
      }
      else {
        const command: EditProjectDocumentContentsCommand = {
          content: this.editor.getData(),
          language: this.currentDocument.language,
          type: this.currentDocument.type?.key
        };
        this.sub.sink = this.projectService.editProjectDocumentContents(this.projectId, command).subscribe((result) => {
          if (result) {
            this.sub.sink = this.translateService.get('EDITOR.MESSAGES.SUCCESS').subscribe((t) => {
              this.toastr.success(t);
            });
          }
          this.editor.isReadOnly = this.isLoading = false;
        }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.editor.isReadOnly = this.isLoading = false; });
      }
    }
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
    this.store.dispatch(clearProjectTemplate());
  }

  cancel(): void {
    if (this.slug) {
      this.router.navigateByUrl(`/project/${this.slug}`);
    }
  }
}

