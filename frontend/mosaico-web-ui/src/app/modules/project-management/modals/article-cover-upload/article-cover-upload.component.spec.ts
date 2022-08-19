import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticleCoverUploadComponent } from './article-cover-upload.component';

describe('ArticleCoverUploadComponent', () => {
  let component: ArticleCoverUploadComponent;
  let fixture: ComponentFixture<ArticleCoverUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ArticleCoverUploadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ArticleCoverUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
