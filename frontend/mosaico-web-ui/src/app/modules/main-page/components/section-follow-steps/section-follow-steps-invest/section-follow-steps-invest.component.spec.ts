import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionFollowStepsInvestComponent } from './section-follow-steps-invest.component';

describe('SectionFollowStepsInvestComponent', () => {
  let component: SectionFollowStepsInvestComponent;
  let fixture: ComponentFixture<SectionFollowStepsInvestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionFollowStepsInvestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionFollowStepsInvestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
