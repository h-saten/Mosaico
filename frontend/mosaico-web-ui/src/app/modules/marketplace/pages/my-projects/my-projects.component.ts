import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { PaginationResponse, SuccessResponse } from 'mosaico-base';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
import { ProjectsList, MarketplaceService } from 'mosaico-project';

@Component({
  selector: 'app-my-projects',
  templateUrl: './my-projects.component.html',
  styleUrls: ['./my-projects.component.scss']
})
export class MyProjectsComponent implements OnInit, OnDestroy {

  sub: SubSink = new SubSink();
  projects: ProjectsList[] = [];

  projectsRequestFinished = false;

  currentStatusFilter = '';

  totalItems = 0;
  totalPages = 0;
  rowsPerPage = 9;
  currentListSize = 0;

  constructor(
    private marketplaceService: MarketplaceService,
    private store: Store
  ) { }

  ngOnInit(): void {
    this.getListOfProjects(0);
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  getListOfProjects(pageNumber: number): void {

    if (pageNumber === 0) {
      this.projectsRequestFinished = false;
    }

    this.sub.sink = this.store.select(selectUserInformation).subscribe((user) => {

      if (user.id) {
        this.sub.sink = this.marketplaceService.getUserProjects(pageNumber, this.rowsPerPage, user.id)
          .subscribe((res: SuccessResponse<PaginationResponse<ProjectsList>>) => {

          if (res && res.ok === true && res.data && res.data.entities) {
            this.totalItems = res.data.total;

            if (this.totalItems > 0) {
              this.totalPages = this.totalItems / this.rowsPerPage;

              if (this.currentListSize > 0 && pageNumber !== 0) {
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
    });
  }

  loadMoreResults(pageNumber: number): void {
    this.getListOfProjects(pageNumber);
  }


  updateStatusFilter(status: string): void {
    this.currentStatusFilter = status;
    this.getListOfProjects(0);
  }
}
