import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionMobileApplicationComponent } from './section-mobile-application.component';

describe('SectionMobileApplicationComponent', () => {
  let component: SectionMobileApplicationComponent;
  let fixture: ComponentFixture<SectionMobileApplicationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionMobileApplicationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionMobileApplicationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
