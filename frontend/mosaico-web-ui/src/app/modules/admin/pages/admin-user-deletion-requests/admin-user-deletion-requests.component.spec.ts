import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserDeletionRequestsComponent } from './admin-user-deletion-requests.component';

describe('AdminUserDeletionRequestsComponent', () => {
  let component: AdminUserDeletionRequestsComponent;
  let fixture: ComponentFixture<AdminUserDeletionRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminUserDeletionRequestsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserDeletionRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
