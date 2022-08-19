import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCoverUploadComponent } from './project-cover-upload.component';

describe('ProjectCoverUploadComponent', () => {
  let component: ProjectCoverUploadComponent;
  let fixture: ComponentFixture<ProjectCoverUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectCoverUploadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCoverUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
