import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { UserInformation } from 'src/app/modules/user-management/models';
import { UserService } from 'src/app/modules/user-management/services';
import { selectIsAuthorized, selectUserInformation } from 'src/app/modules/user-management/store';
import { SubSink } from 'subsink';
import { LazyLoadEvent } from 'primeng/api';

import Swal from 'sweetalert2'
import { eq } from 'lodash';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: [
    './admin-users.component.scss'
  ]
})
export class AdminUsersComponent implements OnInit {
  collapsed = true;
  sub: SubSink = new SubSink();
  isAuthorized$: Observable<boolean>;
  loading: boolean = true;

  activateComment:string;
  activateBtnText:string;
  activateSuccessTitle:string;
  activateSuccessComment:string;

  deactivateComment:string;
  deactivateBtnText:string;
  deactivateSuccessTitle:string;
  deactivateSuccessComment:string;
  emptyReason:string;
  sure:string;


  currentSearchName: string=null;
  currentSearchEmail: string=null;
  currentSkip:number=0;
  currentTake:number=10;
  page = 1;
  pageSize =10;
  totalRecords: number;
  allUser: UserInformation[]=[];
  datasource: UserInformation[];
  cols:any;
  comment:string;
  btnText:string;
  successTitle:string;
  successComment:string;
  user: UserInformation;

  constructor(private store: Store, private users: UserService,private translateService: TranslateService) {
    this.activateComment = this.translateService.instant('DEACTIVATE_USER.activate_comment');
    this.activateBtnText = this.translateService.instant('DEACTIVATE_USER.activate_btnText');
    this.activateSuccessTitle = this.translateService.instant('DEACTIVATE_USER.activate_SuccessTitle');
    this.activateSuccessComment = this.translateService.instant('DEACTIVATE_USER.activate_SuccessComment');

    this.deactivateComment = this.translateService.instant('DEACTIVATE_USER.deactivate_comment');
    this.deactivateBtnText = this.translateService.instant('DEACTIVATE_USER.deactivate_btnText');
    this.deactivateSuccessTitle = this.translateService.instant('DEACTIVATE_USER.deactivate_SuccessTitle');
    this.deactivateSuccessComment = this.translateService.instant('DEACTIVATE_USER.deactivate_SuccessComment');

    this.emptyReason = this.translateService.instant('DEACTIVATE_USER.emptyReason');
    this.sure = this.translateService.instant('DEACTIVATE_USER.sure');
  }

  ngOnInit(): void {
    this.isAuthorized$ = this.store.select(selectIsAuthorized);
      this.store.select(selectUserInformation).subscribe((res) => {
        if(res && res.id && res.id.length > 0){
          this.user = res;
        }
      })
  }

  allUsers(event: LazyLoadEvent){
    this.loading = true;
    this.currentSkip=event.first;
    this.currentTake=event.rows;
    this.currentSearchName=event.filters.name?.value;
    this.currentSearchEmail=event.filters.email?.value;
    setTimeout(() => {
      this.getData(event.first,event.rows,event.filters.name?.value,event.filters.email?.value);
    }, 1000);
  }

  getData(first: number, rows: number, name: string = undefined, email: string = undefined){
    this.sub.sink = this.users.getUsers(first, rows, name, email).subscribe((res) => {
      this.allUser=res.data.users.entities;
      this.totalRecords = res.data.users.total;
      this.loading = false;
    });
  }

  //deactivate user
  deactivateuser(id:string,status:boolean){
    if(status==false){
      this.comment=this.activateComment;
      this.btnText=this.activateBtnText;
      this.successTitle=this.activateSuccessTitle;
      this.successComment=this.activateSuccessComment
    }
    else {
      this.comment=this.deactivateComment;
      this.btnText=this.deactivateBtnText;
      this.successTitle=this.deactivateSuccessTitle;
      this.successComment=this.deactivateSuccessComment
    };
    Swal.fire({
      title: this.sure,
      html: this.comment,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: this.btnText,
      preConfirm: () => {
        const reason = (Swal.getPopup()?.querySelector('#reason') as HTMLInputElement)?.value;
        return { reason: reason }
      }
    }).then((result) => {
      if (result.isConfirmed) {
        this.users.deactivateUser({id:id,status:status,reason:result.value?.reason}).subscribe((res)=>{
          if (res.ok) {
            Swal.fire(
              this.successTitle,
              this.successComment,
              'success'
            )
          };
          this.loading = true;
          this.getData(this.currentSkip,this.currentTake,this.currentSearchName,this.currentSearchEmail);
          this.loading = false;
        })
      }
    })
  }
}
