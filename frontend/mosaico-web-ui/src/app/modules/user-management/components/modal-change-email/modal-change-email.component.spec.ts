import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalChangeEmailComponent } from './modal-change-email.component';

describe('ModalChangeEmailComponent', () => {
  let component: ModalChangeEmailComponent;
  let fixture: ComponentFixture<ModalChangeEmailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalChangeEmailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalChangeEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
