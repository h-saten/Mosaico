import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectNewsCardComponent } from './project-news-card.component';

describe('ProjectNewsCardComponent', () => {
  let component: ProjectNewsCardComponent;
  let fixture: ComponentFixture<ProjectNewsCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectNewsCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectNewsCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
