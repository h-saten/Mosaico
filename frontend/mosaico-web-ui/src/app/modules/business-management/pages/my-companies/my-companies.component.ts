import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { ErrorHandlingService } from 'mosaico-base';
import { CompanyList, CompanyService } from 'mosaico-dao';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
@Component({
  selector: 'app-my-companies',
  templateUrl: './my-companies.component.html',
  styleUrls: ['./my-companies.component.scss']
})
export class MyCompaniesComponent implements OnInit, OnDestroy {
  isLoading = true;
  sub: SubSink = new SubSink();
  companies: CompanyList[] = [];
  constructor(private companyService: CompanyService, private store: Store, private errorHandler: ErrorHandlingService, private router: Router) { }

  ngOnInit(): void {
    this.loadCompanies();
  }

  loadCompanies(): void {
    this.sub.sink = this.store.select(selectUserInformation).subscribe((user) => {
      if (user.id) {
        this.sub.sink = this.companyService.getUserCompanies(0, 100, user.id).subscribe((res) => {
          if (res && res.data && res.data.entities) {
            this.companies = res.data.entities;
            this.isLoading = false;
          }
        });
      }
    }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.isLoading = false; });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

}
