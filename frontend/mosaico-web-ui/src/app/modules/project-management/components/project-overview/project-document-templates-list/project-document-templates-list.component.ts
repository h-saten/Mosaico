import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { DEFAULT_MODAL_OPTIONS, ErrorHandlingService, TranslationService } from 'mosaico-base';
import {
  ModifyProjectDocumentCommand,
  Project,
  ProjectDocument,
  ProjectDocumentType,
  ProjectService,
  UploadProjectDocumentCommand
} from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SubSink } from 'subsink';
import {
  selectPreviewProject,
  selectPreviewProjectPermissions,
  setProjectEditTemplate,
  selectProjectPreview
} from '../../../store';
import { Router } from '@angular/router';
import { DocumentService } from 'src/app/modules/document-management/services';
import { PERMISSIONS } from '../../../constants';
import { languages } from 'src/app/modules/shared/models';
import { ProjectAddDocumentModalComponent } from './project-add-document-modal/project-add-document-modal.component';

@Component({
  selector: 'app-project-document-templates-list',
  templateUrl: './project-document-templates-list.component.html',
  styleUrls: ['./project-document-templates-list.component.scss']
})
export class ProjectDocumentTemplatesListComponent implements OnInit, OnDestroy {
  subs: SubSink = new SubSink();
  canEdit = false;
  project: Project;
  documents: ProjectDocument[] = [];
  plDocuments: ProjectDocument[] = [];
  files: any[] = [];
  isLoaded = false;
  isLoading = false;
  languages = languages;
  uploadedDocumentUrl: string;
  selectedDocument: ProjectDocument;
  selectedLanguage: string;
  constructor(private router: Router,
    private store: Store, private projectService: ProjectService,
    private toastr: ToastrService, private errorHandler: ErrorHandlingService,
    private documentService: DocumentService,private translationService: TranslationService,
    private translateService: TranslateService, private modalService: NgbModal) {
      this.selectedLanguage = this.translationService.getSelectedLanguage();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((res) => {
      if (res) {
        this.project = res.project;
        this.getProjectDocuments();
      }
    });
  }

  getProjectDocuments(force = false): void {
    if ((!this.isLoaded || force === true) && !this.isLoading) {
      this.isLoading = true;
      this.projectService.getProjectDocuments(this.project.id).subscribe((res) => {
        if (res && res.data) {
          this.documents = res.data.documents;
          if (this.documents && !this.canEdit) {
            this.documents = this.documents.filter((d) => d.id && d.id.length > 0);
          }
        }
        this.isLoading = false;
        this.isLoaded = true;
      }, (error) => { this.isLoaded = true; this.isLoading = false; this.errorHandler.handleErrorWithToastr(error); });
      if(this.canEdit){
        this.projectService.getProjectDocumentsByLanguage(this.project.id, this.selectedLanguage === "pl" ? "en" : "pl").subscribe((res) => {
          if (res && res.data) {
            this.plDocuments = res.data.documents;
            if (this.plDocuments && !this.canEdit) {
              this.plDocuments = this.plDocuments.filter((d) => d.id && d.id.length > 0);
            }
          }
          this.isLoading = false;
          this.isLoaded = true;
        }, (error) => { this.isLoaded = true; this.isLoading = false; this.errorHandler.handleErrorWithToastr(error); });
      }
    }
  }

  saveDocument(event: any, id: any): void {
    if (event && event.target && event.target.files && event.target.files.length > 0) {
      const selectedType = this.documents.find(x => x.id === id);
      const command: UploadProjectDocumentCommand = {
        content: event.target.files,
        language: selectedType.language,
        type: selectedType.type.key
      }
      if (this.project) {
        this.subs.sink = this.projectService.uploadProjectDocument(this.project.id, command).subscribe((result) => {
          if (result && result.data) {
            this.translateService.get('PROJECT_OVERVIEW.FILES.MESSAGES.DOCUMENT_SAVE').subscribe((t) => this.toastr.success(t));
            this.getProjectDocuments(true);
          }
        }, (error) => {
          this.errorHandler.handleErrorWithToastr(error);
        });
      }
    }
  }

  changeLan(val,d: ProjectDocument):void{
    this.selectedDocument = this.plDocuments.find(x => x.language == val.lang && x.type.key == d.type.key);
  }

  openFile(d: ProjectDocument): void {
    if(d && d.url && d.url.length > 0){
      window.open(d.url, '_blank');
    }
  }

  openDocumentTemplate(doc: ProjectDocument): void {
    if (doc && doc.language && doc.language.length > 0 && this.project) {
      this.store.dispatch(setProjectEditTemplate({templateKey: doc.type?.key, language: doc.language, documentId: doc.id}));
        this.router.navigateByUrl(`/project/${this.project.slug}/editor`);
    } else {
      this.toastr.error("Please select language");
    }
  }

  openAddModal() {
    const modalRef = this.modalService.open(ProjectAddDocumentModalComponent, DEFAULT_MODAL_OPTIONS);
    this.subs.sink = modalRef.closed.subscribe((res?: boolean) => {
      if(res){
        setTimeout(() => {
        }, 1000);
      }
    });

    modalRef.closed.subscribe(data => this.files.push(data))
  }
}
