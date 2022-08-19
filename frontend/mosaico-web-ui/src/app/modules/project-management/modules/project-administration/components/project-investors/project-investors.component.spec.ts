import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectInvestorsComponent } from './project-investors.component';

describe('ProjectInvestorsComponent', () => {
  let component: ProjectInvestorsComponent;
  let fixture: ComponentFixture<ProjectInvestorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectInvestorsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectInvestorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
