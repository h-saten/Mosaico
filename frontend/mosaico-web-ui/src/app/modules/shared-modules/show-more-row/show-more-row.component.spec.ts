import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowMoreRowComponent } from './show-more-row.component';

describe('ShowMoreRowComponent', () => {
  let component: ShowMoreRowComponent;
  let fixture: ComponentFixture<ShowMoreRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowMoreRowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowMoreRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
