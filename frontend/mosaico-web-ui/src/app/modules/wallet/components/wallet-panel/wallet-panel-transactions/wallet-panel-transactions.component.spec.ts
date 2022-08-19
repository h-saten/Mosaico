import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletPanelTransactionsComponent } from './wallet-panel-transactions.component';

describe('WalletPanelTransactionsComponent', () => {
  let component: WalletPanelTransactionsComponent;
  let fixture: ComponentFixture<WalletPanelTransactionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletPanelTransactionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletPanelTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
