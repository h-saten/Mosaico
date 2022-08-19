import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Company, CompanyService } from 'mosaico-dao';
import { COMPANY_PERMISSIONS } from '../../constants';
import { selectCompanyPermissions, selectCompanyPreview } from '../../store/business.selectors';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-company-description',
  templateUrl: './company-description.component.html',
  styleUrls: ['./company-description.component.scss']
})
export class CompanyDescriptionComponent implements OnInit {
  company: Company;
  isLoaded = false;
  canEdit: boolean = false;
  subs = new SubSink();
  constructor(
    private store: Store,
  ) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectCompanyPreview).subscribe((res) => {
      this.company = res;
      this.store.select(selectCompanyPermissions).subscribe((res) => {
        this.canEdit = res && res[COMPANY_PERMISSIONS.CAN_EDIT_DETAILS];
      });
    });
  }


}
