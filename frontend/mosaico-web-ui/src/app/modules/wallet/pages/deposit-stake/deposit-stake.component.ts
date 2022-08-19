import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute} from "@angular/router";
import {SubSink} from "subsink";
import {WalletService} from "mosaico-wallet";
import {Store} from "@ngrx/store";
import { DepositStakeCommand } from 'mosaico-project';
import { validateForm } from 'mosaico-base';


@Component({
  selector: 'app-deposit-stake',
  templateUrl: './deposit-stake.component.html',
  styleUrls: []
})
export class DepositStakeComponent implements OnInit, OnDestroy {

  form: FormGroup;
  tokenId: string | null;
  sub: SubSink = new SubSink();

  constructor(private route: ActivatedRoute, private walletService: WalletService, private store: Store) {
    this.route.paramMap.subscribe( paramMap => {
      this.tokenId = paramMap.get('token');
    });
  }

  ngOnInit(): void {
    this.createForm();
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  createForm(): void {
    this.form = new FormGroup({
      quantity: new FormControl(null, [Validators.min(0.1)]),
    });
  }

  // save() {

  //   if (validateForm(this.form)) {
  //     const command: DepositStakeCommand = {
  //       tokenId: this.tokenId,
  //       quantity: this.form.controls.quantity.value,
  //     } as DepositStakeCommand;

  //     this.sub.sink = this.walletService
  //       .depositStake(command)
  //       .subscribe();

  //   } else {
  //     console.error("Invalid form data");
  //   }
  // }
}
