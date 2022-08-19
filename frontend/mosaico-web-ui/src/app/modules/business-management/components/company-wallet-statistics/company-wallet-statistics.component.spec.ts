import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyWalletStatisticsComponent } from './company-wallet-statistics.component';

describe('CompanyWalletStatisticsComponent', () => {
  let component: CompanyWalletStatisticsComponent;
  let fixture: ComponentFixture<CompanyWalletStatisticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyWalletStatisticsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyWalletStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
