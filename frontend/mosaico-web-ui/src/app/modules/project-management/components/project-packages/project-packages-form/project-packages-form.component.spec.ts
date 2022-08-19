import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectPackagesFormComponent } from './project-packages-form.component';

describe('ProjectPackagesFormComponent', () => {
  let component: ProjectPackagesFormComponent;
  let fixture: ComponentFixture<ProjectPackagesFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectPackagesFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectPackagesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
