import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyWalletSummaryComponent } from './company-wallet-summary.component';

describe('CompanyWalletSummaryComponent', () => {
  let component: CompanyWalletSummaryComponent;
  let fixture: ComponentFixture<CompanyWalletSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyWalletSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyWalletSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
