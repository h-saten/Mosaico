import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewProjectMemberComponent } from './new-project-member.component';

describe('NewProjectMemberComponent', () => {
  let component: NewProjectMemberComponent;
  let fixture: ComponentFixture<NewProjectMemberComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewProjectMemberComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewProjectMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
