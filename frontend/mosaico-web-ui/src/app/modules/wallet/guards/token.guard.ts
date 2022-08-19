import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { TokenService, TOKEN_PERMISSIONS } from 'mosaico-wallet';
import { Store } from '@ngrx/store';
import { ToastrService } from "ngx-toastr";
import { Observable } from "rxjs";
import { setTokenPermissions } from '../store/wallet.actions';
import { selectProjectPreviewToken } from "../../project-management/store";

@Injectable({
    providedIn: 'root'
})
export class TokenGuard implements CanActivate {
    constructor(private tokenService: TokenService, private store: Store, private router: Router, private toastr: ToastrService) {

    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        return new Promise<boolean>((resolve, reject) => {
            const tokenId = route.paramMap.get('tokenId');
            if(tokenId && tokenId.length > 0){
                this.verifyToken(tokenId, resolve, reject);
            }
            else {
                this.store.select(selectProjectPreviewToken).subscribe((res) => {
                    if(res) {
                        this.verifyToken(res.id, resolve, reject);
                    }
                    else {
                        reject();
                    }
                });
            }
        });
    }

    private verifyToken(tokenId, resolve, reject): void {
        this.tokenService.getTokenPermissions(tokenId).subscribe((response) => {
            if (response) {
                if (response.data[TOKEN_PERMISSIONS.CAN_READ] === true) {
                    this.store.dispatch(setTokenPermissions(response.data));
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
    }
}