import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyWalletPaymentControlComponent } from './company-wallet-payment-control.component';

describe('CompanyWalletPaymentControlComponent', () => {
  let component: CompanyWalletPaymentControlComponent;
  let fixture: ComponentFixture<CompanyWalletPaymentControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyWalletPaymentControlComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyWalletPaymentControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
