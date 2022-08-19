import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JoinOurNewsletterComponent } from './join-our-newsletter.component';

describe('SectionJoinOurNewsletterComponent', () => {
  let component: JoinOurNewsletterComponent;
  let fixture: ComponentFixture<JoinOurNewsletterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JoinOurNewsletterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JoinOurNewsletterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
