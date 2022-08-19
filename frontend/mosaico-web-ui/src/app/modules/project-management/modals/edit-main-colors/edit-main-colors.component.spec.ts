import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMainColorsComponent } from './edit-main-colors.component';

describe('EditMainColorsComponent', () => {
  let component: EditMainColorsComponent;
  let fixture: ComponentFixture<EditMainColorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditMainColorsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditMainColorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
