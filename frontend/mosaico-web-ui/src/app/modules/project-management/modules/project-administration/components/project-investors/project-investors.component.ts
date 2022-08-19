import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { SubSink } from 'subsink';
import { ErrorHandlingService, FilterOptions, FilterParams } from 'mosaico-base';
import { ProjectInvestor, ProjectService } from 'mosaico-project';
import { selectProjectPreview } from '../../../../store';

@Component({
  selector: 'app-project-investors',
  templateUrl: './project-investors.component.html',
  styleUrls: ['./project-investors.component.scss']
})
export class ProjectInvestorsComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();

  investors: ProjectInvestor[] = [];
  filters: FilterParams[] = [];
  expandedRow: number = -1;
  isLoading: boolean = true;
  projectId: string;
  currentSkip: number = 0;
  currentTake: number = 10;
  totalRecords: number;
  page: number = 1;
  pageSize: number = 10;

  constructor(
    private store: Store,
    private errorHandler: ErrorHandlingService,
    private projectService: ProjectService
  ) { }

  ngOnInit(): void {
    this.getProjectId();
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  private getProjectId(): void {
    this.sub.sink = this.store.select(selectProjectPreview).subscribe((project) => {
      if (project?.project?.id) {
        this.projectId = project?.project?.id;
      }
    });
  }

  fetchInvestors(event: LazyLoadEvent): void {
    this.isLoading = true;
    this.currentSkip = event.first;
    this.currentTake = event.rows;

    this.fillFilters(event);

    this.sub.sink = this.projectService.getInvestors(this.projectId, this.currentSkip, this.currentTake).subscribe(
      res => {
        if(res?.data.items) {
          this.investors = res.data.items;
          this.totalRecords = res.data.totalItems;
          this.isLoading = false;
        }
      },
      error => {
        this.errorHandler.handleErrorWithToastr(error);
        this.isLoading = false;
      }
    )
  }

  public getBalance(investor: ProjectInvestor, symbol: string): number {
    return investor?.balances?.find((b) => b.symbol === symbol)?.balance;
  }

  private fillFilters(event: LazyLoadEvent): void {
    const filtersList: FilterParams[] = [];
    if (event?.filters) {
      for (const [key, value] of Object.entries(event.filters)) {
        const capitalizedMatchMode = value.matchMode.charAt(0).toUpperCase() + value.matchMode.slice(1);
        const filterOption = <FilterOptions>FilterOptions[capitalizedMatchMode];
        filtersList.push({ columnName: key, filterValue: value.value, filterOption: filterOption });
      };
    }
    this.filters = filtersList;
  }

  toggleRow(index: number): void {
    if(index !== this.expandedRow) {
      this.expandedRow = index;
    }
    else {
      this.expandedRow = -1;
    }
  }
}
