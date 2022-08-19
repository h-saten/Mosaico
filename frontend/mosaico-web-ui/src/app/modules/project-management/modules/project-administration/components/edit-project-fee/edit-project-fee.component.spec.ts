import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProjectFeeComponent } from './edit-project-fee.component';

describe('EditProjectFeeComponent', () => {
  let component: EditProjectFeeComponent;
  let fixture: ComponentFixture<EditProjectFeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditProjectFeeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProjectFeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
