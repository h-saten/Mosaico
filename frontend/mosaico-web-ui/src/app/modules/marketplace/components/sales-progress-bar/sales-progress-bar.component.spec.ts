import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesProgressBarComponent } from './sales-progress-bar.component';

describe('SalesProgressBarComponent', () => {
  let component: SalesProgressBarComponent;
  let fixture: ComponentFixture<SalesProgressBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SalesProgressBarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesProgressBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
