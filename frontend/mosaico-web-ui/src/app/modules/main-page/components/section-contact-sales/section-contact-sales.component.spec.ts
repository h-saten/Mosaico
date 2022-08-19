import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionContactSalesComponent } from './section-contact-sales.component';

describe('SectionContactSalesComponent', () => {
  let component: SectionContactSalesComponent;
  let fixture: ComponentFixture<SectionContactSalesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionContactSalesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionContactSalesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
