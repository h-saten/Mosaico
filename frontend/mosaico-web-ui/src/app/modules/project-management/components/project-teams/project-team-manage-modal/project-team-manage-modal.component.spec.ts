import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTeamManageModalComponent } from './project-team-manage-modal.component';

describe('ProjectTeamManageModalComponent', () => {
  let component: ProjectTeamManageModalComponent;
  let fixture: ComponentFixture<ProjectTeamManageModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectTeamManageModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTeamManageModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
