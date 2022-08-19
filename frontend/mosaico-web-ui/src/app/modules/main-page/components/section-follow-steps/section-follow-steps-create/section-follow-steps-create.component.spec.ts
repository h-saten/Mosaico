import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionFollowStepsCreateComponent } from './section-follow-steps-create.component';

describe('SectionFollowStepsCreateComponent', () => {
  let component: SectionFollowStepsCreateComponent;
  let fixture: ComponentFixture<SectionFollowStepsCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionFollowStepsCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionFollowStepsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
