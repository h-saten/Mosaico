import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectFaqModalComponent } from './project-faq-modal.component';

describe('ProjectFaqModalComponent', () => {
  let component: ProjectFaqModalComponent;
  let fixture: ComponentFixture<ProjectFaqModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectFaqModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectFaqModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
