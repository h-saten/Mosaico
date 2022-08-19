import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ErrorHandlingService } from 'mosaico-base';
import { PersonalVesting, Token, TOKEN_PERMISSIONS, VestingService } from 'mosaico-wallet';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';
import { selectToken, selectTokenPermissions } from '../../store';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-personal-vesting-management',
  templateUrl: './personal-vesting-management.component.html',
  styleUrls: ['./personal-vesting-management.component.scss']
})
export class PersonalVestingManagementComponent implements OnInit, OnDestroy {

  subs = new SubSink();
  canEdit = false;
  isDataLoaded = false;
  vestings: PersonalVesting[] = [];
  token: Token;
  isLoading$ = new BehaviorSubject<boolean>(false);
  expandedRow  = -1;
  
  constructor(private store: Store, private vestingService: VestingService, private errorHandler: ErrorHandlingService,
    private toastr: ToastrService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectTokenPermissions).subscribe((res) => {
      this.canEdit = res && res[TOKEN_PERMISSIONS.CAN_EDIT] === true;
    });
    this.subs.sink = this.store.select(selectToken).subscribe((t) => {
      this.token = t;
      if(this.token) {
        this.loadData();
      }
    });
  }

  onCopied(): void {
    this.toastr.success('Copied');
  }

  loadData(force = false): void {
    if((!this.isDataLoaded || force === true) && !this.isLoading$.value) {
      this.isLoading$.next(true);
      this.subs.sink = this.vestingService.getPersonalVesting(this.token.id).subscribe((res) => {
        this.vestings = res?.data?.vestings;
        this.isLoading$.next(false);
        this.isDataLoaded = true;
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        this.isLoading$.next(false);
        this.isDataLoaded = false;
      });
    }
  }

  onPersonalVestingClosed(e: boolean): void {
    if(e === true) {
      this.loadData(true);
    }
  }

  toggleRow(index: number): void {
    if(index !== this.expandedRow) {
      this.expandedRow = index;
    }
    else {
      this.expandedRow = -1;
    }
  }

  redeploy(id: string): void {
    if(id) {
      this.subs.sink = this.vestingService.redeploy(id).subscribe((res) => {
        this.toastr.success('Transaction initiated...');
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
      });
    }
  }

}
