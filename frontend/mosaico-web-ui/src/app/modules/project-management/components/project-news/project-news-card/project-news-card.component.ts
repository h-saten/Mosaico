import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Store } from '@ngrx/store';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';

import { DEFAULT_MODAL_OPTIONS, ErrorHandlingService, FormModeEnum } from 'mosaico-base';
import { ProjectArticle, TokenPageService } from 'mosaico-project';

import { setCurrentProjectArticle } from '../../../store';
import { ProjectNewsFormComponent } from '../project-news-form/project-news-form.component';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-project-news-card',
  templateUrl: './project-news-card.component.html',
  styleUrls: ['./project-news-card.component.scss']
})
export class ProjectNewsCardComponent implements OnInit, OnDestroy {
  @Output() updateArticleList: EventEmitter<void> = new EventEmitter();
  @Input() article: ProjectArticle;
  @Input() projectId: string;

  private subs: SubSink = new SubSink();

  isHide: boolean = false;

  constructor(
    private store: Store,
    private modalService: NgbModal,
    private tokenPageService: TokenPageService,
    private toastr: ToastrService,
    public activeModal: NgbActiveModal,
    private errorHandling: ErrorHandlingService,
    private translateService: TranslateService
  ) { }

  ngOnInit(): void {
    this.isHide = this.article.hidden;
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  onHide(value: boolean) {
    this.isHide = value;
    if (this.article.hidden) {
      this.subs.sink = this.tokenPageService.displayArticle(this.article.id).subscribe((result) => {
        if (result) {
          this.translateService.get('ARTICLE.MESSAGES.DISPLAY_SUCCESS')
            .subscribe((data) => {
              this.toastr.success(data)
            });
          this.activeModal.close();
          this.updateArticleList.emit();
        }
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
      });
    } else {
      this.subs.sink = this.tokenPageService.hideArticle(this.article.id).subscribe((result) => {
        if (result) {
          this.translateService.get('ARTICLE.MESSAGES.HIDE_SUCCESS')
            .subscribe((data) => {
              this.toastr.success(data)
            });
          this.activeModal.close();
          this.updateArticleList.emit();
        }
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
      });
    }
  }

  openEditForm(value: boolean): void {
    this.store.dispatch(setCurrentProjectArticle({ article: this.article }));
    const modalRef = this.modalService.open(ProjectNewsFormComponent, DEFAULT_MODAL_OPTIONS);
    modalRef.componentInstance.projectId = this.projectId;
    modalRef.componentInstance.currentFormMode = FormModeEnum.Edit;
    modalRef.componentInstance.article = this.article;
    this.subs.sink = modalRef.closed.subscribe((r) => {
      this.updateArticleList.emit();
    })
  }

  delete(value): void {
    this.subs.sink = this.tokenPageService.deleteArticle(this.article.id).subscribe((result) => {
      if (result) {
        this.toastr.success('Article was deleted successfully');
        this.activeModal.close();
        this.updateArticleList.emit();
      }
    }, (error) => {
      this.errorHandling.handleErrorWithToastr(error);
    });
  }
}
