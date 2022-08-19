import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletOverviewPackagesComponent } from './wallet-overview-packages.component';

describe('WalletOverviewPackagesComponent', () => {
  let component: WalletOverviewPackagesComponent;
  let fixture: ComponentFixture<WalletOverviewPackagesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletOverviewPackagesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletOverviewPackagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
