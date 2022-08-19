import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { PrivateSaleVesting, Token, TOKEN_PERMISSIONS, VestingService } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { selectToken, selectTokenPermissions } from '../../store';
import { BehaviorSubject } from 'rxjs';
import { ErrorHandlingService } from 'mosaico-base';

@Component({
  selector: 'app-private-sale-vesting-management',
  templateUrl: './private-sale-vesting-management.component.html',
  styleUrls: ['./private-sale-vesting-management.component.scss']
})
export class PrivateSaleVestingManagementComponent implements OnInit, OnDestroy {

  subs = new SubSink();
  canEdit = false;
  isDataLoaded = false;
  vestings: PrivateSaleVesting[] = [];
  token: Token;
  isLoading$ = new BehaviorSubject<boolean>(false);

  constructor(private store: Store, private vestingService: VestingService, private errorHandler: ErrorHandlingService) { }

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

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  loadData(force = false): void {
    if((!this.isDataLoaded || force === true) && !this.isLoading$.value) {
      this.isLoading$.next(true);
      this.subs.sink = this.vestingService.getPrivateSaleVesting(this.token.id).subscribe((res) => {
        this.vestings = res?.data;
        this.isLoading$.next(false);
        this.isDataLoaded = true;
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        this.isLoading$.next(false);
        this.isDataLoaded = false;
      });
    }
  }

}
