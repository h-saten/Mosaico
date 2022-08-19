import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnDestroy,
  ChangeDetectorRef,
} from '@angular/core';
import { LayoutService } from './core/layout.service';
import { LayoutInitService } from './core/layout-init.service';
import { ViewportScroller } from '@angular/common';
import { NavigationEnd, Router, Scroll, ActivatedRoute } from '@angular/router';
import { filter } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { selectCurrentUrl, selectShowFullWidth } from 'src/app/store/selectors';
import { ProjectPathEnum } from 'src/app/modules/project-management/constants';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
})
export class LayoutComponent implements OnInit, AfterViewInit, OnDestroy {
  // Public variables
  selfLayout = 'default';
  asideSelfDisplay: true;
  asideMenuStatic: true;
  contentClasses = '';
  contentContainerClasses = '';
  toolbarDisplay = true;
  contentExtended: false;
  asideCSSClasses: string;
  asideHTMLAttributes: any = {};
  headerMobileClasses = '';
  headerMobileAttributes = {};
  footerDisplay: boolean;
  footerCSSClasses: string;
  headerCSSClasses: string;
  headerHTMLAttributes: any = {};
  // offcanvases
  extrasSearchOffcanvasDisplay = false;
  extrasNotificationsOffcanvasDisplay = false;
  extrasQuickActionsOffcanvasDisplay = false;
  extrasCartOffcanvasDisplay = false;
  extrasUserOffcanvasDisplay = false;
  extrasQuickPanelDisplay = false;
  extrasScrollTopDisplay = false;
  asideDisplay: boolean;
  @ViewChild('ktAside', { static: true }) ktAside: ElementRef;
  @ViewChild('ktHeaderMobile', { static: true }) ktHeaderMobile: ElementRef;
  @ViewChild('ktHeader', { static: true }) ktHeader: ElementRef;

  private subs: SubSink = new SubSink();
  hideFooter = false;
  showFullWidth = true;

  constructor(
    private initService: LayoutInitService,
    private layout: LayoutService,
    private router: Router,
    private cdk: ChangeDetectorRef,
    private viewportScroller: ViewportScroller,
    private store: Store,
  ) {
    this.initService.init();

    // it is connected with the goScrollUp function from the component: ProjectMenuComponent
    this.router.events.pipe(filter(e => e instanceof Scroll)).subscribe((e: any) => {
      // console.warn(e);

      // this is fix for dynamic generated(loaded..?) content
      setTimeout(() => {
        if (e.position) {
          this.viewportScroller.scrollToPosition(e.position);
        } else if (e.anchor) {
          this.viewportScroller.scrollToAnchor(e.anchor);
        } else {
          this.viewportScroller.scrollToPosition([0, 0]);
        }
      });
    });
  }

  ngOnInit(): void {

    this.getSelectCurrentUrl();

    // Will it be useful?
    // build view by layout config settings
    this.asideDisplay = this.layout.getProp('aside.display') as boolean;
    this.toolbarDisplay = this.layout.getProp('toolbar.display') as boolean;
    this.contentContainerClasses = this.layout.getStringCSSClasses('contentContainer');
    this.asideCSSClasses = this.layout.getStringCSSClasses('aside');
    this.headerCSSClasses = this.layout.getStringCSSClasses('header');
    this.headerHTMLAttributes = this.layout.getHTMLAttributes('headerMenu');

    // tests
    // console.warn('asideDisplay: ', this.asideDisplay);
    // console.warn('toolbarDisplay: ', this.toolbarDisplay);
    // console.warn('contentContainerClasses: ', this.contentContainerClasses);
    // console.warn('asideCSSClasses: ', this.asideCSSClasses);
    // console.warn('headerCSSClasses: ', this.headerCSSClasses);
    // console.warn('headerHTMLAttributes: ', this.headerHTMLAttributes);

    // console.warn('ktAside: ', this.ktAside);
    // console.warn('ktHeaderMobile: ', this.ktHeaderMobile);
  }

  ngAfterViewInit(): void {
    // Will it be useful?
    if (this.ktHeader) {
      for (const key in this.headerHTMLAttributes) {
        if (this.headerHTMLAttributes.hasOwnProperty(key)) {
          this.ktHeader.nativeElement.attributes[key] =
            this.headerHTMLAttributes[key];
        }
      }
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private getSelectCurrentUrl(): void {
    this.subs.sink = this.store.select(selectShowFullWidth).subscribe((res) => {
      if(this.showFullWidth !== res) {
        this.showFullWidth = res;
        this.cdk.detectChanges();
      }
    });
    this.subs.sink = this.store.select(selectCurrentUrl).subscribe((res) => {
      if (res) {
        const pathUrl = res.split("/");
        if (pathUrl.length > 0) {
          this.hideFooter = pathUrl[3] === ProjectPathEnum.Fund || pathUrl[3] === ProjectPathEnum.Settings || pathUrl[3] === ProjectPathEnum.Settings + '#settings';
          this.cdk.detectChanges();
        }
      }
    });
  }

}
