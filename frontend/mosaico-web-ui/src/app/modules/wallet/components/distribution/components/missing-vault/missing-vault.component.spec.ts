import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MissingVaultComponent } from './missing-vault.component';

describe('MissingVaultComponent', () => {
  let component: MissingVaultComponent;
  let fixture: ComponentFixture<MissingVaultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MissingVaultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MissingVaultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
