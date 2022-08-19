import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from "subsink";
import { selectProjectPreview } from "../../../store";
import { Store } from "@ngrx/store";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { ProjectCertificateService } from "mosaico-project";
import { WalletService } from "mosaico-wallet";
import { selectUserInformation } from "../../../../user-management/store";
import { combineLatest } from "rxjs";
import { finalize, map } from "rxjs/operators";
import { selectProjectPreviewToken } from '../../../store/project.selectors';

@Component({
  selector: 'app-right-site',
  templateUrl: './right-side.component.html',
  styleUrls: ['./right-side.component.scss']
})
export class RightSideComponent implements OnInit, OnDestroy {

  @Input() variant: string;

  private subs: SubSink = new SubSink();

  canDownloadCertificate = false;
  userBalanceFetched = false;
  userBalanceFetchInProgress = false;

  constructor(
    private store: Store,
    private translateService: TranslateService,
    private toastr: ToastrService,
    private projectCertificateService: ProjectCertificateService,
    private walletService: WalletService
  ) { }

  ngOnInit(): void {
    this.subs.sink = combineLatest([this.store.select(selectProjectPreview), this.store.select(selectUserInformation), this.store.select(selectProjectPreviewToken)]).pipe(
      map(([selectProject$, selectUser$, selectToken$]) => ({
        selectProject: selectProject$,
        selectUser: selectUser$,
        selectToken: selectToken$
      }))
    ).subscribe(response => {
      if (response && response.selectProject && response.selectUser && response.selectToken) {
        this.userTokenBalance(response.selectToken.id);
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private userTokenBalance(tokenId: string): void {
    if (tokenId && tokenId.length > 0) {
      if (this.userBalanceFetched || this.userBalanceFetchInProgress) return;
      this.userBalanceFetchInProgress = true;
      this.subs.sink = this.walletService
        .getUserTokenBalance(tokenId)
        .pipe(finalize(() => {
          this.userBalanceFetched = true;
        }))
        .subscribe(response => {
          if (response) {
            this.canDownloadCertificate = response.data.confirmedTransactionAmount > 0;
          }
        });
    }
  }
}
