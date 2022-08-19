import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyRoadMapComponent } from './company-road-map.component';

describe('CompanyRoadMapComponent', () => {
  let component: CompanyRoadMapComponent;
  let fixture: ComponentFixture<CompanyRoadMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyRoadMapComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyRoadMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
