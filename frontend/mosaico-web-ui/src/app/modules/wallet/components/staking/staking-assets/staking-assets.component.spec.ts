import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StakingAssetsComponent } from './staking-assets.component';

describe('StakingAssetsComponent', () => {
  let component: StakingAssetsComponent;
  let fixture: ComponentFixture<StakingAssetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StakingAssetsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StakingAssetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
