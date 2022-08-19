import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletOverviewStakingComponent } from './wallet-overview-staking.component';

describe('WalletOverviewStakingComponent', () => {
  let component: WalletOverviewStakingComponent;
  let fixture: ComponentFixture<WalletOverviewStakingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletOverviewStakingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletOverviewStakingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
