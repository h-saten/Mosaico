import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MenuProjectsService {

  private showDropDownMenu = false;

  constructor() { }

  setShowDropDownMenu(show: boolean): void {
    this.showDropDownMenu = show;
  }

  getShowDropDownMenu(): boolean {
    return this.showDropDownMenu;
  }


}
