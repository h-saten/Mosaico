import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RampModalComponent } from './ramp-modal.component';

describe('RampModalComponent', () => {
  let component: RampModalComponent;
  let fixture: ComponentFixture<RampModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RampModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RampModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
