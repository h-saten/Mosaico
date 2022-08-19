import { Component, OnDestroy, OnInit } from '@angular/core';
import { SwiperOptions } from 'swiper';
import { SubSink } from 'subsink';
import { PaginationResponse, SuccessResponse } from 'mosaico-base';
import { MarketplaceService, ProjectsList } from 'mosaico-project';

@Component({
  selector: 'app-section-project-carousel',
  templateUrl: './section-project-carousel.component.html',
  styleUrls: ['./section-project-carousel.component.scss']
})
export class SectionProjectCarouselComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();
  overlappedProjectId: string | null = null;
  isButtonShown: boolean = true;
  projects: ProjectsList[] = [];
  projectsRequestFinished = false;
  pageSize = 15;

  config: SwiperOptions = {
    slidesPerView: 'auto',
    spaceBetween: 25,
    centeredSlides: true,
    pagination: {
      clickable: true,
      dynamicBullets: true,
      dynamicMainBullets: 4
    },
    breakpoints: {
      576: {
        autoHeight: false,
        centeredSlides: false
      }
    }
  };

  constructor(private marketplaceService: MarketplaceService) { }

  ngOnInit(): void {
    this.getListOfProjects(0, this.pageSize);
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  private getListOfProjects(skip: number, take: number): void {
    this.sub.sink = this.marketplaceService.getProjectsForLanding({ skip, take, landingOnly: true })
      .subscribe((res: SuccessResponse<PaginationResponse<ProjectsList>>) => {
        if (res.ok === true && res.data) {
          this.projects = res.data.entities;
          this.config.loop = res.data.total > 1;
        }
        this.projectsRequestFinished = true;
      });
  }

  onSlideChange() {
    this.overlappedProjectId = null;
  }

  
  onShowProjectCard(id: string | null) {
    this.overlappedProjectId = id;
  }
}
