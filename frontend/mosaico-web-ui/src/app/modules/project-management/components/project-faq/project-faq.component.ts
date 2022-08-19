import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Faq, TokenPageService } from 'mosaico-project';
import { SubSink } from 'subsink';
import { Store } from "@ngrx/store";
import { Observable, of } from 'rxjs';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { DEFAULT_MODAL_OPTIONS, ErrorHandlingService, FormModeEnum } from 'mosaico-base';
import { selectPreviewProjectPermissions, setProjectFaqs, selectProjectFaqs, selectProjectPreview  } from '../../store';
import { PERMISSIONS } from '../../constants';
import { ProjectFaqModalComponent } from '..';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-project-faq',
  templateUrl: './project-faq.component.html',
  styleUrls: ['./project-faq.component.scss']
})
export class ProjectFaqComponent implements OnInit, OnDestroy {

  itemList: Faq[] = [];
  listRequestFinished$: Observable<boolean>;

  pageId = '';
  projectId = '';

  private subs: SubSink = new SubSink();

  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  canEdit = false;

  @ViewChild(ProjectFaqModalComponent) modalFaq: ProjectFaqModalComponent;

  private currentLang = '';

  get isMobile() {
    return window.innerWidth <= 576;
  }

  constructor(
    private tokenPageService: TokenPageService,
    private router: Router,
    private store: Store,
    private errorHandling: ErrorHandlingService,
    private modalService: NgbModal,
    private translateService: TranslateService,
    private toastr: ToastrService
  ) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });

    this.getLanguage();
    this.getProject();
  }

  private getLanguage(): void {
    this.currentLang = this.translateService.currentLang;

    this.subs.sink = this.translateService.onLangChange
      .subscribe((langChangeEvent: LangChangeEvent) => {
        this.currentLang = langChangeEvent.lang;

        this.getListFromApi();
      });
  }

  private getProject(): void {

    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.projectId = data.projectId;
        this.pageId = data.project?.pageId;

        if (this.pageId && this.pageId.length > 0) {
          this.getProjectFaqs();
        } else {
          this.router.navigateByUrl(`'/projects'`);
        }

      }
    });
  }

  private getProjectFaqs(): void {

    this.subs.sink = this.store.select(selectProjectFaqs).subscribe(data => {
      if (data && data.length > 0) {
        this.itemList = data;
        this.listRequestFinished$ = of(true);
      } else {
        this.getListFromApi();
      }
    });
  }

  getListFromApi(update?: boolean): void {
    if (this.pageId && this.pageId.length > 0) {
      this.subs.sink = this.tokenPageService.getFAQs(this.pageId, this.currentLang).subscribe((res) => {
        if (res?.data?.faqs && res?.data?.faqs.length > 0) {
          this.itemList = res.data.faqs;
          this.store.dispatch(setProjectFaqs({ faqs: this.itemList }));
        } else {
          this.itemList = [];

          if (update === true) {
            this.store.dispatch(setProjectFaqs({ faqs: [] }));
          }
        }

        this.listRequestFinished$ = of(true);

      }, (error: HttpErrorResponse) => {
        this.errorHandling.handleErrorWithRedirect(error, `'/projects'`);
      });
    }
  }

  openModalAdd(): void {
    const modalRef = this.modalService.open(ProjectFaqModalComponent, DEFAULT_MODAL_OPTIONS);

    modalRef.componentInstance.currentFormMode = FormModeEnum.Add;
    modalRef.componentInstance.pageId = this.pageId;
    modalRef.componentInstance.projectId = this.projectId;

    this.subs.sink = modalRef.closed.subscribe((res?: boolean) => {
      if(res){
        setTimeout(() => {
          this.getListFromApi(true);
        }, 1000);
      }
    });
  }

  openModalEdit(id: string): void {
    if (id) {
      const modalRef = this.modalService.open(ProjectFaqModalComponent, DEFAULT_MODAL_OPTIONS);

      modalRef.componentInstance.currentFormMode = FormModeEnum.Edit;
      modalRef.componentInstance.showList = false;
      modalRef.componentInstance.pageId = this.pageId;
      modalRef.componentInstance.projectId = this.projectId;
      modalRef.componentInstance.faqId = id;

      this.subs.sink = modalRef.closed.subscribe((res?: boolean) => {
        if (res && res === true) {
          setTimeout(() => {
            this.getListFromApi(true);
          }, 1000);
        }
      });
    }
  }

  delete(id: string): void {
    this.subs.sink = this.tokenPageService.deleteFAQ(this.pageId, id).subscribe((result) => {
        if (result) {
          this.translateService.get('FAQ_DELETED').subscribe((t) => {
            this.toastr.success(t);
          });
          // this.toastr.success('Faq were successfully delete');

          setTimeout(() => {
            this.getListFromApi(true);
          }, 1000);
        }
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
      }, () => {
    });
  }
}
