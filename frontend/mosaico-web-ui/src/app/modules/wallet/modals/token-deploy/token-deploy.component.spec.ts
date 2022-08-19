import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TokenDeployComponent } from './token-deploy.component';

describe('TokenDeployComponent', () => {
  let component: TokenDeployComponent;
  let fixture: ComponentFixture<TokenDeployComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TokenDeployComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TokenDeployComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
