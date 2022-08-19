import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { setShowFullWidth } from 'src/app/store/actions';

@Component({
  selector: 'app-about-us',
  templateUrl: './about-us.component.html',
  styleUrls: ['./about-us.component.scss']
})
export class AboutUsComponent implements OnInit, OnDestroy {

  constructor(private store: Store) { }

  ngOnDestroy(): void {
    this.store.dispatch(setShowFullWidth({showFullWidth: false}));
  }

  ngOnInit(): void {
    this.store.dispatch(setShowFullWidth({showFullWidth: true}));
  }

}
