import { Store } from '@ngrx/store';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { selectIsAuthorized, selectUserIsVerified } from 'src/app/modules/user-management/store';

@Component({
    selector: 'app-kyc-alert',
    templateUrl: './kyc-alert.component.html',
})
export class KycAlertComponent implements OnInit {
    isVerified$: Observable<boolean>;
    isAuthorized$: Observable<boolean>;
    isKycPage: boolean = false;

    constructor(private store: Store, private router: Router) {
        this.isVerified$ = this.store.select(selectUserIsVerified);
        this.isAuthorized$ = this.store.select(selectIsAuthorized);
    }
    ngOnInit(): void {
        this.router.events.pipe(filter(ev => (ev instanceof NavigationEnd))).subscribe((res) => {
            if (res && res as NavigationEnd) {
                this.isKycPage = (<NavigationEnd>res).url.includes('kyc');
            }
        });
    }
}