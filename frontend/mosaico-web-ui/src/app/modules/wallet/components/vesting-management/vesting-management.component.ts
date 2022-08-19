import { Component, OnInit, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { PersonalVestingManagementComponent } from '../personal-vesting-management/personal-vesting-management.component';
import { PrivateSaleVestingManagementComponent } from '../private-sale-vesting-management/private-sale-vesting-management.component';

@Component({
  selector: 'app-vesting-management',
  templateUrl: './vesting-management.component.html',
  styleUrls: ['./vesting-management.component.scss']
})
export class VestingManagementComponent implements OnInit {
  subs = new SubSink();

  constructor() { }

  ngOnInit(): void {

  }

 

}
