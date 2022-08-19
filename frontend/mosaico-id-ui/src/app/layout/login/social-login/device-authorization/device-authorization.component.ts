import {Component, EventEmitter, Inject, OnDestroy, Output} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from '@angular/material/dialog';
import {Subscription} from 'rxjs';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {DeviceAuthorization, DeviceAuthorizationType} from "../../../../services/auth.service";

export interface DialogData {
  manualSectionType: string;
  extraData: object;
  deviceAuthorizationResult: DeviceAuthorization;
}

@Component({
  selector: 'app-device-authorization-dialog',
  templateUrl: './device-authorization-dialog.component.html',
  styleUrls: ['./device-authorization.component.scss']
})
export class DeviceAuthorizationDialogComponent {

  loadConfigurationRequestInProgress = true;
  titleCurrentSection = '';
  formDataInvalid = false;
  extraData: object;
  initConfigurationLoaded = false;

  form: FormGroup;

  messageProvider: DeviceAuthorizationType;
  messageProviderType: typeof DeviceAuthorizationType = DeviceAuthorizationType;
  codeExpiryAt: Date;
  failureReason: string;

  constructor (
    private dialogRef: MatDialogRef<DeviceAuthorizationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private formBuilder: FormBuilder
  ) {
    this.form = this.createForm();
    this.messageProvider = this.data.deviceAuthorizationResult.deviceVerificationType;
    this.codeExpiryAt = this.data.deviceAuthorizationResult.codeExpiryAt;
    this.failureReason = this.data.deviceAuthorizationResult.failureReason;
  }

  ngOnInit(): void {
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      code: ['', Validators.required]
    });
  }

  private getFormControl(name: string) {
    return this.form.get(name);
  }

  private isCodeValid() {
    const e = this.getFormControl('code');
    return e && e.valid;
  }

  hasError(name: string) {
    const e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
  }


  sendCode(): void {
    if (!this.isCodeValid()) {
      return;
    }
    this.dialogRef.close(this.getFormControl('code').value);
  }
}

@Component({
  selector: 'app-device-authorization',
  template: '',
  styleUrls: ['./device-authorization.component.scss']
})
export class DeviceAuthorizationComponent implements OnDestroy {

  @Output() authorizationCode: EventEmitter<string> = new EventEmitter<string>();

  private _beforeClosed: Subscription;
  private _afterClosed: Subscription;

  constructor(
    public dialog: MatDialog
  ) {}

  openDialog(data: DeviceAuthorization): void {

    let modalData: DialogData = {
      manualSectionType: '',
      extraData: data,
      deviceAuthorizationResult: data
    } as DialogData;

    const dialogRef = this.dialog.open(DeviceAuthorizationDialogComponent, {
      panelClass: 'full-screen-dialog',
      data: modalData,
      closeOnNavigation: false,
      disableClose: false
    });

    this._afterClosed = dialogRef.afterClosed().subscribe(result => {
      const isClosedAfterCodeTypedIn = result !== false && result !== undefined;
      if (isClosedAfterCodeTypedIn) {
        this.authorizationCode.emit(result);
      }
    });
  }

  ngOnDestroy(): void {
    this._beforeClosed?.unsubscribe();
    this._afterClosed?.unsubscribe();
  }
}
