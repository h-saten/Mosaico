import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-locked-out',
  templateUrl: './locked-out.component.html',
  styleUrls: ['./locked-out.component.scss']
})
export class LockedOutComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
  ) {
  }

  ngOnInit() {

    //this.spinner.hide();


  }

}
