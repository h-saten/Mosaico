import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProjectMembersComponent } from './edit-project-members.component';

describe('EditProjectMembersComponent', () => {
  let component: EditProjectMembersComponent;
  let fixture: ComponentFixture<EditProjectMembersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditProjectMembersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProjectMembersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
