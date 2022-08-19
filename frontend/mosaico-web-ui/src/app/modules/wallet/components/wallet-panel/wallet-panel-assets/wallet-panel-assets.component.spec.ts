import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletPanelAssetsComponent } from './wallet-panel-assets.component';

describe('WalletPanelAssetsComponent', () => {
  let component: WalletPanelAssetsComponent;
  let fixture: ComponentFixture<WalletPanelAssetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletPanelAssetsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletPanelAssetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
