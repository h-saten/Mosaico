import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorizationCodeComponent } from './authorization-code.component';

describe('AuthorizationCodeComponent', () => {
  let component: AuthorizationCodeComponent;
  let fixture: ComponentFixture<AuthorizationCodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuthorizationCodeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthorizationCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
