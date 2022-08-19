import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactOurSalesComponent } from './contact-our-sales.component';

describe('SectionContactOurSalesComponent', () => {
  let component: ContactOurSalesComponent;
  let fixture: ComponentFixture<ContactOurSalesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContactOurSalesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContactOurSalesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
