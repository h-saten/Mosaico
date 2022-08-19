import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectFaqFormComponent } from './faq-form.component';

describe('ProjectFaqFormComponent', () => {
  let component: ProjectFaqFormComponent;
  let fixture: ComponentFixture<ProjectFaqFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectFaqFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectFaqFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
