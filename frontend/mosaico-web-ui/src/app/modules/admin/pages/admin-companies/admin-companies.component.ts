import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { CompanyService, Verification } from 'mosaico-dao';
import { SubSink } from 'subsink';
@Component({
  selector: 'app-admin-companies',
  templateUrl: './admin-companies.component.html',
  styleUrls: ['./admin-companies.component.scss']
})
export class AdminCompaniesComponent implements OnInit, OnDestroy {

  sub: SubSink = new SubSink();
  verifications: Verification[] = [];
  constructor(private companyService: CompanyService, private store: Store) { }

  ngOnInit(): void {
    this.loadVerifications();
  }

  loadVerifications(): void {
        this.sub.sink = this.companyService.getAllVerifications(0, 100).subscribe((res) => {
          if (res && res.data && res.data.entities) {
            this.verifications = res.data.entities;
          }
        });
      }
  
approveCompany(id: any): void {
  this.sub.sink = this.companyService.approveCompany(id).subscribe((res) => {
    if (res && res.ok) {
      this.loadVerifications();
    }
  });
}

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

}
