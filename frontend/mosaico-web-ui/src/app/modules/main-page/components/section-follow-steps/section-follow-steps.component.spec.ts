import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionFollowStepsComponent } from './section-follow-steps.component';

describe('SectionFollowStepsComponent', () => {
  let component: SectionFollowStepsComponent;
  let fixture: ComponentFixture<SectionFollowStepsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionFollowStepsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionFollowStepsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
