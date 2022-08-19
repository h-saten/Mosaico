import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PROJECT_ROLES } from '../../../constants';
import { ToastrService } from 'ngx-toastr';
import { FormDialogBase } from 'mosaico-base';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LANGUAGES } from 'src/app/modules/document-management/constants';
import { SubSink } from 'subsink';
import { ProjectDocumentType, ProjectService } from 'mosaico-project';
import { validateForm } from 'mosaico-base';

@Component({
  selector: 'app-select-document-template',
  templateUrl: './select-document-template.component.html',
  styleUrls: ['./select-document-template.component.scss']
})
export class SelectDocumentTemplateComponent extends FormDialogBase implements OnInit {
  documentLanguages = LANGUAGES;
  documentLanguageKeys = Object.keys(this.documentLanguages);
  documentTypes: ProjectDocumentType[] = [];
  private sub: SubSink = new SubSink();
  constructor(private projectService: ProjectService, private toastr: ToastrService, modal: NgbModal) { super(modal); }

  ngOnInit(): void {
    this.createForm();
    this.getDocumentTypes();
  }

  createForm(): void {
    this.form = new FormGroup({
      type: new FormControl('', [Validators.required]),
      language: new FormControl('', [Validators.required])
    });
  }

  getDocumentTypes(): void {
    this.sub.sink = this.projectService.getProjectDocumentTypes().subscribe((res) => {
      if (res && res.data && res.data.entities) {
        this.documentTypes = res.data.entities;
      }
    });
  }

  submit(): void {
    if (validateForm(this.form)) {
      const command = this.form.value;
      if (this.modalRef) {
        this.modalRef.close(command);
      }
    }
    else {
      this.toastr.error('Form has invalid values. Please fix issue to continue');
    }
  }

}
