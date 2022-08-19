import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionBuildYourCapitalComponent } from './section-build-your-capital.component';

describe('SectionBuildYourCapitalComponent', () => {
  let component: SectionBuildYourCapitalComponent;
  let fixture: ComponentFixture<SectionBuildYourCapitalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionBuildYourCapitalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionBuildYourCapitalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
