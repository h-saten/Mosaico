import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { DEFAULT_MODAL_OPTIONS } from 'mosaico-base';
import { VestingWallet, WalletService } from 'mosaico-wallet';
import { take, zip } from 'rxjs';
import { ActionSuccessComponent } from 'src/app/modules/project-management/modals';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { selectBlockchain } from 'src/app/store/selectors';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-vesting',
  templateUrl: './vesting.component.html',
  styleUrls: ['./vesting.component.scss']
})
export class VestingComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  vestings: VestingWallet[] = [];

  constructor(private modalService: NgbModal, private walletService: WalletService, private store: Store,
    private translate: TranslateService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = zip(this.store.select(selectUserInformation).pipe(take(1)), this.store.select(selectBlockchain).pipe(take(1))).subscribe((responses) => {
      if(responses && responses.length === 2) {
        this.subs.sink = this.walletService.getWalletVesting(responses[0]?.id, responses[1]).subscribe((res) => {
          this.vestings = res?.data?.items;
        });
      }
    });
  }

  onClaim() {
    const modalRef = this.modalService.open(ActionSuccessComponent, DEFAULT_MODAL_OPTIONS);
    modalRef.componentInstance.successText = `${this.translate.instant("WALLET_VESTING.SUCCESS.TITLE")} 12 ${this.translate.instant("WALLET_VESTING.SUCCESS.TOKENS")}`;
    modalRef.componentInstance.detailsText = this.translate.instant("WALLET_VESTING.SUCCESS.DESCRIPTION");
  }

}
