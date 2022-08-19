import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTokenomicsComponent } from './project-tokenomics.component';

describe('ProjectTokenomicsComponent', () => {
  let component: ProjectTokenomicsComponent;
  let fixture: ComponentFixture<ProjectTokenomicsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectTokenomicsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTokenomicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
