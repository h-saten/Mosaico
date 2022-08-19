import { Component, EventEmitter, Input, OnInit, Output, OnDestroy, ViewChild } from '@angular/core';

import { ImageCropperLocalService } from './image-cropper-local.service';
import { LoadedImage, ImageCroppedEvent, Dimensions, ImageTransform, CropperPosition, ImageCropperComponent } from 'ngx-image-cropper';
import { FileParameters, FormModeEnum } from 'mosaico-base';

@Component({
  selector: 'app-image-cropper-local',
  templateUrl: './image-cropper-local.component.html',
  styleUrls: ['./image-cropper-local.component.scss']
})
export class ImageCropperLocalComponent implements OnInit, OnDestroy {

  @Input() currentImgUrl = '';
  @Input() currentFormMode: FormModeEnum;
  @Input() resizeToWidth: number;
  @Input() aspectRatio: number; // proporcje zdjęcia
  @Input() roundCropper: boolean; // czy zdjęcie ma byc w kółku
  @Input() imageCropperClass: string | string[];
  @Input() isRemovable = true;
  @Input() addPhotoText: string;
  @Input() changePhotoText: string;
  @Input() blankImage = '/assets/media/avatars/blank.png';
  @Input() containWithinAspectRatio?: boolean; // to remove
  @Input() showAllowedFormats = true;
  @Input() preview100percent = false;
  @Output() editingIsInProgressEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() editingCancelEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  FormModeEnum: typeof FormModeEnum = FormModeEnum;

  private imgFileNameInitial = '';
  // photoFileName: string = '';
  // photoData: File = null;
  notAllowedFileFormat = false;
  fileSizeExceeded = false;

  // showCropper = false;
  imageChangedEvent: any = '';
  croppedImage = '';
  cropImage = false;

  canvasRotation = 0;
  rotation = 0;
  scale = 1;
  transform: ImageTransform = {};

  cropperPosition_x1 = 0;
  cropperPosition_x2 = 0;
  cropperPosition_y1 = 0;
  cropperPosition_y2 = 0;

  cropper: CropperPosition = {
    x1: 0,
    y1: 0,
    x2: 0,
    y2: 0
  };

  cropperSave: CropperPosition = {
    x1: 0,
    y1: 0,
    x2: 0,
    y2: 0
  };

  isButtonSaveCrop = false; // pressing the Save button - starting the view of the cropped photo

  @ViewChild(ImageCropperComponent) imageCropper: ImageCropperComponent;

  isLoadImageFailed = false;

  constructor(
    private imageCropperLocalService: ImageCropperLocalService,
  ) { }

  ngOnDestroy(): void {
    this.imageCropperLocalService.resetAllVariables();
  }

  ngOnInit(): void {
  }

  fileChangeEvent(fileInput: Event): void {

    this.imageChangedEvent = fileInput;

    this.imgFileNameInitial = this.imageChangedEvent.target.files[0].name;

    this.cropImage = true;

    this.editingIsInProgressEvent.emit(true);
  }

  imageLoaded(image: LoadedImage): void {

    // the function is run only once when the user loads the file
    // passes the original data of the file (base64) and after its transformation immediately after loading (base64)

    /*
    needed until the next time - because something is wrong with the loaded sizes
    console.error('imageLoaded - image: ', image);
    console.error('imageLoaded - image: ', image.original.size.height);
    console.error('imageLoaded - image: ', image.original.size.width);

    console.error('imageLoaded - image: ', image.transformed.size.height);
    console.error('imageLoaded - image: ', image.transformed.size.width);
    */
  }

  // this function generates the final cropped image as base64
  // when autoCrop = true - the function starts when the trimming movement is finished - that is, after each trimming change
  // so after each shift of the crop - the new file data is already generated in the service using the imageCroppedPrepareToSave function

  // change!!! - autoCrop = false - now the function starts only after pressing the SaveCrop button

  imageCropped(event: ImageCroppedEvent): void {
    // console.error('imageCropped - event: ', event);

    // preparation of a cropped fragment of graphics for saving in the database
    this.imageCropperLocalService.imageCroppedPrepareToSave(event, this.imgFileNameInitial);

    // above, a new photo is created - the data of the added photo will be read in the photo saving function - e.g. in the modal mode
    // using the code below

    if (typeof (event.base64) === 'string') {

      this.croppedImage = event.base64;

      this.cropperPosition_x1 = event.cropperPosition.x1;
      this.cropperPosition_x2 = event.cropperPosition.x2;
      this.cropperPosition_y1 = event.cropperPosition.y1;
      this.cropperPosition_y2 = event.cropperPosition.y2;
    }
  }

  cropperReady(sourceImageDimensions: Dimensions): void {
    // console.error('cropperReady - sourceImageDimensions: ', sourceImageDimensions);
    // cropper ready
    // the returned dimensions are height: width: of the loaded image - not the cropped image

    // setting the trimmer as it was before saveCrop () and cancelCrop ()
    if (this.cropperSave.x1 > 0 || this.cropperSave.x2 > 0 || this.cropperSave.y1 > 0 || this.cropperSave.y2 > 0) {
      this.cropper = this.cropperSave;
    }

  }

  startCropImage(event): void {
    // console.error('startCropImage - event: ', event);
  }

  loadImageFailed(event: any): void {
    // console.error('loadImageFailed - event: ', event);

    if (event === undefined) {
      this.isLoadImageFailed = true;
    } else {
      this.isLoadImageFailed = false;
    }
  }

  saveCrop(): void {

    this.isButtonSaveCrop = true; // start the photo cropping process

    this.imageCropper.crop(); // calls the imageCropped () function

    // saving the position of the trimmer so that when you click changeCrop - the trimmer will be in the same place as it was when you saved it
    this.cropperSave = {x1: this.cropperPosition_x1, y1: this.cropperPosition_y1, x2: this.cropperPosition_x2, y2: this.cropperPosition_y2};

    this.cropImage = !this.cropImage;

    this.editingIsInProgressEvent.emit(false);
  }

  changeCrop(): void {

    this.cropImage = !this.cropImage;

    this.editingIsInProgressEvent.emit(true);
  }

  cancelCrop(): void {
    if (this.isButtonSaveCrop === true) {
      // if you press cancel, it will return to the view of the previously trimmed photo
      this.cropImage = false;
      this.editingIsInProgressEvent.emit(false);
    } else {
      this.cropImage = false;
      this.imageChangedEvent = '';
      this.croppedImage = '';
      this.imageCropperLocalService.resetAllVariables();

      this.editingIsInProgressEvent.emit(false);
      this.editingCancelEvent.emit(true);
    }

    this.isLoadImageFailed = false;
  }

  removeImage(): void {
    if(this.isRemovable){
      this.currentImgUrl = '';
      this.imageCropperLocalService.setRemoveCurrentImage();
      this.editingIsInProgressEvent.emit(false);
    }
  }

  getFileParameters(): FileParameters {
    // data for the photo has been generated in image-cropper-local in the imageCropped () functio`
    return this.imageCropperLocalService.getFileParameters() as FileParameters;
  }


  // for scaling - not yet used

  resetImage(): void {

    this.scale = 1;
    this.rotation = 0;
    this.canvasRotation = 0;
    this.transform = {};
  }

  zoomOut(): void {
    if (this.scale > 1) {
      this.scale -= .1;
      this.transform = {
          ...this.transform,
          scale: this.scale
      };
    }

    else
    {
      console.warn('Nie można bardziej zmniejszyć grafiki!');
    }
  }

  zoomIn(): void {
      this.scale += .1;
      this.transform = {
          ...this.transform,
          scale: this.scale
      };
  }

}
