import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTokenVestingComponent } from './edit-token-vesting.component';

describe('EditTokenVestingComponent', () => {
  let component: EditTokenVestingComponent;
  let fixture: ComponentFixture<EditTokenVestingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditTokenVestingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditTokenVestingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
