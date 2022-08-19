import { Component, OnInit, Input, OnDestroy, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../services';
import { ImageCropperLocalComponent } from 'src/app/modules/shared-modules/image-cropper-local/image-cropper-local.component';
import { ErrorHandlingService, FileParameters, FormModeEnum } from 'mosaico-base';
import { SubSink } from 'subsink';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-modal-profile',
  templateUrl: './modal-profile.component.html',
  styleUrls: ['./modal-profile.component.scss']
})
export class ModalProfileComponent implements OnInit, OnDestroy {
  @Input() currentImgUrl: string;
  @Input() userId: string;

  @ViewChild(ImageCropperLocalComponent) imageCropperLocal: ImageCropperLocalComponent;
  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  currentFormMode: FormModeEnum = FormModeEnum.Add;
  isEditingIsInProgress = false;
  dataSavingRequestInProgress = false;
  addingStarted = false;
  subs = new SubSink();

  constructor(
    public activeModal: NgbActiveModal,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private userService: UserService,
    private errorHandler: ErrorHandlingService
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
      const fileParameters: FileParameters = this.imageCropperLocal.getFileParameters();
      if(this.userId){
        if (fileParameters.removeImg === true) {
          this.subs.sink = this.userService.deleteUserPhoto(this.userId)
            .subscribe(() => {
              // this.toastr.success('Photo was successfully removed');
              // not always the function that reads the user's data reads them up-to-date - sometimes it does it too early and reads it before updating
              this.activeModal.close();
          },  (error) => { this.errorHandler.handleErrorWithToastr(error); this.dataSavingRequestInProgress = false; });
        // add img

        } else if (fileParameters.data) {
          this.subs.sink = this.userService.updateUserPhoto(fileParameters.data, this.userId)
            .subscribe((res2) => {
              if(res2 && res2.data){
                // this.toastr.success('Photo was successfully updated');
                // not always the function that reads the user's data reads them up-to-date - sometimes it does it too early and reads it before updating
                this.activeModal.close(fileParameters);
            }
          },  (error) => { this.errorHandler.handleErrorWithToastr(error); this.dataSavingRequestInProgress = false; });
        } else {
          this.translateService.get('MODALS.PROJECT_LOGO_EDITOR.MESSAGES.INVALID_FORM')
          .subscribe((t) => {
            this.toastr.error(t);
          });
        }
      }
    }
  }
}
