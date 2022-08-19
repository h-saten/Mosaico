import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TokenManagementComponent } from './token-management.component';

describe('TokenManagementComponent', () => {
  let component: TokenManagementComponent;
  let fixture: ComponentFixture<TokenManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TokenManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TokenManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
