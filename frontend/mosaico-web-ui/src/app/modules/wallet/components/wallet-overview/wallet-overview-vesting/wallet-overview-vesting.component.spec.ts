import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletOverviewVestingComponent } from './wallet-overview-vesting.component';

describe('WalletOverviewVestingComponent', () => {
  let component: WalletOverviewVestingComponent;
  let fixture: ComponentFixture<WalletOverviewVestingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletOverviewVestingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletOverviewVestingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
