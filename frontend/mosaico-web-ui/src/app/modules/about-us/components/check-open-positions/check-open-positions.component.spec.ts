import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckOpenPositionsComponent } from './check-open-positions.component';

describe('CheckOpenPositionsComponent', () => {
  let component: CheckOpenPositionsComponent;
  let fixture: ComponentFixture<CheckOpenPositionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CheckOpenPositionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckOpenPositionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
