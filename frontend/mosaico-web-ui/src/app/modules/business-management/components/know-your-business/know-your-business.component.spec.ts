import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KnowYourBusinessComponent } from './know-your-business.component';

describe('KnowYourBusinessComponent', () => {
  let component: KnowYourBusinessComponent;
  let fixture: ComponentFixture<KnowYourBusinessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ KnowYourBusinessComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(KnowYourBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
