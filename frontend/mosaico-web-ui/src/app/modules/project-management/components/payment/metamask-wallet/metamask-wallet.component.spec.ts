import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MetamaskWalletComponent } from './metamask-wallet.component';

describe('MetamaskWalletComponent', () => {
  let component: MetamaskWalletComponent;
  let fixture: ComponentFixture<MetamaskWalletComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MetamaskWalletComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MetamaskWalletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
