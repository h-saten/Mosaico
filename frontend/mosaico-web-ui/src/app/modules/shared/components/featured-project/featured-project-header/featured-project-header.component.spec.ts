import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeaturedProjectHeaderComponent } from './featured-project-header.component';

describe('FeaturedProjectHeaderComponent', () => {
  let component: FeaturedProjectHeaderComponent;
  let fixture: ComponentFixture<FeaturedProjectHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeaturedProjectHeaderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeaturedProjectHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
