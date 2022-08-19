import { Component, HostBinding, OnInit } from '@angular/core';

@Component({
  selector: 'app-dropdown-menu-users-list',
  templateUrl: './dropdown-menu-users-list.component.html'
})
export class DropdownMenuUsersListComponent implements OnInit {

  @HostBinding('class') class =
    'menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold w-200px';
  @HostBinding('attr.data-kt-menu') dataKtMenu = 'true';

  constructor() { }

  ngOnInit(): void {
  }

}
