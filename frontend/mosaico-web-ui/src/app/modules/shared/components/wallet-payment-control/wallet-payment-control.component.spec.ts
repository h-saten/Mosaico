import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletPaymentControlComponent } from './wallet-payment-control.component';

describe('WalletPaymentControlComponent', () => {
  let component: WalletPaymentControlComponent;
  let fixture: ComponentFixture<WalletPaymentControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletPaymentControlComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletPaymentControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
