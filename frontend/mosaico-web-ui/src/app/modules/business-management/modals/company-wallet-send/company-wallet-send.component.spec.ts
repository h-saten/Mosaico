import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyWalletSendComponent } from './company-wallet-send.component';

describe('CompanyWalletSendComponent', () => {
  let component: CompanyWalletSendComponent;
  let fixture: ComponentFixture<CompanyWalletSendComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyWalletSendComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyWalletSendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
