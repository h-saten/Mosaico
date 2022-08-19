import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionFeeManagerComponent } from './transaction-fee-manager.component';

describe('TransactionFeeManagerComponent', () => {
  let component: TransactionFeeManagerComponent;
  let fixture: ComponentFixture<TransactionFeeManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TransactionFeeManagerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransactionFeeManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
