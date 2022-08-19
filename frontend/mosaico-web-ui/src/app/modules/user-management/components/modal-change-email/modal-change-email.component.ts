import { AfterViewInit, Component, OnDestroy, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal,NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { ErrorHandlingService, FormBase, trim, validateForm } from 'mosaico-base';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { ChangeEmail } from '../../commands';
import { UserInformation } from '../../models';
import { UserService } from '../../services';
import { selectUserInformation } from '../../store';

@Component({
  selector: 'app-modal-change-email',
  templateUrl: './modal-change-email.component.html',
  styleUrls: ['./modal-change-email.component.scss']
})
export class ModalChangeEmailComponent extends FormBase implements OnInit, OnDestroy{
  userProfile: UserInformation;
  subs: SubSink = new SubSink();
  @Input() isModal = false;

  @Output() changeCanceled = new EventEmitter();
  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private store: Store,
    private errorHandler: ErrorHandlingService,
    private userService: UserService,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private router: Router){
      super();
  }
  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.changeEmailForm();
    this.subs.sink = this.store.select(selectUserInformation).subscribe(
      (profile) => {
        this.userProfile = profile;
      }
    );
  }

  private changeEmailForm(): void {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  save(): void {
    if (validateForm(this.form) && this.userProfile) {
      let formValue = this.form.getRawValue() as ChangeEmail;
      if(formValue.email != this.userProfile.email){
        this.form.disable();
        formValue = trim(formValue);
        this.userService.checkEmailExistence(formValue.email).subscribe((r)=>{
          if(!r.data["exist"]){
            this.subs.sink = this.userService.ChangeEmail(this.userProfile.id, formValue).subscribe((res) => {
              if(res){
                this.translateService.get('USERPROFILE.email.initial.toastrsuccess').subscribe((t) => {
                  this.toastr.success(t);
                });
                this.activeModal.close();
              }
            }, () => { this.translateService.get('USERPROFILE.email.initial.failed').subscribe((t) => {
                  this.toastr.error(t);
                }); this.form.enable(); });
          }
          else{
            this.translateService.get('USERPROFILE.email.initial.exists').subscribe((t) => {
              this.toastr.warning(t);
            });
            this.form.enable();
          }
        })

      }
      else {
        this.translateService.get('USERPROFILE.email.initial.same').subscribe((t) => {
          this.toastr.info(t);
        });
      }
    } else {
      this.translateService.get('USERPROFILE.form.invalid.data').subscribe((t) => {
        this.toastr.error(t);
      });
    }
  }


}
