import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StageEditingComponent } from './stage-editing.component';

describe('StageEditingComponent', () => {
  let component: StageEditingComponent;
  let fixture: ComponentFixture<StageEditingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StageEditingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StageEditingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
