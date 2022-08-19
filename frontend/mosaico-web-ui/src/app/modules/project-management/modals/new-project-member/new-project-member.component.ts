import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PROJECT_ROLES } from '../../constants';
import { ToastrService } from 'ngx-toastr';
import { FormDialogBase } from 'mosaico-base';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { validateForm } from 'mosaico-base';

@Component({
  selector: 'app-new-project-member',
  templateUrl: './new-project-member.component.html',
  styleUrls: ['./new-project-member.component.scss']
})
export class NewProjectMemberComponent extends FormDialogBase implements OnInit {

  constructor(private toastr: ToastrService, modal: NgbModal) { super(modal); }

  ngOnInit(): void {
    this.createForm();
  }

  open(): void {
    this.ngOnInit();
    super.open();
  }

  createForm(): void {
    this.form = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.min(5), Validators.max(100)]),
      role: new FormControl(PROJECT_ROLES.MEMBER, [Validators.required])
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
