import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AirdropWithdrawalComponent } from './airdrop-withdrawal.component';

describe('AirdropWithdrawalComponent', () => {
  let component: AirdropWithdrawalComponent;
  let fixture: ComponentFixture<AirdropWithdrawalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AirdropWithdrawalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AirdropWithdrawalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
