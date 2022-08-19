import { ViewportScroller } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Page } from 'mosaico-project';
import { SubSink } from 'subsink';
import { PERMISSIONS, ProjectPathEnum } from '../../constants';
import { selectPreviewProjectPermissions } from '../../store';
import { selectProjectPage } from '../../store/project.selectors';

@Component({
  selector: 'app-project-menu',
  templateUrl: './project-menu.component.html',
  styleUrls: ['./project-menu.component.scss']
})
export class ProjectMenuComponent implements OnInit {

  @Input() slug: string;

  classProjectsMenuMobile = '';
  classBtnMenuMobile = '';
  page: Page;
  projectPathEnum: typeof ProjectPathEnum = ProjectPathEnum;
  projectPath: ProjectPathEnum;

  sub: SubSink = new SubSink();

  canEdit = false;

  constructor(
    private store: Store,
    private viewportScroller: ViewportScroller,
  ) { }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectProjectPage).subscribe((res) => {
      this.page = res;
    });
    this.getCanEdit();
  }

  private getCanEdit(): void {
    this.sub.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && (res['CAN_VIEW_DASHBOARD'] === true);
    });
  }

  goScrollUp(target: string): void {
    if (target) {
      // function needed to scroll the page up when you are already in a given tab
      // this is combined with the LayoutComponent constructor
      this.viewportScroller.setOffset([0, 90]);
      this.viewportScroller.scrollToAnchor(target);
    }
  }

  /*
  goScrollUp2(target: string) {
    document.getElementById(target).scrollIntoView({
      behavior: "smooth",
      block: "start",
      inline: "nearest"
    });
  }

  goScrollUp3() {
    this.router.navigate([], { fragment: "aboutus" });
  }
  */

}
