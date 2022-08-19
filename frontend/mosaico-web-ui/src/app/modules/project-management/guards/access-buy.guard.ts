import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { ProjectService } from 'mosaico-project';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PERMISSIONS } from '../constants';
import { selectPreviewProjectPermissions } from '../store';

@Injectable({
  providedIn: 'root'
})
export class AccessBuyGuard implements CanActivate, CanLoad {
  constructor(private store: Store, private router: Router) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.store.select(selectPreviewProjectPermissions).pipe(map((permission) =>  {
      const canPurchase = permission && permission[PERMISSIONS.CAN_PURCHASE] === true;
      if(!canPurchase) {
        this.router.navigate(['/']);
      }
      return canPurchase;
    }));
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.store.select(selectPreviewProjectPermissions).pipe(map((permission) =>  {
      const canPurchase = permission && permission[PERMISSIONS.CAN_PURCHASE] === true;
      if(!canPurchase) {
        this.router.navigate(['/']);
      }
      return canPurchase;
    }));
  }
}
