import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { setShowFullWidth } from 'src/app/store/actions';

@Component({
  // selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent implements OnInit, OnDestroy {

  constructor(private store: Store) { }

  ngOnDestroy(): void {
    this.store.dispatch(setShowFullWidth({showFullWidth: false}));
  }

  ngOnInit(): void {
    this.store.dispatch(setShowFullWidth({showFullWidth: true}));
  }

}
