import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectNewsComponent } from './project-news.component';

describe('ProjectNewsComponent', () => {
  let component: ProjectNewsComponent;
  let fixture: ComponentFixture<ProjectNewsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectNewsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectNewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
