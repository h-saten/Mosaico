import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionGetintouchComponent } from './section-getintouch.component';

describe('SectionGetintouchComponent', () => {
  let component: SectionGetintouchComponent;
  let fixture: ComponentFixture<SectionGetintouchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionGetintouchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionGetintouchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
