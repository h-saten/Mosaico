import { Component, OnDestroy, OnInit } from '@angular/core';
import { Company } from 'mosaico-dao';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { ActivatedRoute } from '@angular/router';
import { selectPreviewProject, setCurrentProjectCompany } from '../../../project-management/store';
import { CompanyService } from 'mosaico-dao';

@Component({
  selector: 'app-project-footer',
  templateUrl: './project-footer.component.html',
  styleUrls: ['./project-footer.component.scss']
})
export class ProjectFooterComponent implements OnInit, OnDestroy {

  public company: Company;
  public currentCompanyId: string;
  public logoUrl: string;
  public sub: SubSink = new SubSink();

  constructor(
    private store: Store,
    private companyService: CompanyService) { }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectPreviewProject).subscribe((prj) => {
      if (prj) {
        this.currentCompanyId = prj.companyId;
        this.getCompanyDetails();
      }
    });
  }


  getCompanyDetails(): void {
    if (this.currentCompanyId) {
      this.companyService.getCompany(this.currentCompanyId).subscribe((res) => {
        if (res && res.data) {
          this.company = res.data.company;
          this.extractCompanyLogo(res.data.company);
          this.store.dispatch(setCurrentProjectCompany({company: this.company}));
        }
      });
    }
  }

  extractCompanyLogo(company: Company): void {
    if (company) {
      this.logoUrl = company.logoUrl;
    }
  }
}
