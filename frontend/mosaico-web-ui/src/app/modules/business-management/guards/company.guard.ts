import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { COMPANY_PERMISSIONS } from '../constants';
import { setCompanyPermissions } from '../store';
import { CompanyService } from 'mosaico-dao';

@Injectable({
  providedIn: 'root'
})
export class CompanyGuard implements CanActivate {
  constructor(private projectService: CompanyService, private store: Store, private router: Router, private toastr: ToastrService) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return new Promise<boolean>((resolve, reject) => {
      const id: string | null =  route.paramMap.get('id');
      this.projectService.getCompanyPermissions(id).subscribe((response) => {
        if (response) {
          if (response.data[COMPANY_PERMISSIONS.CAN_READ] === true) {
            this.store.dispatch(setCompanyPermissions(response.data));
            resolve(true);
          }
          else {
            this.toastr.error("You don't have permission to see this page");
            this.router.navigateByUrl('/');
            reject();
          }

        }
        else {
          reject();
        }
      }, (error) => {
        this.router.navigateByUrl('/');
        reject();
      });
    });
  }
}
