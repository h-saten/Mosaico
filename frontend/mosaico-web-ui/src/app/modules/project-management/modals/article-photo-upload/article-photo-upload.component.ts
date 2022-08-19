import { Component, Input, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormModeEnum } from 'mosaico-base';
import { ProjectService, TokenPageService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { ImageCropperLocalComponent } from '../../../shared-modules/image-cropper-local/image-cropper-local.component';

@Component({
  selector: 'app-article-photo-upload',
  templateUrl: './article-photo-upload.component.html',
  styleUrls: ['./article-photo-upload.component.scss']
})
export class ArticlePhotoUploadComponent implements OnInit, OnDestroy {
  @Input() currentImgUrl: string;
  @Input() projectId: string;
  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  currentFormMode: FormModeEnum = FormModeEnum.Add;
  @ViewChild(ImageCropperLocalComponent) imageCropperLocal: ImageCropperLocalComponent;
  subs = new SubSink();
  isLoading = false;

  constructor(
    public modalService: NgbActiveModal,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private tokenPageService: TokenPageService,
    private errorHandler: ErrorHandlingService
  ) {
  }

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

  submit(): void {
    const logo = this.imageCropperLocal?.getFileParameters();
    if (logo) {
      this.isLoading = true;
      this.subs.sink = this.tokenPageService.updateArticlePhoto(logo.data, this.projectId).subscribe((res) => {
        this.subs.sink = this.translateService.get('MODALS.PROJECT_ARTICLE_PHOTO_UPLOAD.MESSAGES.SUCCESS').subscribe((t) => this.toastr.success(t));
        this.modalService.close(res.data);
        this.isLoading = false;
      }, (error) => {this.errorHandler.handleErrorWithToastr(error); this.isLoading = false;});
    }
    else {
      this.translateService.get('MODALS.PROJECT_ARTICLE_PHOTO_UPLOAD.MESSAGES.INVALID_FORM').subscribe((t) => {
        this.toastr.error(t);
      });
    }
  }

}
