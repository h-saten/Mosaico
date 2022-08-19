import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectCompanyPermissions } from '../../store/business.selectors';
import { COMPANY_PERMISSIONS } from '../../constants';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-company-menu',
  templateUrl: './company-menu.component.html',
  styleUrls: ['./company-menu.component.scss']
})
export class CompanyMenuComponent implements OnInit, OnDestroy {

  @Input() slug: string;
  canEdit = false;

  private subs = new SubSink();

  constructor(private store: Store) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectCompanyPermissions).subscribe((res) => {
      this.canEdit = res && res[COMPANY_PERMISSIONS.CAN_EDIT_DETAILS];
    })
  }

}
