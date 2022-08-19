import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MosaicoWalletComponent } from './mosaico-wallet.component';

describe('MosaicoWalletComponent', () => {
  let component: MosaicoWalletComponent;
  let fixture: ComponentFixture<MosaicoWalletComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MosaicoWalletComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MosaicoWalletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
