import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletSummaryChartComponent } from './wallet-summary-chart.component';

describe('WalletSummaryChartComponent', () => {
  let component: WalletSummaryChartComponent;
  let fixture: ComponentFixture<WalletSummaryChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletSummaryChartComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletSummaryChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
