import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectAirdropsComponent } from './project-airdrops.component';

describe('ProjectAirdropsComponent', () => {
  let component: ProjectAirdropsComponent;
  let fixture: ComponentFixture<ProjectAirdropsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectAirdropsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectAirdropsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

