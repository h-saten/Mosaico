import { Observable } from 'rxjs';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectUserIsVerified } from '../../store';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
})
export class ProfileComponent implements OnInit, OnDestroy {

  isVerified$: Observable<boolean> | undefined;

  constructor(
    private store: Store,
  ) {}

  ngOnDestroy(): void {
  }

  ngOnInit(): void {

    this.isVerified$ = this.store.select(selectUserIsVerified);
  }

}
