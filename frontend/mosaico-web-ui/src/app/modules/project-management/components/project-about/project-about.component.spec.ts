import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCompanyDetailsComponent } from './project-about.component';

describe('ProjectCompanyDetailsComponent', () => {
  let component: ProjectCompanyDetailsComponent;
  let fixture: ComponentFixture<ProjectCompanyDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectCompanyDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCompanyDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
