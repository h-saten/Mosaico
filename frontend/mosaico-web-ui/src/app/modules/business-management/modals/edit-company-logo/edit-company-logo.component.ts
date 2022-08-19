import { Component, Input, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FileParameters, FormModeEnum } from 'mosaico-base';
import { CompanyService } from 'mosaico-dao';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { ImageCropperLocalComponent } from '../../../shared-modules/image-cropper-local/image-cropper-local.component';

@Component({
  selector: 'app-edit-company-logo',
  templateUrl: './edit-company-logo.component.html',
  styleUrls: ['./edit-company-logo.component.scss']
})
export class EditCompanyLogoComponent implements OnInit, OnDestroy {
  @Input() currentImg: string;
  @Input() companyId: string;

  @ViewChild(ImageCropperLocalComponent) imageCropperLocal: ImageCropperLocalComponent;
  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  currentFormMode: FormModeEnum = FormModeEnum.Add;
  isEditingIsInProgress = false;
  dataSavingRequestInProgress = false;
  addingStarted = false;
  subs = new SubSink();

  constructor(
    public modalService: NgbActiveModal,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private companyService: CompanyService,
    private errorHandler: ErrorHandlingService
  ) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    if (this.currentImg === '' || this.currentImg === undefined || this.currentImg === null) {
      this.currentFormMode = FormModeEnum.Add;
    } else {
      this.currentFormMode = FormModeEnum.Edit;
    }
  }

  editingIsInProgress(event: boolean): void {
    this.addingStarted = true;
    this.isEditingIsInProgress = event;
  }

  editingCancel(): void {
    this.addingStarted = false;
  }

  savePhoto(): void {
    if (this.addingStarted === true) {
      this.dataSavingRequestInProgress = true;
      const logo: FileParameters = this.imageCropperLocal?.getFileParameters();
      if (logo) {
        this.subs.sink = this.companyService.uploadCompanyLogo(this.companyId, logo.data)
          .subscribe((res) => {
            this.subs.sink = this.translateService.get('MODALS.COMPANY_LOGO_EDITOR.MESSAGES.SUCCESS').subscribe((t) => this.toastr.success(t));
            this.modalService.close(logo);
            // this.isLoading = false;
        }, (error) => {this.errorHandler.handleErrorWithToastr(error); this.dataSavingRequestInProgress = false;});

      } else {
        this.translateService.get('MODALS.COMPANY_LOGO_EDITOR.MESSAGES.INVALID_FORM')
        .subscribe((t) => {
          this.toastr.error(t);
        });
      }
    }
  }

  closeModal(): void {
    if (this.isEditingIsInProgress === true) {
      // we close the virtual second modal
      this.imageCropperLocal.cancelCrop();
    } else {
      this.modalService.dismiss(false);
    }
  }

}
