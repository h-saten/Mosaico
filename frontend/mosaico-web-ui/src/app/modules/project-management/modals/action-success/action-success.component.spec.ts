import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActionSuccessComponent } from './action-success.component';

describe('ActionSuccessComponent', () => {
  let component: ActionSuccessComponent;
  let fixture: ComponentFixture<ActionSuccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ActionSuccessComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ActionSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
