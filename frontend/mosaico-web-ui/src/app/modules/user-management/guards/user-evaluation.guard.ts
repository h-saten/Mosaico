import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { selectUserInformation } from '../store/user.selectors';

@Injectable({
  providedIn: 'root'
})
export class UserEvaluationGuard implements CanActivate, CanLoad {

  constructor(private store: Store, private router: Router) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.store.select(selectUserInformation).pipe(map((t) => {
        if(t?.id && !t.evaluationCompleted) {
            this.router.navigateByUrl('/user/evaluation');
            return false;
        }
        return true;
    }));
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.store.select(selectUserInformation).pipe(map((t) => {
        if(t?.id && !t.evaluationCompleted) {
            this.router.navigateByUrl('/user/evaluation');
            return false;
        }
        return true;
    }));
  }
}
