import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvestorCardComponent } from './investor-card.component';

describe('InvestorCardComponent', () => {
  let component: InvestorCardComponent;
  let fixture: ComponentFixture<InvestorCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvestorCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InvestorCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
