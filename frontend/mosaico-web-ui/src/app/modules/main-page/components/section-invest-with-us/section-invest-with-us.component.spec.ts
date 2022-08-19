import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionInvestWithUsComponent } from './section-invest-with-us.component';

describe('SectionInvestWithUsComponent', () => {
  let component: SectionInvestWithUsComponent;
  let fixture: ComponentFixture<SectionInvestWithUsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionInvestWithUsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionInvestWithUsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
