import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyWalletAssetsComponent } from './company-wallet-assets.component';

describe('CompanyWalletAssetsComponent', () => {
  let component: CompanyWalletAssetsComponent;
  let fixture: ComponentFixture<CompanyWalletAssetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyWalletAssetsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyWalletAssetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
