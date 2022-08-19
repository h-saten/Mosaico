import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorizePrivateSaleComponent } from './authorize-private-sale.component';

describe('AuthorizePrivateSaleComponent', () => {
  let component: AuthorizePrivateSaleComponent;
  let fixture: ComponentFixture<AuthorizePrivateSaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuthorizePrivateSaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthorizePrivateSaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
