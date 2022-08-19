import { Injectable, Inject, EventEmitter } from '@angular/core';
import { Role } from '../models';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { RoleFactory } from '../core/role-factory';
import * as _ from 'lodash';

@Injectable()
export class RoleService {
  private roles: Array<Role> = [];
  public loaded: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public onChanges: BehaviorSubject<any> = new BehaviorSubject(null);

  constructor(@Inject(RoleFactory) private roleFactory: Observable<string[]>) {

  }

  public initRoles(roles?: string[]): Observable<boolean> {
    if (this.roleFactory && !this.loaded.value) {
      return new Observable((subscriber) => {
        this.roleFactory.subscribe((res) => {
          this.roles = res.map((roleName) => {
            return {
              name: roleName,
              key: roleName
            };
          });
          this.loaded.next(true);
          subscriber.next(true);
          subscriber.complete();
        });
      });
    }
    else {
      return of(true);
    }
  }

  public addRole(param: Role | Array<Role> | string, markAsInitialized: boolean = false): void {
    if (param instanceof Role) {
      this.addOneRole(param);
    }
    else if (Array.isArray(param)) {
      this.addManyRoles(param);
    }
    else if (typeof param === 'string') {
      this.addOneRole({
        name: param,
        key: param
      });
    }
    else {
      throw new Error('Invalid role type');
    }
    if (markAsInitialized){
      this.loaded.next(true);
    }
    this.onChanges.next(this.roles);
  }

  private addOneRole(role: Role | string): void {
    if(typeof role === 'string'){
      role = new Role(role);
    }
    const roleExists: boolean = _.findIndex(this.roles, (r: Role) => r.name === (<Role>role).name) >= 0;
    if (roleExists){
      return;
    }
    this.roles.push(role);
  }

  private addManyRoles(roles: Array<Role> | Array<string>): void {
    roles.forEach((r: any) => {
      this.addOneRole(r);
    });
  }

  public isInRole(name: string | Array<string>): boolean {
    if (typeof name === 'string'){
      return _.findIndex(this.roles, (r: Role) => r.name === name) >= 0;
    }
    else if (name instanceof Array || Array.isArray(name)) {
      if (name.length === 0){
        return true;
      }
      return this.roles.some((value) => {
        return name.includes(<string>value.name);
      });
    }
    return false;
  }

  public removeRole(name: string): void {
    this.roles = _.remove(this.roles, (r: Role) => r.name === name);
    this.onChanges.next(this.roles);
  }

  public removeAll(): void {
    this.roles = [];
    this.onChanges.next(this.roles);
  }

  get allRoles(): Role[] {
    return this.roles;
  }
}
