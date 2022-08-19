import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Token } from 'mosaico-wallet';
import { selectProjectPreviewToken } from 'src/app/modules/project-management/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-project-admin-token-management',
  templateUrl: './token-management.component.html',
  styleUrls: ['./token-management.component.scss']
})
export class ProjectAdminTokenManagementComponent implements OnDestroy, OnInit {
  sub = new SubSink();
  token: Token;

  constructor(private store: Store) { }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectProjectPreviewToken).subscribe((res) => {
      this.token = res;
    });
  }

}
