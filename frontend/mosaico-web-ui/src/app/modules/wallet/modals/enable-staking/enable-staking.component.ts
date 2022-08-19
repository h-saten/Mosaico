import { Component, OnInit, EventEmitter, Output, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { FormDialogBase, validateForm } from 'mosaico-base';
import { PaymentCurrencyService, Token, TokenService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { selectToken } from '../..';
import { SubSink } from 'subsink';
import { zip } from 'rxjs';

@Component({
  selector: 'app-enable-staking',
  templateUrl: './enable-staking.component.html',
  styleUrls: ['./enable-staking.component.scss']
})
export class EnableStakingComponent extends FormDialogBase implements OnInit, OnDestroy {
  tokens: Token[] = [];
  token: Token;
  subs = new SubSink();
  @Output() onCreated = new EventEmitter<string>();

  constructor(modalService: NgbModal, private store: Store, private toastr: ToastrService, private translateService: TranslateService, private tokenService: TokenService, private paymentCurrencyService: PaymentCurrencyService) { super(modalService); }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  open(): void {
    this.ngOnInit();
    super.open();
  }

  ngOnInit(): void {
    this.createForm();
    this.subs.sink = this.store.select(selectToken).subscribe((t) => {
      if (t) {
        this.token = t;
        this.subs.sink = zip(this.tokenService.getCompanyTokens(t.companyId), this.paymentCurrencyService.getPaymentCurrencies(this.token.network)).subscribe((responses) => {
          this.tokens = responses[0]?.data?.filter((tok) => tok.id !== this.token.id);
          this.tokens.push(...responses[1]?.data?.paymentCurrencies);
        });
      }
    });
  }

  private createForm(): void {
    this.form = new FormGroup({
      tokenId: new FormControl(null, [Validators.required]),
      maxReward: new FormControl(null, [Validators.required, Validators.min(0), Validators.max(99999)]),
      maxParticipants: new FormControl(null, [Validators.required, Validators.min(0)]),
      rewardCycle: new FormControl(null, [Validators.required]),
      cycleSpendingLimit: new FormControl(null, [Validators.required, Validators.min(0)])
    });
  }

  submit(): void {
    if (validateForm(this.form)) {

    }
    else {
      this.toastr.error("Invalid values. Fix them before continuation.");
    }
  }

}
