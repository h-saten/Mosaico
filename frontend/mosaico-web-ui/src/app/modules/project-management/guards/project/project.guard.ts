import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { setCurrentProjectPermissions } from '../../store/project.actions';
import { PERMISSIONS } from '../../constants';
import { ToastrService } from 'ngx-toastr';
import { ProjectService } from 'mosaico-project';

@Injectable({
  providedIn: 'root'
})
export class ProjectGuard implements CanActivate {
  constructor(private projectService: ProjectService, private store: Store, private router: Router, private toastr: ToastrService) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return new Promise<boolean>((resolve, reject) => {
      this.projectService.getProjectPermissions(route.paramMap.get('projectId')).subscribe((response) => {
        if (response) {
          if (response.data[PERMISSIONS.CAN_READ] === true) {
            this.store.dispatch(setCurrentProjectPermissions(response.data));
            resolve(true);
          }
          else {
            this.toastr.error("You don't have permission to see this page");
            this.router.navigateByUrl('/projects');
            reject();
          }

        }
        else {
          reject();
        }
      }, (error) => {
        this.router.navigateByUrl('/projects');
        reject();
      });
    });
  }
}
