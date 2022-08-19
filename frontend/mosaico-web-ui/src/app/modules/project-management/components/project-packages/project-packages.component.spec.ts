import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectPackagesComponent } from './project-packages.component';

describe('ProjectPackagesComponent', () => {
  let component: ProjectPackagesComponent;
  let fixture: ComponentFixture<ProjectPackagesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectPackagesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectPackagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
