import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StakingPanelActiveComponent } from './staking-panel-active.component';

describe('StakingPanelActiveComponent', () => {
  let component: StakingPanelActiveComponent;
  let fixture: ComponentFixture<StakingPanelActiveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StakingPanelActiveComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StakingPanelActiveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
