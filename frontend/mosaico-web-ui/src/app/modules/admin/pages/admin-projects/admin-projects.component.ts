import { Component, OnDestroy, OnInit } from '@angular/core';
import { ErrorHandlingService } from 'mosaico-base';
import { InvitationsService, ProjectService, ProjectsList, MarketplaceService, AdminProjectService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { LazyLoadEvent } from 'primeng/api';
import { Subject } from 'rxjs';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-admin-projects',
  templateUrl: './admin-projects.component.html',
  styleUrls: ['./admin-projects.component.scss']
})
export class AdminProjectsComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();
  projects: ProjectsList[] = [];
  currentSkip: number = 0;
  currentTake: number = 30;
  page = 1;
  pageSize = 30;
  totalRecords: number;
  updating = false;
  isLoading = false;

  constructor(
    private projectService: AdminProjectService,
    private errorHandler: ErrorHandlingService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {

  }

  reloadProjects(): void {
    this.sub.sink = this.projectService.getProjects(this.currentSkip, this.currentTake).subscribe((res) => {
      if (res && res.data && res.data.entities) {
        this.projects = res.data.entities;
        this.totalRecords = res.data.total;
      }
      this.isLoading = false;
    }, (error) => {
      this.errorHandler.handleErrorWithToastr(error);
      this.isLoading = false;
    });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  accept(id: string): void {
    this.sub.sink = this.projectService.approve(id).subscribe((res) => {
      if (res) {
        this.toastr.success('Project accepted');
        this.reloadProjects();
      }
    }, (error) => {this.errorHandler.handleErrorWithToastr(error);});
  }

  onChangeVisibility(id: any, e: any) {
    this.updating = true;
    this.sub.sink = this.projectService.updateVisibility(id, { visibility: e.target.checked }).subscribe((res) => {
      if (res) {
        this.toastr.success('Visibility updated');
        this.updating = false;
      }
    }, (error) => {this.errorHandler.handleErrorWithToastr(error);});
  }

  fetchProjects(event: LazyLoadEvent): void {
    this.isLoading = true;
    this.currentSkip = event.first;
    this.currentTake = event.rows;
    this.reloadProjects();
  }

}
