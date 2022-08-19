import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionPillarsInvestmentComponent } from './section-pillars-investment.component';

describe('SectionPillarsInvestmentComponent', () => {
  let component: SectionPillarsInvestmentComponent;
  let fixture: ComponentFixture<SectionPillarsInvestmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionPillarsInvestmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionPillarsInvestmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
