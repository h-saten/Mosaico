import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyDetailsCardComponent } from './company-details-card.component';

describe('CompanyDetailsCardComponent', () => {
  let component: CompanyDetailsCardComponent;
  let fixture: ComponentFixture<CompanyDetailsCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyDetailsCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyDetailsCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
