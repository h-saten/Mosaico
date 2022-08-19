import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MatDialogModule} from '@angular/material/dialog';
import {TranslateModule} from '@ngx-translate/core';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {DeviceAuthorizationComponent, DeviceAuthorizationDialogComponent} from "./device-authorization.component";

@NgModule({
  declarations: [DeviceAuthorizationComponent, DeviceAuthorizationDialogComponent],
  entryComponents: [DeviceAuthorizationDialogComponent],
  imports: [
    CommonModule,
    MatDialogModule,
    TranslateModule,
    MatFormFieldModule,
    MatSelectModule,
    FormsModule,
    ReactiveFormsModule,
    ReactiveFormsModule,
    CommonModule,
    CommonModule,
    CommonModule,
    CommonModule,
  ],
  exports: [
    DeviceAuthorizationComponent,
    DeviceAuthorizationDialogComponent
  ]
})
export class DeviceAuthorizationModule { }
