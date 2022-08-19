import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmBankTransferComponent } from './confirm-bank-transfer.component';

describe('ConfirmBankTransferComponent', () => {
  let component: ConfirmBankTransferComponent;
  let fixture: ComponentFixture<ConfirmBankTransferComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfirmBankTransferComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmBankTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
