import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { COMPANY_ROLES } from '../../constants';
import { ToastrService } from 'ngx-toastr';
import { FormDialogBase } from 'mosaico-base';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { validateForm } from 'mosaico-base';
import { CreateTeamMemberCommand } from 'mosaico-dao';

@Component({
  selector: 'app-new-company-member',
  templateUrl: './new-company-member.component.html',
  styleUrls: ['./new-company-member.component.scss']
})
export class NewCompanyMemberComponent extends FormDialogBase implements OnInit {
  companyRoles = COMPANY_ROLES;

  constructor(private toastr: ToastrService, modal: NgbModal) { super(modal); }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(): void {
    this.form = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.min(5), Validators.max(100)]),
      role: new FormControl(null, [Validators.required])
    });
  }

  submit(): void {
    if (validateForm(this.form)) {
      const command = this.form.value as CreateTeamMemberCommand;
      if (this.modalRef) {
        this.modalRef.close(command);
      }
    }
    else {
      this.toastr.error('Form has invalid values. Please fix issue to continue');
    }
  }

}
