import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { PaymentCurrency, StakeCommand, StakingPair, StakingService, Token, TokenBalance, WalletBalance } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { selectWalletTokenBalance } from '../../../store/wallet.selectors';

export interface StakableAsset {
  symbol: string;
  logoUrl: string;
  id: string;
}

@Component({
  selector: 'app-staking-assets',
  templateUrl: './staking-assets.component.html',
  styleUrls: ['./staking-assets.component.scss']
})
export class StakingAssetsComponent implements OnInit, OnDestroy {
  assetsForm: FormGroup;
  subs = new SubSink();
  stakingParis: StakingPair[] = [];
  stakableTokens: StakableAsset[] = [];
  pairs: StakableAsset[] = [];
  balances: TokenBalance[] = [];
  selectedAssetBalance?: number = null;
  currentPair: StakingPair;
  stakingDisabled = true;
  interval: NodeJS.Timer;
  currentAssetTicker: string;

  constructor(
    private stakingService: StakingService,
    private formBuilder: FormBuilder,
    private store: Store
  ) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    clearInterval(this.interval);
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectWalletTokenBalance).subscribe((res) => {
      this.balances = res?.tokens;
    });
    this.interval = setInterval(() => {
      const assetId = this.assetsForm.get('assetId')?.value;
      if(assetId && assetId.length > 0) {
        this.selectedAssetBalance = this.balances?.find((t) => t.id === assetId)?.balance;
      }
      else{
        this.selectedAssetBalance = null;
      }
    }, 500);
    this.assetsForm = this.formBuilder.group({
      assetId: new FormControl(),
      balance: new FormControl(null),
      days: new FormControl(''),
      pairId: new FormControl('')
    });
    this.assetsForm.get('days').disable();
    this.assetsForm.get('pairId').disable();
    this.assetsForm.get('balance').disable();

    this.subs.sink =  this.assetsForm.get('assetId').valueChanges.subscribe((res) => {
      this.assetsForm.get('pairId').setValue(null);
      this.assetsForm.get('balance').setValue(null);
      this.currentAssetTicker = null;

      if(res && res.length > 0) {
        const stakingPairs = this.stakingParis.filter((p) => p.type === 'Token' ? p.stakingToken?.id === res : p.stakingPaymentCurrency?.id === res);
        if(stakingPairs && stakingPairs.length > 0){
          this.pairs = stakingPairs?.map((p) => p.token);
          const pair = stakingPairs[0];
          if(pair) {
            this.currentAssetTicker = pair.type === 'Token' ? pair.stakingToken?.symbol : pair.stakingPaymentCurrency?.ticker;
          }
        }
        this.assetsForm.get('pairId').enable();
        this.assetsForm.get('balance').enable();
      }
      else{
        this.assetsForm.get('balance').disable();
        this.assetsForm.get('pairId').disable();
        this.pairs = [];
      }
    });
    this.subs.sink = this.assetsForm.get('pairId').valueChanges.subscribe((res) => {
      if(res && res.length > 0) {
        this.currentPair = this.stakingParis.find((p) => {
          const stakingAssetId = this.assetsForm.get('assetId').value;
          return p.type === 'Token' ? p.token.id === res &&  p.stakingToken?.id === stakingAssetId : p.token.id === res && p.stakingPaymentCurrency?.id === stakingAssetId;
        });
        const balance = this.assetsForm.get('balance').value;
        if(balance > 0) {
          this.stakingDisabled = false;
        }
        else {
          this.stakingDisabled = true;
        }
      }
      else {
        this.currentPair = null;
      }
    });
    this.subs.sink = this.assetsForm.get('balance').valueChanges.subscribe((res) => {
      if(res > 0 && this.currentPair) {
        this.stakingDisabled = false;
      }
      else{
        this.stakingDisabled = true;
      }
    });
    this.refresh();
  }

  refresh(): void {
    this.subs.sink = this.stakingService.getPairs().subscribe((res) => {
      this.stakingParis = res?.data;
      if(this.stakingParis) {
        this.stakableTokens = this.stakingParis.map((s) => {
          if(s.type === 'Token') {
            return {
              symbol: s.stakingToken?.symbol,
              id: s.stakingToken?.id,
              logoUrl: s.stakingToken?.logoUrl
            };
          }
          else {
            return {
              symbol: s.stakingPaymentCurrency?.ticker,
              id: s.stakingPaymentCurrency?.id,
              logoUrl: s.stakingPaymentCurrency?.logoUrl
            };
          }
        });
      }
    });
  }

  getStakeCommand(): StakeCommand {
     return {
       stakingPairId: this.currentPair?.id,
       balance: +this.assetsForm.get('balance')?.value,
       days: +this.assetsForm.get('days')?.value
     };
  }

  setMaxBalance(): void {
    if(this.selectedAssetBalance) {
      this.assetsForm.get('balance').setValue(this.selectedAssetBalance);
    }
  }

}
