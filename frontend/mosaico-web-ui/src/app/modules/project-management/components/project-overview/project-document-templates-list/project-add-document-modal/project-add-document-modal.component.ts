import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBase } from 'mosaico-base';

@Component({
  // selector: 'app-project-add-document-modal',
  templateUrl: './project-add-document-modal.component.html',
  styleUrls: ['./project-add-document-modal.component.scss']
})
export class ProjectAddDocumentModalComponent extends FormBase implements OnInit, OnDestroy {
  file: File;
  fileName: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder
  ) {
    super();
  }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(): void {
    this.form = this.formBuilder.group({
      title: ['', Validators.required]
    });
  }

  saveDocument(event: any) {
    event.preventDefault();
    this.file = event.target.files[0];
    this.fileName = this.file.name;
  }

  onSubmit() {
    this.activeModal.close(this.form.controls.title.value);
  }
}
