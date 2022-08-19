import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProjectCoverComponent } from './edit-project-cover.component';

describe('EditProjectCoverComponent', () => {
  let component: EditProjectCoverComponent;
  let fixture: ComponentFixture<EditProjectCoverComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditProjectCoverComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProjectCoverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
