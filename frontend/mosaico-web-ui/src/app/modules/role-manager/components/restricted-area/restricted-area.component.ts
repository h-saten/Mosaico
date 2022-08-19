import { Component, OnInit, OnChanges, Input, ContentChild, TemplateRef } from '@angular/core';
import { RoleService } from '../../services';

@Component({
  selector: 'lib-restricted-area, [restricted-area]',
  templateUrl: './restricted-area.component.html',
  styleUrls: ['./restricted-area.component.scss']
})
export class RestrictedAreaComponent implements OnInit, OnChanges {
  //will peak first ng-template in view
  @ContentChild(TemplateRef) content: any;
  show = false;
  @Input() only?: string | Array<string>;
  @Input() customShow?: boolean;
  
  constructor(private roleService: RoleService) { }

  ngOnInit(): void {
    this.roleService.onChanges.subscribe((newRoles) => {
      this.evaluate();
    });
  }

  ngOnChanges(): void {
    this.evaluate();
  }

  private evaluate(): void {
    if (this.only) {
      this.show = this.roleService.isInRole(this.only);
    }
    else {
      this.show = <boolean>this.customShow;
    }
  }

}
