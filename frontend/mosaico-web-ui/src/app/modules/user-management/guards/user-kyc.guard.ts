import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { UserService } from '../services/user.service';
import { setUserProfilePermissions } from '../store/user.actions';
import { SuccessResponse } from '../../../../../projects/mosaico-base/src/lib/utils/success-response';
import { GetUserProfilePermissionsResponse } from '../models';
import { USER_PERMISSIONS } from 'mosaico-base';

@Injectable({
  providedIn: 'root'
})
export class UserKycGuard implements CanActivate, CanLoad {

  constructor(private userService: UserService, private store: Store) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.userService.getProfilePermissions().pipe(tap((p: SuccessResponse<GetUserProfilePermissionsResponse>) => this.store.dispatch(setUserProfilePermissions({perm: p?.data}))),
        map((t) => t && t.data && t.data[USER_PERMISSIONS.CAN_VERIFY_ACCOUNT] === true));
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.userService.getProfilePermissions().pipe(tap((p: SuccessResponse<GetUserProfilePermissionsResponse>) => this.store.dispatch(setUserProfilePermissions({perm: p?.data}))),
        map((t) => t && t.data && t.data[USER_PERMISSIONS.CAN_VERIFY_ACCOUNT] === true));
  }
}
