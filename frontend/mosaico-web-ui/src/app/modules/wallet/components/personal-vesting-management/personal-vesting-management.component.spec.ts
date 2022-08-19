import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalVestingManagementComponent } from './personal-vesting-management.component';

describe('PersonalVestingManagementComponent', () => {
  let component: PersonalVestingManagementComponent;
  let fixture: ComponentFixture<PersonalVestingManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonalVestingManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonalVestingManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
