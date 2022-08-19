import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDocumentTemplatesListComponent } from './project-document-templates-list.component';

describe('ProjectDocumentTemplatesListComponent', () => {
  let component: ProjectDocumentTemplatesListComponent;
  let fixture: ComponentFixture<ProjectDocumentTemplatesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectDocumentTemplatesListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectDocumentTemplatesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
