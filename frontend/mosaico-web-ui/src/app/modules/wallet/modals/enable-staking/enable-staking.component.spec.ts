import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnableStakingComponent } from './enable-staking.component';

describe('EnableStakingComponent', () => {
  let component: EnableStakingComponent;
  let fixture: ComponentFixture<EnableStakingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EnableStakingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EnableStakingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
