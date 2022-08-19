import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-staking-token-claim',
  templateUrl: './staking-token-claim.component.html',
  styleUrls: ['./staking-token-claim.component.scss']
})
export class StakingTokenClaimComponent implements OnInit {
  isClaimed: boolean = false;
  claimIsConfirmed: boolean = false;

  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit(): void {
  }

  onClaime() {
    this.isClaimed = true;
  }

  goBack() {
    this.isClaimed = false;
  }

  onConfirm() {
    this.isClaimed = false;
    this.claimIsConfirmed = true;
  }

}
