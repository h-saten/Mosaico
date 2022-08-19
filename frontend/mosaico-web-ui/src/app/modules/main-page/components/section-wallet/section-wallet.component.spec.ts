import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionWalletComponent } from './section-wallet.component';

describe('SectionWalletComponent', () => {
  let component: SectionWalletComponent;
  let fixture: ComponentFixture<SectionWalletComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionWalletComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionWalletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
