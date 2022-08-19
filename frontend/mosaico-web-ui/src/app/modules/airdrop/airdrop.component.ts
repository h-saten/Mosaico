import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Airdrop, AirdropService } from 'mosaico-project';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { AuthService, ErrorHandlingService } from 'mosaico-base';
import { Store } from '@ngrx/store';
import { selectIsAuthorized } from '../user-management/store/user.selectors';

@Component({
  selector: 'app-airdrop',
  templateUrl: './airdrop.component.html',
  styleUrls: ['./airdrop.component.scss']
})
export class AirdropComponent implements OnInit, OnDestroy {
  airdropSlug: string;
  claimed = false;
  airdrop: Airdrop;
  isAuth = false;

  constructor(private route: ActivatedRoute, private router: Router, private service: AirdropService, private store: Store,
    private toastr: ToastrService, private errorHandler: ErrorHandlingService, public auth: AuthService) { }

  isClaiming$ = new BehaviorSubject<boolean>(false);
  subs = new SubSink();

  ngOnInit(): void {
    this.airdropSlug = this.route.snapshot.paramMap.get('slug');
    if (!this.airdropSlug || this.airdropSlug.length === 0) {
      this.router.navigateByUrl('/error/401');
    }
    this.subs.sink = this.service.getAirdrop(this.airdropSlug).subscribe((res) => {
      this.airdrop = res?.data;
      if (!this.airdrop) {
        this.router.navigateByUrl('/error/401');
      }
    });
    this.subs.sink = this.store.select(selectIsAuthorized).subscribe((res) => {
      this.isAuth = res;
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  claim(): void {
    if(!this.isAuth) {
      this.auth.loginWithRedirect(this.router.url);
    }
    this.isClaiming$.next(true);
    if (this.airdrop) {
      this.service.claim(this.airdrop.id).subscribe((res) => {
        this.toastr.success('You successfully received your tokens');
        this.isClaiming$.next(false);
        this.claimed = true;
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.isClaiming$.next(false); });
    }
  }

}
