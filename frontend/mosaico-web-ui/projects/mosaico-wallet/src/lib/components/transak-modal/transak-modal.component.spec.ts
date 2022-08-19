import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransakModalComponent } from './transak-modal.component';

describe('TransakModalComponent', () => {
  let component: TransakModalComponent;
  let fixture: ComponentFixture<TransakModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TransakModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransakModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
