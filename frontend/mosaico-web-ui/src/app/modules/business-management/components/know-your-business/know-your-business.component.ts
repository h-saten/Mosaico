import { Component, OnInit, Input, OnChanges, SimpleChange, SimpleChanges } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SubSink } from 'subsink';
import { DocumentService } from 'src/app/modules/document-management/services';
import { Router } from '@angular/router';
import { ErrorHandlingService, trim, TranslationService, validateForm } from 'mosaico-base';
import { AddVerificationCommand, CompanyService, Verification } from 'mosaico-dao';

@Component({
  selector: 'app-kyb',
  templateUrl: './know-your-business.component.html',
  styleUrls: ['./know-your-business.component.scss']
})
export class KnowYourBusinessComponent implements OnInit, OnChanges {
  @Input() companyId: string;
  @Input() canEdit: boolean;
  verification: Verification;

  readOnly: boolean = false;
  savingFile: string;
  sub: SubSink = new SubSink();
  contentsControl: FormArray = new FormArray([]);
  mainForm: FormGroup;

  constructor(private modalService: NgbModal,
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private documentService: DocumentService,
    private errorHandler: ErrorHandlingService,
    private router: Router,
    private companyService: CompanyService,
    private translationService: TranslationService
    ) {
  }

  ngOnInit(): void {
    if (this.companyId) {
      this.getVerification();
    }
    this.createForm();
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.companyId) {
      this.ngOnInit();
    }
  }

  private updateFormValue(verification: Verification): void {
    if (verification.shareholders.length > 0) {
      for (var i = 0; i < verification.shareholders.length; i++) {
        this.addContent();
        var shareholders = verification.shareholders[i];
        this.contentsControl.at(i).get("fullName")?.setValue(shareholders.fullName);
        this.contentsControl.at(i).get("share")?.setValue(shareholders.share);
        this.contentsControl.at(i).get("email")?.setValue(shareholders.email);
      }
    }
    this.mainForm.patchValue({ "companyRegistration": verification.companyRegistrationUrl});
    this.mainForm.patchValue({ "companyAddress": verification.companyAddressUrl});

  }

  getVerification() {
    this.sub.sink = this.companyService.getVerification(this.companyId).subscribe((result) => {
      if (result && result.data) {
        if (!this.verification) {
          this.verification = result.data.verification;
          this.updateFormValue(result.data.verification);
          this.mainForm.disable();
          this.readOnly = true;
        }
      }
    }, (error) => {
      setTimeout(() => this.mainForm.enable(), 1500);
    },
    );
  }

  private getContentForm(): FormGroup {
    return this.formBuilder.group({
      fullName: ['', [Validators.required]],
      share: ['', [Validators.required, Validators.min(25)]],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  private createForm(): void {
    this.mainForm = this.formBuilder.group({
      companyRegistration: ['', [Validators.required]],
      companyAddress: ['', [Validators.required]],
      contents: this.contentsControl
    });
  }

  save(): void {
    if (validateForm(this.mainForm) && this.contentsControl.value?.length > 0) {
      let command: AddVerificationCommand = {
        companyRegistrationUrl: this.mainForm.value.companyRegistration,
        companyAddressUrl: this.mainForm.value.companyAddress,
        shareholders: this.contentsControl.value,
        companyId: this.companyId,
        companyName: null
      };
      this.mainForm.disable();
      command = trim(command);
      this.sub.sink = this.companyService.addVerification(this.companyId, command).subscribe((result) => {
        if (result && result.ok) {
          this.toastr.success('Verification was added successfully');
          this.readOnly = true;
          this.mainForm.disable();
          this.router.navigateByUrl(`/dao/${this.companyId}/overview/`);
        }
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        setTimeout(() => this.mainForm.enable(), 1500);
      },
        () => setTimeout(() => this.mainForm.enable(), 1500)
      );
    }
    else {
      this.toastr.error('Form has invalid values');
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

  saveCompanyRegistration(event: any): void {
    this.savingFile = "companyRegistration";
    this.saveFile(event);
  }

  saveCompanyAddress(event: any): void {
    this.savingFile = "companyAddress";
    this.saveFile(event);
  }

  deleteRow(i: number): void {
    this.contentsControl.removeAt(i);
  }

  saveFile(event: any): void {
    if (event && event.target && event.target.files && event.target.files.length > 0) {
      const target: HTMLInputElement = event.target;
      this.mainForm.disable();
      this.sub.sink = this.companyService.storeFile(target.files!, this.companyId).subscribe((result) => {
        if (result && result.data) {
          if (this.savingFile == "companyRegistration") {
            this.mainForm.patchValue({ "companyRegistration": result.data });
          } else {
            this.mainForm.patchValue({ "companyAddress": result.data });
          }
        }
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        setTimeout(() => this.mainForm.enable(), 1500);
      },
        () => setTimeout(() => this.mainForm.enable(), 1500));
    }
  }
}
