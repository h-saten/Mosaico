import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TokenTypeSelectorComponent } from './token-type-selector.component';

describe('TokenTypeSelectorComponent', () => {
  let component: TokenTypeSelectorComponent;
  let fixture: ComponentFixture<TokenTypeSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TokenTypeSelectorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TokenTypeSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
