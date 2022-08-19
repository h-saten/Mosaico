import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DEFAULT_MODAL_OPTIONS } from 'mosaico-base';
import { StakingService, WalletStake } from 'mosaico-wallet';
import { StakingTokenClaimComponent } from 'src/app/modules/project-management/modals';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-staking-panel-active',
  templateUrl: './staking-panel-active.component.html',
  styleUrls: ['./staking-panel-active.component.scss']
})
export class StakingPanelActiveComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  items: WalletStake[] = [];
  hoursInADay = 24;
  minutesInAnHour = 60;
  secondsInAMinute = 60;

  constructor(private modalService: NgbModal, private stakingService: StakingService) { }
  
  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.stakingService.getStakes().subscribe((res) => {
      this.items = res?.data?.stakings;
    });
  }

  openClaimModal(): void {
    const modalRef = this.modalService.open(StakingTokenClaimComponent, DEFAULT_MODAL_OPTIONS);
  }

  getMinutesLeft(stake: WalletStake): number {
    if(stake && stake.nextRewardAt && stake.nextRewardAt.length > 0) {
      let timeDifference = new Date(stake.nextRewardAt).getTime() - new Date().getTime();
      if (timeDifference > 0) {
        timeDifference = timeDifference / 1000;
        return Math.floor((timeDifference) / (this.minutesInAnHour) % this.secondsInAMinute);;
      }
    }
    return 0;
  }

  getHoursLeft(stake: WalletStake): number {
    if(stake && stake.nextRewardAt && stake.nextRewardAt.length > 0) {
      let timeDifference = new Date(stake.nextRewardAt).getTime() - new Date().getTime();
      if (timeDifference > 0) {
        timeDifference = timeDifference / 1000;
        return Math.floor((timeDifference) / (this.minutesInAnHour * this.secondsInAMinute) % this.hoursInADay);
      }
    }
    return 0;
  }

  getDaysLeft(stake: WalletStake): number {
    if(stake && stake.nextRewardAt && stake.nextRewardAt.length > 0) {
      let timeDifference = new Date(stake.nextRewardAt).getTime() - new Date().getTime();
      if (timeDifference > 0) {
        timeDifference = timeDifference / 1000;
        return Math.floor((timeDifference) / (this.minutesInAnHour * this.secondsInAMinute * this.hoursInADay));
      }
    }
    return 0;
  }

  getSecondsLeft(stake: WalletStake): number {
    if(stake && stake.nextRewardAt && stake.nextRewardAt.length > 0) {
      let timeDifference = new Date(stake.nextRewardAt).getTime() - new Date().getTime();
      if (timeDifference > 0) {
        timeDifference = timeDifference / 1000;
        return Math.floor((timeDifference) % this.secondsInAMinute);;
      }
    }
    return 0;
  }
}
