import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeeWithdrawalComponent } from './fee-withdrawal.component';

describe('FeeWithdrawalComponent', () => {
  let component: FeeWithdrawalComponent;
  let fixture: ComponentFixture<FeeWithdrawalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeeWithdrawalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeeWithdrawalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
