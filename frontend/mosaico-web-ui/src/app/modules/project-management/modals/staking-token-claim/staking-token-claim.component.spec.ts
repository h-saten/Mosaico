import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StakingTokenClaimComponent } from './staking-token-claim.component';

describe('StakingTokenClaimComponent', () => {
  let component: StakingTokenClaimComponent;
  let fixture: ComponentFixture<StakingTokenClaimComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StakingTokenClaimComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StakingTokenClaimComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
