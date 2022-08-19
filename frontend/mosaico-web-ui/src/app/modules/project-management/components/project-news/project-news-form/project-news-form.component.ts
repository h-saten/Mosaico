import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { DEFAULT_MODAL_OPTIONS, ErrorHandlingService, FormBase, FormModeEnum, validateForm } from 'mosaico-base';
import { CreateUpdateProjectArticleCommand, ProjectArticle, TokenPageService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ImageCropperLocalComponent } from 'src/app/modules/shared-modules/image-cropper-local/image-cropper-local.component';
import { SubSink } from 'subsink';
import { ProjectPathEnum } from '../../../constants';
import { clearProjectArticle, selectProjectArticle, selectProjectPreview } from '../../../store';
import { ArticleCoverUploadComponent } from '../../../modals/article-cover-upload/article-cover-upload.component';
import { ArticlePhotoUploadComponent } from '../../../modals/article-photo-upload/article-photo-upload.component';

@Component({
  // selector: 'app-project-news-form',
  templateUrl: './project-news-form.component.html',
  styleUrls: ['./project-news-form.component.scss']
})
export class ProjectNewsFormComponent extends FormBase implements OnInit, OnDestroy {

  private subs: SubSink = new SubSink();

  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  @Input() currentFormMode: FormModeEnum;

  @Input() projectId: string;
  @Input() pageId: string;
  @Input() currentLang: string;
  @Input() article: ProjectArticle;

  @Output() saveEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() cancelEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  cropImage = false;
  coverPictureUrl = '';
  blankImgUrl = 'assets/media/tokenpage/article-cover.png';
  coverId = '';
  photoId = '';
  articleId = '';
  authorPhotoUrl ='';
  hiddenArticle = false;
  projectPathEnum: typeof ProjectPathEnum = ProjectPathEnum;
  textAreaCount = 0;
  date: Date;
  @ViewChild(ImageCropperLocalComponent) imageCropperLocal: ImageCropperLocalComponent;

  dataSavingRequestInProgress = false;

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private store: Store,
    private errorHandling: ErrorHandlingService,
    private toastr: ToastrService,
    private tokenPageService: TokenPageService,
    private router: Router,
    private translateService: TranslateService,
    private modalService: NgbModal,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    this.store.dispatch(clearProjectArticle());
  }

  ngOnInit(): void {

    this.createForm();

    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.projectId = data.project.id;
      }
    });

    this.subs.sink = this.store.select(selectProjectArticle).subscribe((articleState) => {
      if (articleState) {
        this.SetArticleData(articleState.article);
      }
    });
  }

  private SetArticleData(article: ProjectArticle): void {
    this.date = new Date(article.date);
    this.date.setDate(new Date(article.date).getDate()+1);
    this.form.setValue({
      coverPicture: article.coverPicture,
      link: article.link,
      visibleText: article.visibleText,
      date: new Date(this.date).toISOString().slice(0, -1),
      authorPhoto: article.authorPhoto,
      name: article.authorName,
    });
    this.articleId = article.id;
    this.authorPhotoUrl = article.authorPhoto;
    this.coverPictureUrl = article.coverPicture;
    this.hiddenArticle = article.hidden;
    this.textAreaChange();
  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      coverPicture: [''],
      link: ['', [Validators.required, Validators.minLength(3)]],
      visibleText: ['', [Validators.required, Validators.min(0)]],
      date: [''],
      authorPhoto: [''],
      name: ['']
    });
  }

  textAreaChange(): void {
    this.textAreaCount = this.form.controls.visibleText.value.length;
  }

  save(): void {
    if (validateForm(this.form)) {

      this.dataSavingRequestInProgress = true;

      const command = this.form.value as CreateUpdateProjectArticleCommand;
      command.coverPicture = this.coverPictureUrl ? this.coverPictureUrl : this.blankImgUrl;
      command.link = this.form.controls.link.value;
      command.date = this.form.controls.date.value;
      command.visibleText = this.form.controls.visibleText.value;
      command.authorPhoto = this.authorPhotoUrl;
      if (this.coverId) {
        command.coverId = this.coverId;
      }
      if (this.photoId) {
        command.photoId = this.photoId;
      }
      command.name = this.form.controls.name.value;
      if (this.articleId) {
        command.articleId = this.articleId;
      }
      this.form.disable();

      this.subs.sink = this.tokenPageService.upsertArticle(this.projectId,command).subscribe((result) => {
        if (result) {
          let succesText = this.translateService.instant(this.articleId ? 'ARTICLE.MESSAGES.UPDATE_SUCCESS' : 'ARTICLE.MESSAGES.CREATE_SUCCESS');
          this.toastr.success(succesText);
          this.activeModal.close();
        }
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
      }, () => {
      });
    } else {
      this.toastr.error(this.translateService.instant('PRESS_ROOM_invalid_form'));
    }
  }

  openCoverEditing(): void {
    if(this.projectId ) {
      const modalRef = this.modalService.open(ArticleCoverUploadComponent, DEFAULT_MODAL_OPTIONS);
      modalRef.componentInstance.blankImgUrl = this.blankImgUrl;
      modalRef.componentInstance.currentImgUrl = this.coverPictureUrl;
      modalRef.componentInstance.projectId = this.projectId;
      this.subs.sink = modalRef.closed.subscribe((res?: any) => {
        if(res){
          this.coverPictureUrl = res.fileURL;
          this.coverId = res.articleFileId;
        }
      });
    }
  }
}

