import { Injectable } from '@angular/core';
import { FileParameters } from 'mosaico-base';
import { ImageCroppedEvent } from 'ngx-image-cropper';

@Injectable()
export class ImageCropperLocalService {
  private newImgData: File | null;
  notAllowedFileFormat = false;
  fileSizeExceeded = false;
  private removeCurrentImage = false;

  imageCroppedPrepareToSave(event: ImageCroppedEvent, fileNameInitial: string): void {
    if (typeof (event.base64) === 'string') {
      const croppedImage: string = event.base64;
      const blob = this.convertBase64ToBlob(croppedImage);
      const imageBlob: Blob = blob;
      const partsImage = fileNameInitial.split('.');
      const imageName: string = partsImage[0] + '_cropped.jpeg'; // nazwa oryginalna plus info że wycięte i zmiana rozszerzenia na jpeg
      const imageFile: File = new File([imageBlob], imageName, {
        type: 'image/jpeg',
        lastModified: Date.now()
      });
      this.prepareAsAFile(imageFile);
    }

  }

  private convertBase64ToBlob(Base64Image: string): Blob {

    const parts = Base64Image.split(';base64,');

    // HOLD THE CONTENT TYPE
    const imageType = parts[0].split(':')[1];

    // DECODE BASE64 STRING
    const decodedData = window.atob(parts[1]);

    // CREATE UNIT8ARRAY OF SIZE SAME AS ROW DATA LENGTH
    const uInt8Array = new Uint8Array(decodedData.length);

    // INSERT ALL CHARACTER CODE INTO UINT8ARRAY
    for (let i = 0; i < decodedData.length; ++i)
    {
      uInt8Array[i] = decodedData.charCodeAt(i);
    }
    // RETURN BLOB IMAGE AFTER CONVERSION
    return new Blob([uInt8Array], { type: imageType });

  }

  // if necessary, another function - from the example
  // https://stackblitz.com/edit/image-cropper-qnkclf?file=app%2Fimage-cropper%2Futils%2Fblob.utils.ts
  /*
  base64ToFile(base64Image: string): Blob {
    const split = base64Image.split(',');
    const type = split[0].replace('data:', '').replace(';base64', '');
    const byteString = atob(split[1]);
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i += 1) {
        ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], {type});
  }
  */

  private prepareAsAFile(file: File): void {

    if (file) {

      const fileToUpload = file as File;

      this.validateFormat(fileToUpload);
      this.validateSize(fileToUpload);

      this.newImgData = fileToUpload;
    }
  }

  setRemoveCurrentImage(): void {
    this.removeCurrentImage = true;
  }

  getFileParameters (): FileParameters {

    let fileParameters: FileParameters;

    fileParameters = {
      data: this.newImgData ? this.newImgData : null,
      fileName: this.newImgData ? this.newImgData?.name : null,
      removeImg: this.removeCurrentImage
    };

    return fileParameters;
  }

   // called in ngDestroy image-cropper-local and in function cancelFirstBeforeSaveCrop()
  resetAllVariables(): void {
    this.newImgData = null;
    this.removeCurrentImage = false;
  }

  private validateFormat(file: File): void {

    this.notAllowedFileFormat = false;

    const validFileType = ['image/jpeg', 'image/gif', 'image/png', 'image/jpg'];

    if (validFileType.includes(file.type)) {
      return;
    }

    this.notAllowedFileFormat = true;

  }

  private validateSize(file: File): void {
    this.fileSizeExceeded = false;
    const maxFileSize = 10;
    if (file.size / 1024 / 1024 <= maxFileSize) {
      return;
    }
    this.fileSizeExceeded = true;

  }

}
