import { ActivatedRoute, Data } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { ProjectsStatusEnum } from '../../constants';
import { PaginationResponse, SuccessResponse } from 'mosaico-base';
import { ProjectsList, MarketplaceService} from 'mosaico-project';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit, OnDestroy {

  projects: ProjectsList[] = [];
  sub: SubSink = new SubSink();
  currentStatusFilter = '';
  currentSearchText = '';
  projectsRequestFinished = false;
  rowsPerPage: number;

  totalItems = 0;
  totalPages = 0;
  currentListSize = 0;

  constructor(
    private marketplaceService: MarketplaceService,
    private activatedRoute: ActivatedRoute
    ) { }

  ngOnInit(): void {
    this.rowsPerPage = window.innerWidth > 992 ? 11 : 10;

    this.getStatusFromRouting();
    this.getListOfProjects(0);
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  private getStatusFromRouting(): void {
    this.activatedRoute.data.subscribe((response: Data) => {
      const status: ProjectsStatusEnum = response['status'];

      this.currentStatusFilter = status;
    });
  }

  private getListOfProjects(skip: number): void {
    this.sub.sink = this.marketplaceService.getProjects(skip, this.rowsPerPage, this.currentStatusFilter, this.currentSearchText)
      .subscribe((res: SuccessResponse<PaginationResponse<ProjectsList>>) => {
      if (res.ok === true && res.data) {
        if(this.currentSearchText?.length > 0) {
          this.totalItems = 0;
          this.projects = [];
          this.currentListSize = 0;
        }
        this.totalItems = res.data.total;
        if (this.totalItems > 0) {
          this.totalPages = this.totalItems / this.rowsPerPage;
          if (this.currentListSize > 0 && skip !== 0) {
            this.projects = this.projects.concat(res.data.entities);
          } else {
            this.projects = res.data.entities;
          }
          this.currentListSize = this.projects.length;
        }
      }
      this.projectsRequestFinished = true;
    });
  }

  loadMoreResults(skip: number): void {
    this.rowsPerPage = window.innerWidth > 992 ? 9 : 10;
    this.getListOfProjects(skip);
  }

  updateStatusFilter(status: string): void {
    if (status === 'All' || status === '') {
      this.currentStatusFilter = '';
    } else {
      this.currentStatusFilter = status;
    }
  }

  updateSearchText(search: string): void {
    this.currentSearchText = search;
    this.getListOfProjects(0);
  }
}
