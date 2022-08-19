import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectPackagesModalComponent } from './project-packages-modal.component';

describe('ProjectPackagesModalComponent', () => {
  let component: ProjectPackagesModalComponent;
  let fixture: ComponentFixture<ProjectPackagesModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectPackagesModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectPackagesModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
