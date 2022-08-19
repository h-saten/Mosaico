import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionPlatformComponent } from './section-platform.component';

describe('SectionPlatformComponent', () => {
  let component: SectionPlatformComponent;
  let fixture: ComponentFixture<SectionPlatformComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SectionPlatformComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionPlatformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
