import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import {RequestList} from '../../models';
import {AdminService} from '../../services';
import { ProjectsList } from 'mosaico-project';
@Component({
  selector: 'app-admin-user-deletion-requests',
  templateUrl: './admin-user-deletion-requests.component.html',
  styleUrls: ['./admin-user-deletion-requests.component.scss']
})
export class AdminUserDeletionRequestsComponent implements OnInit, OnDestroy {
  sub: SubSink = new SubSink();
  projects: ProjectsList[] = [];
  requests: RequestList[] = [];
  constructor(
    private adminService: AdminService
  ) { }

  ngOnInit(): void {
    this.loadRequests();
  }


  loadRequests(): void {
    this.sub.sink = this.adminService.getDeletionRequests().subscribe((res) => {
      if(res && res.data && res.data.entities){
        this.requests = res.data.entities;
      }
    });
  }

  restoreAccount(id: string): void {
    this.sub.sink = this.adminService.restoreAccount(id).subscribe((response) => {
      if(response && response.data){
        this.loadRequests();
      }
    })
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }


}
