import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectAddDocumentModalComponent } from './project-add-document-modal.component';

describe('ProjectFaqModalComponent', () => {
  let component: ProjectAddDocumentModalComponent;
  let fixture: ComponentFixture<ProjectAddDocumentModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectAddDocumentModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectAddDocumentModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
