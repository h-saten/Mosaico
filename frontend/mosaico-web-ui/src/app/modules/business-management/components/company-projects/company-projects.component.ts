import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Company } from 'mosaico-dao';
import { Project, ProjectService, ProjectsList } from 'mosaico-project';
import { SubSink } from 'subsink';
import { COMPANY_PERMISSIONS } from '../../constants';
import { selectCompanyPermissions, selectCompanyPreview } from '../../store/business.selectors';

@Component({
  selector: 'app-company-projects',
  templateUrl: './company-projects.component.html',
  styleUrls: ['./company-projects.component.scss']
})
export class CompanyProjectsComponent implements OnInit, OnDestroy {
  projects: ProjectsList[];
  company: Company;
  isLoaded = false;
  subs = new SubSink();
  canEdit: boolean = false;

  constructor(private store: Store, private projectService: ProjectService) { }
  
  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectCompanyPreview).subscribe((res) => {
      this.company = res;
      this.loadProjects();
    });
    this.getCanEdit();
  }

  loadProjects(): void {
    if(this.company){
      this.projectService.getCompanyProjects(0, 50, this.company.id).subscribe((res) => {
        this.projects = res?.data?.entities;
        this.isLoaded = true;
      });
    }
  }

  getCanEdit(): void {
    this.subs.sink = this.store.select(selectCompanyPermissions).subscribe((res) => {
      this.canEdit = res && (res[COMPANY_PERMISSIONS.CAN_EDIT_DETAILS]);
    });
  }

}
