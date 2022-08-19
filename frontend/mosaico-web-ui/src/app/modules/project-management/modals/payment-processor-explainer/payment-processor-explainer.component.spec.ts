import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentProcessorExplainerComponent } from './payment-processor-explainer.component';

describe('PaymentProcessorExplainerComponent', () => {
  let component: PaymentProcessorExplainerComponent;
  let fixture: ComponentFixture<PaymentProcessorExplainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PaymentProcessorExplainerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentProcessorExplainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
