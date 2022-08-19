import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSubscriptionToNewsletterComponent } from './project-subscription-to-newsletter.component';

describe('ProjectSubscriptionToNewsletterComponent', () => {
  let component: ProjectSubscriptionToNewsletterComponent;
  let fixture: ComponentFixture<ProjectSubscriptionToNewsletterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectSubscriptionToNewsletterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectSubscriptionToNewsletterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
