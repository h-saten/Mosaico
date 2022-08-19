import { AfterContentInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import { ProjectsList } from 'mosaico-project';

@Component({
  selector: 'app-featured-project-details',
  templateUrl: './featured-project-details.component.html',
  styleUrls: ['./featured-project-details.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FeaturedProjectDetailsComponent implements OnInit, AfterContentInit, OnChanges {
  @ViewChild('contentDiv') contentDiv: ElementRef;
  @Input() project: ProjectsList;
  @Input() isProjectItem: boolean = false;
  @Input() isLandingPage: boolean = false;
  isDescriptionShown = false;
  wrapperHeight: number;
  timer: NodeJS.Timeout;

  get isMobile() {
    return window.innerWidth <= 768;
  }

  constructor(private router: Router, private cdk: ChangeDetectorRef) { }

  ngOnChanges(changes: SimpleChanges): void {
    this.cdk.detectChanges();
  }

  ngAfterContentInit(): void {
    this.refreshWrapperHeight();
    this.cdk.detectChanges();
  }

  ngOnInit(): void {
  }

  onViewDetails() {
    this.router.navigate(['/', 'project', this.project.slug]);
  }

  showDescription(e: any): void {
    this.isDescriptionShown = true;
    this.refreshWrapperHeight();
  }

  hideDescription(e: any): void {
    this.isDescriptionShown = false;
    this.refreshWrapperHeight();
  }

  refreshWrapperHeight(): void {
    setTimeout(() => {
      this.wrapperHeight = this.contentDiv?.nativeElement.offsetHeight + 5;
      this.cdk.detectChanges();
    }, 25);
  }

  onNavigate(link: string) {
    if(!this.isMobile) {
      this.router.navigate([link]);
    }
  }

  onNavigateToProject(link: string) {
    if(this.isMobile) {
      this.router.navigate([link]);
    }
  }

}
