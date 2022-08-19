import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletAssetComponent } from './wallet-asset.component';

describe('WalletAssetComponent', () => {
  let component: WalletAssetComponent;
  let fixture: ComponentFixture<WalletAssetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletAssetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletAssetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
