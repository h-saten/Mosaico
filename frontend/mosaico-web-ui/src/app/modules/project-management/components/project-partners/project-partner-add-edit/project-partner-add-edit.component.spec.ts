import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectPartnerAddEditComponent } from './project-partner-add-edit.component';

describe('ProjectPartnerAddEditComponent', () => {
  let component: ProjectPartnerAddEditComponent;
  let fixture: ComponentFixture<ProjectPartnerAddEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectPartnerAddEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectPartnerAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
