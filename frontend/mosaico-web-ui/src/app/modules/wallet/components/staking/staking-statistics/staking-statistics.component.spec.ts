import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StakingStatisticsComponent } from './staking-statistics.component';

describe('StakingStatisticsComponent', () => {
  let component: StakingStatisticsComponent;
  let fixture: ComponentFixture<StakingStatisticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StakingStatisticsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StakingStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
