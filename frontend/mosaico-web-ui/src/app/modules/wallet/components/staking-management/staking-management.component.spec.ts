import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StakingManagementComponent } from './staking-management.component';

describe('StakingManagementComponent', () => {
  let component: StakingManagementComponent;
  let fixture: ComponentFixture<StakingManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StakingManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StakingManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
