import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TokenMintComponent } from './token-mint.component';

describe('TokenMintComponent', () => {
  let component: TokenMintComponent;
  let fixture: ComponentFixture<TokenMintComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TokenMintComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TokenMintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
