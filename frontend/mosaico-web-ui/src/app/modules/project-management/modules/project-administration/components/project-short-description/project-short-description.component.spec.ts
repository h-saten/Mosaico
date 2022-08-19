import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectShortDescriptionComponent } from './project-short-description.component';

describe('ProjectShortDescriptionComponent', () => {
  let component: ProjectShortDescriptionComponent;
  let fixture: ComponentFixture<ProjectShortDescriptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectShortDescriptionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectShortDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
