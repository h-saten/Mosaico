import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatusFollowShareComponent } from './status-follow-share.component';

describe('StatusFollowShareComponent', () => {
  let component: StatusFollowShareComponent;
  let fixture: ComponentFixture<StatusFollowShareComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatusFollowShareComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatusFollowShareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
