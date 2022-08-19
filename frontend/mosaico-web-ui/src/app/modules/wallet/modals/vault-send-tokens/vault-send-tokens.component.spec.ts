import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VaultSendTokensComponent } from './vault-send-tokens.component';

describe('VaultSendTokensComponent', () => {
  let component: VaultSendTokensComponent;
  let fixture: ComponentFixture<VaultSendTokensComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VaultSendTokensComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VaultSendTokensComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
