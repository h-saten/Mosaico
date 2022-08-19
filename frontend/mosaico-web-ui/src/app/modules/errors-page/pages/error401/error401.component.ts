import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-error401',
  templateUrl: './error401.component.html',
  styleUrls: ['./error401.component.scss']
})
export class Error401Component implements OnInit {

  constructor(
    private location: Location
  ) { }

  ngOnInit(): void {
  }

  backClicked() {
    // const navigationId: any = this.location.getState();
    // if (navigationId.navigationId === 1) {}
    this.location.back();
  }

}
