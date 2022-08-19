import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCompanyLogoComponent } from './edit-company-logo.component';

describe('EditCompanyLogoComponent', () => {
  let component: EditCompanyLogoComponent;
  let fixture: ComponentFixture<EditCompanyLogoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditCompanyLogoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCompanyLogoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
