import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { AuthService } from 'mosaico-base';
import { Observable } from 'rxjs';
import { RoleService } from '../modules/role-manager/services/role.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate, CanLoad {
  constructor(private authService: AuthService, private router: Router, private roleService: RoleService) { }

  canLoad(route: Route, segments: UrlSegment[]): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    if (this.authService.isAuthenticated()) {
      const roles = route?.data?.roles;
      if(roles && !this.roleService.isInRole(roles)) {
        this.router.navigateByUrl('/error/404');
      }
      return true;
    }
    else {
      this.authService.loginWithRedirect(window.location.pathname);
      return false;
    }
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | boolean {
    if (this.authService.isAuthenticated()) {
      return true;
    }
    else {
      this.authService.loginWithRedirect(window.location.pathname);
      return false;
    }
  }
}