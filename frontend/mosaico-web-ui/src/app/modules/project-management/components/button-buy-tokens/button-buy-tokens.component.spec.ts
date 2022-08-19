import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ButtonBuyTokensComponent } from './button-buy-tokens.component';

describe('ButtonBuyTokensComponent', () => {
  let component: ButtonBuyTokensComponent;
  let fixture: ComponentFixture<ButtonBuyTokensComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ButtonBuyTokensComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ButtonBuyTokensComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
