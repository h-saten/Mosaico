import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectNewsFormComponent } from './project-news-form.component';

describe('ProjectNewsFormComponent', () => {
  let component: ProjectNewsFormComponent;
  let fixture: ComponentFixture<ProjectNewsFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectNewsFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectNewsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
