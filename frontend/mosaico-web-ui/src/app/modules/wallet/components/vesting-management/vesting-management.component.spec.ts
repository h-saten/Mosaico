import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VestingManagementComponent } from './vesting-management.component';

describe('VestingManagementComponent', () => {
  let component: VestingManagementComponent;
  let fixture: ComponentFixture<VestingManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VestingManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VestingManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
