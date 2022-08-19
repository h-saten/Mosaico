import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionOurTeamManagementComponent } from './section-our-team-management.component';

describe('OurTeamManagementComponent', () => {
  let component: SectionOurTeamManagementComponent;
  let fixture: ComponentFixture<SectionOurTeamManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionOurTeamManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionOurTeamManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
