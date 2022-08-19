import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditBankDataComponent } from './edit-bank-data.component';

describe('EditBankDataComponent', () => {
  let component: EditBankDataComponent;
  let fixture: ComponentFixture<EditBankDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditBankDataComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditBankDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
