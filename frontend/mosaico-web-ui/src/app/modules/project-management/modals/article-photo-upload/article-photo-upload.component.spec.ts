import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticlePhotoUploadComponent } from './article-photo-upload.component';

describe('ArticlePhotoUploadComponent', () => {
  let component: ArticlePhotoUploadComponent;
  let fixture: ComponentFixture<ArticlePhotoUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ArticlePhotoUploadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ArticlePhotoUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
