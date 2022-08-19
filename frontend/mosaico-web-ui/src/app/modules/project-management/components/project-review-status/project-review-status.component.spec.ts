import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectReviewStatusComponent } from './project-review-status.component';

describe('ProjectReviewStatusComponent', () => {
  let component: ProjectReviewStatusComponent;
  let fixture: ComponentFixture<ProjectReviewStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectReviewStatusComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectReviewStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
