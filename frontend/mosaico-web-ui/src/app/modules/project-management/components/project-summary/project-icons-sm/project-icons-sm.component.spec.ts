import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectIconsSmComponent } from './project-icons-sm.component';

describe('ProjectIconsSmComponent', () => {
  let component: ProjectIconsSmComponent;
  let fixture: ComponentFixture<ProjectIconsSmComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectIconsSmComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectIconsSmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
