import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisibilityEditComponent } from './visibility-edit.component';

describe('VisibilityEditComponent', () => {
  let component: VisibilityEditComponent;
  let fixture: ComponentFixture<VisibilityEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VisibilityEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VisibilityEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
