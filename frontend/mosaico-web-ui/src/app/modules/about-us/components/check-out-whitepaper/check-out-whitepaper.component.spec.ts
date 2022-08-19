import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckOutWhitepaperComponent } from './check-out-whitepaper.component';

describe('CheckOutWhitepaperComponent', () => {
  let component: CheckOutWhitepaperComponent;
  let fixture: ComponentFixture<CheckOutWhitepaperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CheckOutWhitepaperComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckOutWhitepaperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
