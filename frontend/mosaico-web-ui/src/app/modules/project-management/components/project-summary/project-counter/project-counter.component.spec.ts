import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCounterComponent } from './project-counter.component';

describe('ProjectCounterComponent', () => {
  let component: ProjectCounterComponent;
  let fixture: ComponentFixture<ProjectCounterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectCounterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCounterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
