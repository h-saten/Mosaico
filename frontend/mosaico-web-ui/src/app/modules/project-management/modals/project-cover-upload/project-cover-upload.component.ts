import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FileParameters, FormModeEnum } from 'mosaico-base';
import { TokenPageService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { ImageCropperLocalComponent } from '../../../shared-modules/image-cropper-local/image-cropper-local.component';

@Component({
  selector: 'app-project-cover-upload',
  templateUrl: './project-cover-upload.component.html',
  styleUrls: ['./project-cover-upload.component.scss']
})
export class ProjectCoverUploadComponent implements OnInit, OnDestroy {
  @Input() currentImgUrl: string;
  @Input() pageId: string;
  @Input() lang = 'en';

  @ViewChild(ImageCropperLocalComponent) imageCropperLocal: ImageCropperLocalComponent;
  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  currentFormMode: FormModeEnum = FormModeEnum.Add;
  isEditingIsInProgress = false;
  dataSavingRequestInProgress = false;
  addingStarted = false;
  private subs = new SubSink();

  constructor(
    public activeModal: NgbActiveModal,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private pageService: TokenPageService,
    private errorHandling: ErrorHandlingService
  ) { }

  ngOnDestroy(): void {
    let photo: FileParameters;
    photo = {
      data: null,
      fileName: null,
      removeImg: false
    };
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    if (this.currentImgUrl === '' || this.currentImgUrl === undefined || this.currentImgUrl === null) {
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

      if (this.pageId) {
        const fileParameters: FileParameters = this.imageCropperLocal.getFileParameters();

        if (fileParameters.removeImg === true) {
          this.subs.sink = this.pageService.deletePageCover(this.pageId, this.lang).subscribe((res) => {
            this.activeModal.close();
          },  (error) => { this.errorHandling.handleErrorWithToastr(error); this.dataSavingRequestInProgress = false; });

        } else if (fileParameters.data) {
          this.subs.sink = this.pageService.uploadPageCover(this.pageId, fileParameters.data, this.lang)
            .subscribe((res2) => {
              if(res2 && res2.data){
                this.subs.sink = this.translateService.get('MODALS.COVER_UPLOAD.MESSAGES.SUCCESS').subscribe((t) => this.toastr.success(t));
                // not always the function that reads the user's data reads them up-to-date - sometimes it does it too early and reads it before updating
                this.activeModal.close(fileParameters);
            }
          },  (error) => { this.errorHandling.handleErrorWithToastr(error); this.dataSavingRequestInProgress = false; });

        } else {
          this.translateService.get('MODALS.COVER_UPLOAD.MESSAGES.INVALID_FORM')
          .subscribe((t) => {
            this.toastr.error(t);
          });
        }
      }
    }
  }

  closeModal(): void {
    if (this.isEditingIsInProgress === true) {
      // we close the virtual second modal
      this.imageCropperLocal.cancelCrop();
    } else {
      this.activeModal.dismiss(false);
    }
  }
}
