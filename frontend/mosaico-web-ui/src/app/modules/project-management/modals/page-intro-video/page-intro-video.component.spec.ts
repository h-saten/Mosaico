import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageIntroVideoComponent } from './page-intro-video.component';

describe('PageIntroVideoComponent', () => {
  let component: PageIntroVideoComponent;
  let fixture: ComponentFixture<PageIntroVideoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageIntroVideoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageIntroVideoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
