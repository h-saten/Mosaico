import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPageColorsComponent } from './edit-page-colors.component';

describe('EditPageColorsComponent', () => {
  let component: EditPageColorsComponent;
  let fixture: ComponentFixture<EditPageColorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditPageColorsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPageColorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
