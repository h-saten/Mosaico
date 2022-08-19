// import { ViewportScroller } from '@angular/common';
import { AfterViewInit, Component, OnInit } from '@angular/core';
// import { ActivatedRoute, Data } from '@angular/router';

@Component({
  selector: 'app-project-administration',
  templateUrl: './project-administration.component.html',
  styleUrls: ['./project-administration.component.scss']
})
export class ProjectAdministrationComponent implements OnInit {

  // For DIMA :)
  // I am leaving this code as it will probably need to be copied to use in other tabs - to scroll up to the menu after refreshing the page

  // private fragmentTarget = '';

  // currentPath = '';

  constructor(
    // private activatedRoute: ActivatedRoute,
    // private viewportScroller: ViewportScroller
  ) {
    // this.activatedRoute.fragment.subscribe(fragment => {
    //   if (fragment) {
    //     this.fragmentTarget = fragment;
    //   }
    // });
  }

  ngOnInit(): void {
    // this.getPathFromRouting();
  }

  // ngAfterViewInit(): void {
  //   if (this.fragmentTarget) {
  //     setTimeout(() => {
  //       this.goScrollUp(this.fragmentTarget);
  //     }, 1000);
  //   }
  // }

  // private goScrollUp(target: string): void {
  //   this.viewportScroller.scrollToAnchor(target);
  // }

  // private getPathFromRouting(): void {
  //   this.activatedRoute.data.subscribe((response: Data) => {
  //     console.log('response: ', response);
  //     if (response) {
  //       this.currentPath = response['path'];
  //     }
  //   });
  // }

}
