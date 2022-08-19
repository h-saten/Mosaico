import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterUrlDocumentsComponent } from './footer-url-documents.component';

describe('FooterUrlDocumentsComponent', () => {
  let component: FooterUrlDocumentsComponent;
  let fixture: ComponentFixture<FooterUrlDocumentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FooterUrlDocumentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FooterUrlDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
