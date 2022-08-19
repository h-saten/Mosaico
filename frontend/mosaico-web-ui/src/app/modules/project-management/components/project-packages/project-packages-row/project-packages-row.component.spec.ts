import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectPackagesRowComponent } from './project-packages-row.component';

describe('ProjectPackagesRowComponent', () => {
  let component: ProjectPackagesRowComponent;
  let fixture: ComponentFixture<ProjectPackagesRowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectPackagesRowComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectPackagesRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
