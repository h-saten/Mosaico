import { Component, Input, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FileParameters, FormModeEnum } from 'mosaico-base';
import { ProjectService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { ImageCropperLocalComponent } from '../../../shared-modules/image-cropper-local/image-cropper-local.component';

@Component({
  selector: 'app-edit-project-logo',
  templateUrl: './edit-project-logo.component.html',
  styleUrls: ['./edit-project-logo.component.scss']
})
export class EditProjectLogoComponent implements OnInit, OnDestroy {
  @Input() currentImgUrl: string;
  @Input() projectId: string;

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
    private projectService: ProjectService,
    private errorHandling: ErrorHandlingService
  ) { }

  ngOnDestroy(): void {
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

      if (this.projectId) {
        const fileParameters: FileParameters = this.imageCropperLocal?.getFileParameters();

        if (fileParameters) {
          this.subs.sink = this.projectService.updateProjectLogo(fileParameters.data, this.projectId)
            .subscribe((res2) => {
              if(res2 && res2.data){
                this.subs.sink = this.translateService.get('MODALS.PROJECT_LOGO_EDITOR.MESSAGES.SUCCESS').subscribe((t) => this.toastr.success(t));
                this.activeModal.close(fileParameters);
              }
          }, (error) => {this.errorHandling.handleErrorWithToastr(error); this.dataSavingRequestInProgress = false;});

        } else {
          this.translateService.get('MODALS.PROJECT_LOGO_EDITOR.MESSAGES.INVALID_FORM')
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
