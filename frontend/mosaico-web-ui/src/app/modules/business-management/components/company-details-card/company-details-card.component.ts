import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Company, CompanyService } from 'mosaico-dao';
import { SubSink } from 'subsink';
import { selectCompanyPreview } from '../../store/business.selectors';

@Component({
  selector: 'app-company-details-card',
  templateUrl: './company-details-card.component.html',
  styleUrls: ['./company-details-card.component.scss']
})
export class CompanyDetailsCardComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  company: Company;
  links:any;
  noLinkFound:boolean;

  constructor(private companyService: CompanyService, private store: Store) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectCompanyPreview).subscribe((res) => {
      this.company = res;
    });
    this.getSocialLinks();
  }

  getSocialLinks() {
    this.subs.sink = this.companyService.getCompanySocialLinks(this.company.id).subscribe((result) => {
      if (result && result.data) {
        this.links=result.data;
        this.noLinkFound = Object.values(this.links).every(s => s === null);
      }
    });
  }

}
