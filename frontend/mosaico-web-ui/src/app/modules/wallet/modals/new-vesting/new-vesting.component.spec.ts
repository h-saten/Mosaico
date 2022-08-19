import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewVestingComponent } from './new-vesting.component';

describe('NewVestingComponent', () => {
  let component: NewVestingComponent;
  let fixture: ComponentFixture<NewVestingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewVestingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewVestingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
