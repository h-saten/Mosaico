import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordConfirmAccountDeleteComponent } from './password-confirm-account-delete.component';

describe('PasswordConfirmAccountDeleteComponent', () => {
  let component: PasswordConfirmAccountDeleteComponent;
  let fixture: ComponentFixture<PasswordConfirmAccountDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PasswordConfirmAccountDeleteComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PasswordConfirmAccountDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
