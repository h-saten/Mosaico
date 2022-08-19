import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Data, Route, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of, zip } from 'rxjs';
import { Store } from "@ngrx/store";
import {
  clearCurrentProjectPreview,
  selectPreviewProjectPermissions,
  selectProjectPage,
  setCurrentProject,
  setCurrentProjectPreviewId,
  setPaymentMethods,
  setProjectPage,
  setProjectToken,
  setUserSubscribeProject
} from "../../store";
import { SubSink } from 'subsink';
import { Page, Project, ProjectService, TokenPageService, IntroVideo, Script, ScriptService } from 'mosaico-project';
import { DEFAULT_MODAL_OPTIONS, ErrorHandlingService } from 'mosaico-base';
import { TokenService } from 'mosaico-wallet';
import { ProjectPathEnum } from '../../constants';
import { TranslateService } from '@ngx-translate/core';
import { selectCurrentUrl } from 'src/app/store/selectors';
import { setProjectPackages } from '../../store/project.actions';
import { Meta } from '@angular/platform-browser';
import { MetatagsService } from '../../../../services/metatags.service';
import { PageIntroVideoComponent } from '../../modals';
import { ViewportScroller } from '@angular/common';
@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit, OnDestroy {

  private project: Project;
  private subs: SubSink = new SubSink();
  renderView: Observable<boolean>;
  slug = ''; // idProject

  currentPath: ProjectPathEnum;
  projectPathEnum: typeof ProjectPathEnum = ProjectPathEnum;
  page: Page;
  hideHeaderTokenPage = false;
  pageIntroVideos: IntroVideo;
  showIntro: string;
  canEdit = false;

  constructor(
    private viewportScroller: ViewportScroller,
    private activatedRoute: ActivatedRoute,
    private projectService: ProjectService,
    private store: Store,
    private router: Router,
    private route: ActivatedRoute,
    private errorHandler: ErrorHandlingService,
    private tokenService: TokenService,
    private tokenPageService: TokenPageService,
    private translateService: TranslateService,
    private metaService: MetatagsService,
    private modalService: NgbModal,
    private renderer: Renderer2,
    private scriptService: ScriptService
  ) { }

  ngOnInit(): void {
    this.getSelectCurrentUrl();
    this.getLanguage();
    this.getProject();
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && (res['CAN_VIEW_DASHBOARD'] === true);
    });
    const refCode = this.route.snapshot.queryParamMap.get('refCode');
    if (refCode && refCode.length > 0) {
      localStorage.setItem('refCode', refCode);
    }
    const introQuery = this.route.snapshot.queryParamMap.get('showIntro');
    if (introQuery && introQuery.length > 0) {
      this.showIntro = introQuery;
    }
    this.subs.sink = this.store.select(selectProjectPage).subscribe((res) => {
      this.page = res;
    });
  }

  private getLanguage(): void {
    this.subs.sink = this.translateService.onLangChange
      .subscribe(() => {
        this.getPageInfo();
      });
  }

  private getProject(): void {
    this.subs.sink = this.activatedRoute.paramMap.subscribe(data => {
      const projectId: string | null = data.get('projectId');
      if (projectId !== null) {
        this.store.dispatch(setCurrentProjectPreviewId({ id: projectId }));
        zip(
          this.projectService.getProject(projectId)
        ).subscribe((res) => {

          if (res && res[0].data) {

            this.store.dispatch(setUserSubscribeProject({ isSubscribed: res[0].data.isSubscribed }));

            this.project = res[0].data.project;
            if (!this.project) {
              this.router.navigateByUrl('/projects');
            }

            this.slug = this.project.slug;
            if (this.slug && projectId !== this.slug) {
              this.router.navigate(['/project/' + this.slug]);
            }

            this.saveInStore(this.project);
            this.renderView = of(true);

            if (this.project.tokenId && this.project.tokenId.length > 0) {
              this.subs.sink = this.tokenService.getToken(this.project.tokenId).subscribe((res2) => {
                if (res2 && res2.data) {
                  this.store.dispatch(setProjectToken({ token: res2.data }));
                }
              });
            }

            this.getPageInfo();

            if (this.project.paymentMethods && this.project.paymentMethods.length > 0) {
              this.store.dispatch(setPaymentMethods({ paymentMethods: this.project.paymentMethods }));
            }
            this.getPageIntroVideos();
          }
        }, (error: HttpErrorResponse) => {
          this.errorHandler.handleErrorWithRedirect(error, '/projects');
        });

      } else {
        this.router.navigateByUrl('/projects');
      }
    });
  }

  private getSelectCurrentUrl(): void {
    this.subs.sink = this.store.select(selectCurrentUrl).subscribe((res) => {
      if (res) {
        const tabUrl = res.split("/");
        if (tabUrl.length > 0) {
          this.hideHeaderTokenPage = tabUrl?.includes(ProjectPathEnum.Fund) ||
            tabUrl?.some((t) => t.includes(ProjectPathEnum.Fund) || t.includes(ProjectPathEnum.PaymentConfirmed)) ||
            tabUrl?.includes(ProjectPathEnum.Settings) ||
            tabUrl?.includes(ProjectPathEnum.PaymentConfirmed) ||
            tabUrl?.includes(ProjectPathEnum.Settings + '#settings');
        }
      }
    });
  }

  private saveInStore(project: Project): void {
    this.store.dispatch(setCurrentProject(project));
  }

  private getPageInfo(): void {
    if (this.project.pageId && this.project.pageId.length > 0) {
      this.subs.sink = this.tokenPageService.getPage(this.project.pageId).subscribe((p) => {
        if (p && p.data) {
          this.store.dispatch(setProjectPage({ page: p.data }));
          this.store.dispatch(setProjectPackages({ packages: p.data.investmentPackages }));
          this.updateMetaTags(this.project, p.data);
          this.loadScripts(p.data?.scripts);
        }
      });
    }
  }

  private loadScripts(scripts: Script[] = []): void {
    if (scripts) {
      scripts.sort((a, b) => a.order - b.order).forEach((s) => {
        if (s.isEnabled === true && s.src && s.src.length > 0) {
          let script: HTMLScriptElement;
          if (s.isExternal === true) {
            script = this.scriptService.loadExternalScript(this.renderer, s.src);
          }
          else if (s.isExternal === false) {
            script = this.scriptService.loadJsScript(this.renderer, s.src);
          }
        }
      });
    }
  }

  private getPageIntroVideos(): void {
    if (this.project.pageId && this.project.pageId.length > 0) {
      this.subs.sink = this.tokenPageService.getIntroVideos(this.project.pageId).subscribe((res) => {
        if (res && res.data) {
          this.pageIntroVideos = res.data.introVideo;
          this.showIntroVideo();
        }
      });
    }
  }

  private updateProjectVisit(): void {
    this.subs.sink = this.projectService.updateProjectVisitor(this.project.id).subscribe((res) => {

    });
  }

  private showIntroVideo(): void {
    if (this.pageIntroVideos && this.project.id) {
      if (this.showIntro && this.showIntro === "true") {
        const modalRef = this.modalService.open(PageIntroVideoComponent, { "modalDialogClass": "page-into-modal" });
        modalRef.componentInstance.showVideoUrl = this.pageIntroVideos.showLocalVideo;
        modalRef.componentInstance.videoUrl = this.pageIntroVideos.videoUrl;
        modalRef.componentInstance.videoExternalLink = this.pageIntroVideos.videoExternalLink;
      } else {
        this.subs.sink = this.projectService.getIsProjectVisited(this.project.id).subscribe((res) => {
          if (!res.data) {
            const modalRef = this.modalService.open(PageIntroVideoComponent, { "modalDialogClass": "page-into-modal" });
            modalRef.componentInstance.showVideoUrl = this.pageIntroVideos.showLocalVideo;
            modalRef.componentInstance.videoUrl = this.pageIntroVideos.videoUrl;
            modalRef.componentInstance.videoExternalLink = this.pageIntroVideos.videoExternalLink;
            this.updateProjectVisit();
          }
        });
      }
    }
  }

  ngOnDestroy(): void {
    this.store.dispatch(clearCurrentProjectPreview());
    this.subs.unsubscribe();
    this.metaService.reset();
  }

  private updateMetaTags(project: Project, page: Page): void {
    this.metaService.setData(page, project);
  }

  goScrollUp(target: string): void {
    if (target) {
      // function needed to scroll the page up when you are already in a given tab
      // this is combined with the LayoutComponent constructor
      this.viewportScroller.setOffset([0, 140]);
      this.viewportScroller.scrollToAnchor(target);
    }
  }
}
