import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSalesProgressBarComponent } from './project-sales-progress-bar.component';

describe('ProjectSalesProgressBarComponent', () => {
  let component: ProjectSalesProgressBarComponent;
  let fixture: ComponentFixture<ProjectSalesProgressBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectSalesProgressBarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectSalesProgressBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
