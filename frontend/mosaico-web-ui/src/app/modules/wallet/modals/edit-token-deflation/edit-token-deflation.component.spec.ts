import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTokenDeflationComponent } from './edit-token-deflation.component';

describe('EditTokenDeflationComponent', () => {
  let component: EditTokenDeflationComponent;
  let fixture: ComponentFixture<EditTokenDeflationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditTokenDeflationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditTokenDeflationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
