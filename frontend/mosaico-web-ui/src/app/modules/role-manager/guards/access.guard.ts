import { Injectable, Inject } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, CanLoad, Router, Route } from '@angular/router';
import { RoleService } from '../services';
import { BehaviorType } from '../core';
import { AccessGuardConfig } from '../models';
import { tap, map, filter, take } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable()
export class AccessGuard implements CanActivate, CanLoad {
  constructor(private roleService: RoleService, private router: Router, @Inject(BehaviorType) private behaviorType: boolean) {
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    try {
      let guardCfg = next.data["roleGuard"] as AccessGuardConfig;
      if (guardCfg && guardCfg.roles && guardCfg.roles.length > 0) {
        return true;
      }
      return this.behaviorType;
    }
    catch (ex) {
      return false;
    }
  }

  canLoad(route: Route): Observable<boolean> | Promise<boolean> | boolean {
    try {
      if(route && route.data) {
        let guardCfg = route.data["roleGuard"] as AccessGuardConfig;
        if (guardCfg && guardCfg.roles && guardCfg.roles.length > 0) {
          return true;
        }
        return this.behaviorType;
      }
      return false;
    }
    catch (ex) {
      return false;
    }
  }

  // private verifyAccess(config: AccessGuardConfig): Observable<boolean> | boolean {
  //   return this.auth.user$.pipe(
  //     filter((u) => !!u),
  //     map((u) => this.verifyRoles(config)),
  //     take(1)
  //   );
  // }

  private verifyRoles(config:AccessGuardConfig):boolean{
    const canActivate = this.roleService.isInRole(config.roles);
    if (!canActivate) {
      if(config.redirectUrl && config.redirectUrl.length > 0){
        this.router.navigate([config.redirectUrl]);
      }
    }
    return canActivate;
  }
}
