import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletPanelAssetRowComponent } from './wallet-panel-assets-row.component';

describe('WalletAssetComponent', () => {
  let component: WalletPanelAssetRowComponent;
  let fixture: ComponentFixture<WalletPanelAssetRowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletPanelAssetRowComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletPanelAssetRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
