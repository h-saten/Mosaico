import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivateSaleVestingManagementComponent } from './private-sale-vesting-management.component';

describe('PrivateSaleVestingManagementComponent', () => {
  let component: PrivateSaleVestingManagementComponent;
  let fixture: ComponentFixture<PrivateSaleVestingManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrivateSaleVestingManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivateSaleVestingManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
