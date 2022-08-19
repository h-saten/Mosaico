import { Component, Input, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FileParameters, FormModeEnum } from 'mosaico-base';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { ImageCropperLocalComponent } from '../../../shared-modules/image-cropper-local/image-cropper-local.component';
import { TokenService } from 'mosaico-wallet';

@Component({
  selector: 'app-edit-token-logo',
  templateUrl: './edit-token-logo.component.html',
  styleUrls: ['./edit-token-logo.component.scss']
})
export class EditTokenLogoComponent implements OnInit, OnDestroy {
  @Input() currentImgUrl: string;
  @Input() tokenId: string;

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
    private tokenService: TokenService,
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

      if (this.tokenId) {
        const fileParameters: FileParameters = this.imageCropperLocal?.getFileParameters();

        if (fileParameters) {
          this.subs.sink = this.tokenService.updateTokenLogo(fileParameters.data, this.tokenId)
            .subscribe((res2) => {
              if(res2 && res2.data){
                this.subs.sink = this.translateService.get('MODALS.TOKEN_LOGO_EDITOR.MESSAGES.SUCCESS').subscribe((t) => this.toastr.success(t));
                this.activeModal.close(fileParameters);
              }
          }, (error) => {this.errorHandling.handleErrorWithToastr(error); this.dataSavingRequestInProgress = false;});

        } else {
          this.translateService.get('MODALS.TOKEN_LOGO_EDITOR.MESSAGES.INVALID_FORM')
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
