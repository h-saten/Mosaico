import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StakeModalComponent } from './stake-modal.component';

describe('StakeModalComponent', () => {
  let component: StakeModalComponent;
  let fixture: ComponentFixture<StakeModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StakeModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StakeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
