import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, Observable } from 'rxjs';
import { setCurrentProjectPermissions } from '../../store/project.actions';
import { PERMISSIONS } from '../../constants';
import { ToastrService } from 'ngx-toastr';
import { ProjectService } from 'mosaico-project';
import { selectPreviewProjectPermissions } from '../../store';

@Injectable({
  providedIn: 'root'
})
export class ProjectEditGuard implements CanActivate, CanLoad {
  constructor(private store: Store, private router: Router, private toastr: ToastrService) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.store.select(selectPreviewProjectPermissions).pipe(map((permission) => {
      const canEdit = permission && permission[PERMISSIONS.CAN_VIEW_DASHBOARD] === true;
      if (!canEdit) {
        this.toastr.error("You do not have permissions");
        this.router.navigate(['/']);
      }
      return canEdit;
    }));
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.store.select(selectPreviewProjectPermissions).pipe(map((permission) => {
      const canEdit = permission && permission[PERMISSIONS.CAN_VIEW_DASHBOARD] === true;
      if (!canEdit) {
        this.toastr.error("You do not have permissions");
        this.router.navigate(['/']);
      }
      return canEdit;
    }));
  }
}
