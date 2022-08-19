import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BeamerComponent } from './beamer.component';

describe('BeamerComponent', () => {
  let component: BeamerComponent;
  let fixture: ComponentFixture<BeamerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BeamerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BeamerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
