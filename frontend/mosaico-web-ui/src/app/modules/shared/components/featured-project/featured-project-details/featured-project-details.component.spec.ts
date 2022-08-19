import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeaturedProjectDetailsComponent } from './featured-project-details.component';

describe('FeaturedProjectDetailsComponent', () => {
  let component: FeaturedProjectDetailsComponent;
  let fixture: ComponentFixture<FeaturedProjectDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeaturedProjectDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeaturedProjectDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
