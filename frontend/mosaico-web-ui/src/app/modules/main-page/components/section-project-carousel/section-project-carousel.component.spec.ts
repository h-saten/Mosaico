import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionProjectCarouselComponent } from './section-project-carousel.component';

describe('SectionProjectCarouselComponent', () => {
  let component: SectionProjectCarouselComponent;
  let fixture: ComponentFixture<SectionProjectCarouselComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionProjectCarouselComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionProjectCarouselComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
