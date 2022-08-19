import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionMeetTeamComponent } from './section-meet-team.component';

describe('SectionMeetTeamComponent', () => {
  let component: SectionMeetTeamComponent;
  let fixture: ComponentFixture<SectionMeetTeamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionMeetTeamComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionMeetTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
