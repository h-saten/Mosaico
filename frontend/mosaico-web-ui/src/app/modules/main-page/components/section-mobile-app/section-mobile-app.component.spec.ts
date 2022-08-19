import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionMobileAppComponent } from './section-mobile-app.component';

describe('SectionMobileAppComponent', () => {
  let component: SectionMobileAppComponent;
  let fixture: ComponentFixture<SectionMobileAppComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionMobileAppComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionMobileAppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
