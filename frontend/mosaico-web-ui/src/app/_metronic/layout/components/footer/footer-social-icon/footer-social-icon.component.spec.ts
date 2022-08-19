import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterSocialIconComponent } from './footer-social-icon.component';

describe('FooterSocialIconComponent', () => {
  let component: FooterSocialIconComponent;
  let fixture: ComponentFixture<FooterSocialIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FooterSocialIconComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FooterSocialIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
