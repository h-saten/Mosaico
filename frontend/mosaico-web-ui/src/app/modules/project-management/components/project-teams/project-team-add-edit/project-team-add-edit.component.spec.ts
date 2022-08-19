import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTeamsAddEditComponent } from './project-team-add-edit.component';

describe('ProjectTeamsAddEditComponent', () => {
  let component: ProjectTeamsAddEditComponent;
  let fixture: ComponentFixture<ProjectTeamsAddEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectTeamsAddEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTeamsAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
