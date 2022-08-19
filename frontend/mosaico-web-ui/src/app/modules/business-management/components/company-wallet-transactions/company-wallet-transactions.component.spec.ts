import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyWalletTransactionsComponent } from './company-wallet-transactions.component';

describe('CompanyWalletTransactionsComponent', () => {
  let component: CompanyWalletTransactionsComponent;
  let fixture: ComponentFixture<CompanyWalletTransactionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyWalletTransactionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyWalletTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
