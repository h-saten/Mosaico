import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StakingPanelComponent } from './staking-panel.component';

describe('StakingPanelComponent', () => {
  let component: StakingPanelComponent;
  let fixture: ComponentFixture<StakingPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StakingPanelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StakingPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
