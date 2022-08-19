import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalTransactionConfirmationComponent } from './external-transaction-confirmation.component';

describe('ExternalTransactionConfirmationComponent', () => {
  let component: ExternalTransactionConfirmationComponent;
  let fixture: ComponentFixture<ExternalTransactionConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExternalTransactionConfirmationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExternalTransactionConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
