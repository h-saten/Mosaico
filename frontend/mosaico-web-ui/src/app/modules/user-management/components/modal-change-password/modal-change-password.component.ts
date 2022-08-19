import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { ChangePassword, ConfirmChangePassword } from '../../commands';
import { UserInformation } from '../../models';
import { UserService } from '../../services';
import { selectUserInformation } from '../../store';

import Swal from 'sweetalert2'
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormBase, trim, validateForm } from 'mosaico-base';

@Component({
  selector: 'app-modal-change-password', // this is not modal!
  templateUrl: './modal-change-password.component.html',
  styleUrls: ['./modal-change-password.component.scss']
})
export class ModalChangePasswordComponent extends FormBase implements OnInit {
  userProfile: UserInformation | null | undefined;
  subs: SubSink = new SubSink();

  verificationCode:string;
  verificationCodeTitle:string;
  verificationAlertMessage:string;
  btnFinishTxt:string;
  verificationCodeEmptyErr:string;
  confirmedTitle:string;
  confirmedComment:string;
  errorTitle:string;
  errorComment:string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private store: Store,
    private errorHandler: ErrorHandlingService,
    private userService: UserService,
    private toastr: ToastrService,
    private router: Router,
    private translateService: TranslateService){
    super();
  }

  ngOnInit(): void {
    this.validateForm();
    this.subs.sink = this.store.select(selectUserInformation).subscribe(
      (profile) => {
        this.userProfile = profile;
      }
    );
  }

  private validateForm(): void {
    const reg = /(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&].{8,}/;
    this.form = this.formBuilder.group({
      oldPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.pattern(reg)]],
      confirmNewPassword: ['', [Validators.required, Validators.pattern(reg)]]
    });
  }

  save():void{
    const formValue = this.form.value;

    if (validateForm(this.form) && this.userProfile) {
      this.form.disable();
        let command = {
          id: this.userProfile.id,
          oldPassword:formValue.oldPassword,
          newPassword:formValue.newPassword,
          confirmPassword:formValue.confirmNewPassword
        } as ChangePassword;
        command = trim(command);
        this.userService.ChangePassword(command).subscribe((res) => {
          if(res){
            this.activeModal.close('finish with confirm email send');

            this.verificationCode = this.translateService.instant('CHANGEPASSWORD.verificationCode');
            this.verificationCodeTitle = this.translateService.instant('CHANGEPASSWORD.verificationCodeTitle');
            this.verificationCodeEmptyErr = this.translateService.instant('CHANGEPASSWORD.verificationCodeEmptyErr');
            this.verificationAlertMessage = this.translateService.instant('CHANGEPASSWORD.verificationAlertMessage')
            this.btnFinishTxt = this.translateService.instant('CHANGEPASSWORD.btnFinishTxt');

            Swal.fire({
              icon: 'question',
              title: this.verificationCodeTitle,
              html: '<p class="swal2-html-container">'+this.verificationAlertMessage+'</p><input type="text" id="code" class="swal2-input" placeholder="'+this.verificationCode+'">',
              confirmButtonText: this.btnFinishTxt,
              focusConfirm: false,
              confirmButtonColor: '#0063F5',
              preConfirm: () => {
                const code = (Swal.getPopup()?.querySelector('#code') as HTMLInputElement).value
                if (!code) {
                  Swal.showValidationMessage(this.verificationCodeEmptyErr)
                }
                return { code: code }
              }
            }).then((result) => {
              const commandConfirm = {
                id: this.userProfile?.id,
                oldPassword:formValue.oldPassword,
                newPassword:formValue.newPassword,
                confirmPassword:formValue.confirmNewPassword,
                code:result.value?.code
              } as ConfirmChangePassword;
              this.userService.ConfirmChangePassword(commandConfirm).subscribe((res)=>{
                if(res){
                  this.showSuccessMessage();
                }
              }, (error) => {
                this.showErrorMessage();
              })
            })

          }
        }, (error) => {
          this.errorHandler.handleErrorWithToastr(error);
          this.form.enable();
          this.cleanForm();
        });
    } else {

      this.translateService.get('USERPROFILE.form.invalid.data').subscribe((t) => {
        this.toastr.error(t);
      });

      this.form.enable();
    }
  }

  private showSuccessMessage(): void {

    this.confirmedTitle = this.translateService.instant('CHANGEPASSWORD.confirmedTitle');
    this.confirmedComment = this.translateService.instant('CHANGEPASSWORD.confirmedComment');

    Swal.fire({
      title: this.confirmedTitle,
      html: this.confirmedComment,
      icon: 'success',
      confirmButtonColor: '#0063F5',
    });
    this.form.enable();
    this.cleanForm();
  }

  private showErrorMessage(): void {

    this.errorTitle = this.translateService.instant('CHANGEPASSWORD.errorTitle');
    this.errorComment = this.translateService.instant('CHANGEPASSWORD.errorComment');

    Swal.fire({
      title: this.errorTitle,
      html: this.errorComment,
      icon: 'error',
      confirmButtonColor: '#0063F5',
    });
    this.form.enable();
    this.cleanForm();
  }

  cleanForm(): void {
    this.form.setValue({
      oldPassword: null,
      newPassword: null,
      confirmNewPassword: null
    });
    this.form.markAsUntouched();
    this.form.markAsPristine();
  }

}

