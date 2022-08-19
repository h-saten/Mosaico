import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionPillarsComponent } from './section-pillars.component';

describe('SectionPillarsComponent', () => {
  let component: SectionPillarsComponent;
  let fixture: ComponentFixture<SectionPillarsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionPillarsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionPillarsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
