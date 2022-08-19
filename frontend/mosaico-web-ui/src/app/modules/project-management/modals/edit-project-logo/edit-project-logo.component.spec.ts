import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProjectLogoComponent } from './edit-project-logo.component';

describe('EditProjectLogoComponent', () => {
  let component: EditProjectLogoComponent;
  let fixture: ComponentFixture<EditProjectLogoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditProjectLogoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProjectLogoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
