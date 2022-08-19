import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { DEFAULT_MODAL_OPTIONS, ErrorHandlingService, FormModeEnum } from 'mosaico-base';
import { ProjectPackage, TokenPageService } from 'mosaico-project';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';
import { ProjectPackagesModalComponent } from '..';
import { PERMISSIONS } from '../../constants';
import { selectPreviewProjectPermissions, selectProjectPackages, selectProjectPreview, setProjectPackages } from '../../store';

// import Swiper core and required components
import SwiperCore , {
  Lazy,
  Navigation,
  Pagination,
  Scrollbar,
  Mousewheel,
  A11y,
  Virtual,
  Zoom,
  Autoplay,
  Thumbs,
  Controller,
  EffectCoverflow,
  SwiperOptions
} from 'swiper';
import { ToastrService } from 'ngx-toastr';

// install Swiper components
SwiperCore.use([
  Navigation,
  Pagination,
  Lazy,
  Mousewheel,
  Scrollbar,
  A11y,
  Virtual,
  Zoom,
  Autoplay,
  Thumbs,
  Controller,
  EffectCoverflow
]);

@Component({
  selector: 'app-project-packages',
  templateUrl: './project-packages.component.html',
  styleUrls: ['./project-packages.component.scss']
})
export class ProjectPackagesComponent implements OnInit, OnDestroy {

  projectPackagesList: ProjectPackage[] = [];
  listRequestFinished$ = new BehaviorSubject<boolean>(false);

  projectId = '';
  pageId = '';

  private subs: SubSink = new SubSink();

  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  canEdit = false;

  config: SwiperOptions = {
    spaceBetween: 32,
    slidesPerView: 'auto',
    loop: true,
    pagination: {
      clickable: true
    },
    a11y: {
      enabled: false
    }
  }

  @ViewChild(ProjectPackagesModalComponent) modalFaq: ProjectPackagesModalComponent;

  private currentLang = '';

  constructor(
    private tokenPageService: TokenPageService,
    private router: Router,
    private store: Store,
    private errorHandling: ErrorHandlingService,
    private modalService: NgbModal,
    private translateService: TranslateService,
    private toastr: ToastrService
  ) {
  }

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
          this.getProjectPackages();
        } else {
          this.router.navigateByUrl(`'/projects'`);
        }
      }
    });
  }

  private getProjectPackages(): void {
    this.subs.sink = this.store.select(selectProjectPackages).subscribe(data => {
      if (data && data.length > 0) {
        this.projectPackagesList = data;
      } else {
        this.getListFromApi();
      }
      this.listRequestFinished$.next(true);
    });
  }

  getListFromApi(update?: boolean): void {
    if (this.pageId && this.pageId.length > 0) {
      this.subs.sink = this.tokenPageService.getPackages(this.pageId, this.currentLang).subscribe((res) => {
        if (res?.data?.packages && res?.data?.packages.length > 0) {
          this.projectPackagesList = res.data.packages;
          this.store.dispatch(setProjectPackages({ packages: this.projectPackagesList }));
        } else {
          this.projectPackagesList = [];

          if (update === true) {
            this.store.dispatch(setProjectPackages({ packages: [] }));
          }
        }

        this.listRequestFinished$.next(true);

      }, (error: HttpErrorResponse) => {
        this.errorHandling.handleErrorWithRedirect(error, `'/projects'`);
      });
    }
  }

  openModalAdd(): void {
    const modalRef = this.modalService.open(ProjectPackagesModalComponent, DEFAULT_MODAL_OPTIONS);

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
      const modalRef = this.modalService.open(ProjectPackagesModalComponent, DEFAULT_MODAL_OPTIONS);

      modalRef.componentInstance.currentFormMode = FormModeEnum.Edit;
      modalRef.componentInstance.showList = false;
      modalRef.componentInstance.pageId = this.pageId;
      modalRef.componentInstance.projectId = this.projectId;
      modalRef.componentInstance.packageId = id;

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
    this.subs.sink = this.tokenPageService.deletePackage(this.pageId, id).subscribe((result) => {
        if (result) {
          this.translateService.get('PACKAGE_DELETED').subscribe((t) => {
            this.toastr.success(t);
          });

          setTimeout(() => {
            this.getListFromApi(true);
          }, 1000);
        }
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
      }, () => {
    });
  }

  onSwiper([swiper]): void {

  }
  onSlideChange(): void {

  }
}
