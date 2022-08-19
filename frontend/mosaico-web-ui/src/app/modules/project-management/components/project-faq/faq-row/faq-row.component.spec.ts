import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FaqRowComponent } from './faq-row.component';

describe('FaqRowComponent', () => {
  let component: FaqRowComponent;
  let fixture: ComponentFixture<FaqRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FaqRowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FaqRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
