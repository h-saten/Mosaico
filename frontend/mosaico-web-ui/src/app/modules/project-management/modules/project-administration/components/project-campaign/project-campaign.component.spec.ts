import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCampaignComponent } from './project-campaign.component';

describe('ProjectCampaignComponent', () => {
  let component: ProjectCampaignComponent;
  let fixture: ComponentFixture<ProjectCampaignComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectCampaignComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCampaignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
