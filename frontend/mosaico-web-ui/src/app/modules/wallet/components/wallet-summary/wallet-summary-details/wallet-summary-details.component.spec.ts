import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletSummaryDetailsComponent } from './wallet-summary-details.component';

describe('WalletSummaryDetailsComponent', () => {
  let component: WalletSummaryDetailsComponent;
  let fixture: ComponentFixture<WalletSummaryDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletSummaryDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletSummaryDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
