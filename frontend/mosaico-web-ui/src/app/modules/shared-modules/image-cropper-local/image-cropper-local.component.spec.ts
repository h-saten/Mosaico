import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageCropperLocalComponent } from './image-cropper-local.component';

describe('ImageCropperLocalComponent', () => {
  let component: ImageCropperLocalComponent;
  let fixture: ComponentFixture<ImageCropperLocalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImageCropperLocalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageCropperLocalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
