import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetTeamSocialLinksComponent } from './meet-team-social-links.component';

describe('MeetTeamSocialLinksComponent', () => {
  let component: MeetTeamSocialLinksComponent;
  let fixture: ComponentFixture<MeetTeamSocialLinksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MeetTeamSocialLinksComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MeetTeamSocialLinksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
