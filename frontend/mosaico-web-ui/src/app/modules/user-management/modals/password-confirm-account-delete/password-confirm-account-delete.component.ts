import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FormDialogBase } from 'mosaico-base';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { validateForm } from 'mosaico-base';

@Component({
  selector: 'app-password-confirm-account-delete',
  templateUrl: './password-confirm-account-delete.component.html',
  styleUrls: ['./password-confirm-account-delete.component.scss']
})
export class PasswordConfirmAccountDeleteComponent extends FormDialogBase implements OnInit {

  constructor(private toastr: ToastrService, modal: NgbModal) { super(modal); }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(): void {
    this.form = new FormGroup({
      password: new FormControl('', [Validators.required, Validators.min(5), Validators.max(100)])
    });
  }

  submit(): void {
    if (validateForm(this.form)) {
      const command = this.form.value;
      if (this.modalRef) {
        this.modalRef.close(command);
      }
    }
    else {
      this.toastr.error('Form has invalid values. Please fix issue to continue');
    }
  }

}
