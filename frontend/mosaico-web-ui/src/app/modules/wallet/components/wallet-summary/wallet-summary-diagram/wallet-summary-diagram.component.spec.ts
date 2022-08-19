import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletSummaryDiagramComponent } from './wallet-summary-diagram.component';

describe('WalletSummaryDiagramComponent', () => {
  let component: WalletSummaryDiagramComponent;
  let fixture: ComponentFixture<WalletSummaryDiagramComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletSummaryDiagramComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletSummaryDiagramComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
