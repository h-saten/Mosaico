import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeaturedProjectFooterComponent } from './featured-project-footer.component';

describe('FeaturedProjectFooterComponent', () => {
  let component: FeaturedProjectFooterComponent;
  let fixture: ComponentFixture<FeaturedProjectFooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeaturedProjectFooterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeaturedProjectFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
