import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MetaWalletFundingComponent } from './meta-wallet-funding.component';

describe('MetaWalletFundingComponent', () => {
  let component: MetaWalletFundingComponent;
  let fixture: ComponentFixture<MetaWalletFundingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MetaWalletFundingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MetaWalletFundingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
