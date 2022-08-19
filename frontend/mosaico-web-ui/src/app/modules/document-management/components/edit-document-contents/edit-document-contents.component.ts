import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { AddDocumentContentCommand } from '../../commands';
import { DocumentContent } from '../../models';
import { DocumentService } from '../../services';
import { LANGUAGES } from '../../constants';
import { ErrorHandlingService, validateForm } from 'mosaico-base';

@Component({
  selector: 'app-edit-document-contents',
  templateUrl: './edit-document-contents.component.html',
  styleUrls: ['./edit-document-contents.component.scss']
})
export class EditDocumentContentsComponent implements OnInit {
  documentLanguages = LANGUAGES;
  documentLanguageKeys = Object.keys(this.documentLanguages);

  mainForm: FormGroup;
  sub: SubSink = new SubSink();
  contentsControl: FormArray = new FormArray([]);

  @Input("documentId")
  documentId: string;
  @Input("documentTitle")
  documentTitle: string;

  documentContents: DocumentContent[];
  @Input('documentContents') set DocumentContents(contents: DocumentContent[]) {
    this.documentContents = contents;
    if (contents && contents.length > 0) {
      this.contentsControl.clear();
      contents.forEach((content) => {
        const form = this.getContentForm();
        form.setValue({
          id: content.id,
          language: content.language,
          documentAddress: content.documentAddress,
          fileId: ''
        });
        this.contentsControl.push(form);
      });
    }
  }

  @Output("contentsModified")
  contentsModified = new EventEmitter<string>();

  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private errorHandler: ErrorHandlingService,
    private documentService: DocumentService
  ) {
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
  }

  private getContentForm(): FormGroup {
    return this.formBuilder.group({
      id: [''],
      language: ['', [Validators.required]],
      fileId: ['', [Validators.required]],
      documentAddress: ['']
    });
  }


  private createForm(): void {
    this.mainForm = this.formBuilder.group({
      contents: this.contentsControl
    });
    this.contentsControl.push(this.getContentForm());
  }

  saveContent(content: FormGroup, index: number): void {
    if (validateForm(content)) {
      const command: AddDocumentContentCommand = {
        language: content.value.language,
        fileId: content.value.fileId
      } as AddDocumentContentCommand;
      if (this.documentId && this.documentId.length > 0) {
        this.mainForm.disable();
        this.sub.sink = this.documentService.addDocumentContents(this.documentId, command).subscribe((result) => {
          if (result && result.data) {
            this.toastr.success('Content was added successfully');
            this.addContent();
            this.contentsModified.emit(result.data);
          }
        }, (error) => {
          this.errorHandler.handleErrorWithToastr(error);
          setTimeout(() => this.mainForm.enable(), 1500);
        },
          () => setTimeout(() => this.mainForm.enable(), 1500)
        );
      }
    }
    else {
      this.toastr.error('Form has invalid values');
    }
  }

  deleteContent(f: FormGroup, index: number): void {
    const contentId = f.value.id;
    if (this.documentId && this.documentId.length > 0 && contentId && contentId.length > 0) {
      this.mainForm.disable();
      this.sub.sink = this.documentService.removeDocumentContents(this.documentId, f.value.language).subscribe((result) => {
        if (result) {
          this.toastr.success('Content was removed successfully');
          this.contentsControl.removeAt(index);
          this.contentsModified.emit(contentId);
        }
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        setTimeout(() => this.mainForm.enable(), 1500);
      },
        () => setTimeout(() => this.mainForm.enable(), 1500));
    }
    else {
      this.contentsControl.removeAt(index);
    }
  }


  addContent(): void {
    const contentForm = this.getContentForm();
    this.contentsControl.push(contentForm);
  }


  getContentsControls(f: FormGroup): FormGroup[] {
    const contentsControl = f.controls.contents as FormArray;
    return contentsControl.controls.map((a) => a as FormGroup);
  }

  saveFile(event: any, f: FormGroup): void {
    if (event && event.target && event.target.files && event.target.files.length > 0) {
      const target: HTMLInputElement = event.target;
      this.mainForm.disable();
      this.sub.sink = this.documentService.storeFile(target.files!).subscribe((result) => {
        if (result && result.data) {
          f.patchValue({ 'fileId': result.data });
        }
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        setTimeout(() => this.mainForm.enable(), 1500);
      },
        () => setTimeout(() => this.mainForm.enable(), 1500));
    }
  }
}

