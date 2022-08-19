import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscriptionToNewsletterComponent } from './subscription-to-newsletter.component';

describe('SubscriptionToNewsletterComponent', () => {
  let component: SubscriptionToNewsletterComponent;
  let fixture: ComponentFixture<SubscriptionToNewsletterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubscriptionToNewsletterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubscriptionToNewsletterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
