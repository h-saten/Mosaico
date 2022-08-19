import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StakingPanelHistoryComponent } from './staking-panel-history.component';

describe('StakingPanelHistoryComponent', () => {
  let component: StakingPanelHistoryComponent;
  let fixture: ComponentFixture<StakingPanelHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StakingPanelHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StakingPanelHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
