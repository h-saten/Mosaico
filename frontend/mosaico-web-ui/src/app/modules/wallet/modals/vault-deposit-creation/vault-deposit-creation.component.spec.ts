import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VaultDepositCreationComponent } from './vault-deposit-creation.component';

describe('VaultDepositCreationComponent', () => {
  let component: VaultDepositCreationComponent;
  let fixture: ComponentFixture<VaultDepositCreationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VaultDepositCreationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VaultDepositCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
