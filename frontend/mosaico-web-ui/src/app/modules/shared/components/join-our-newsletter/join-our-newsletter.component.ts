import { Component, OnInit } from '@angular/core';
import {NewsLetterSubscribeService,ErrorHandlingService,trim, validateForm} from 'mosaico-base';
import {ToastrService} from 'ngx-toastr';
import { FormBuilder,FormControl, FormGroup,Validators } from '@angular/forms';
import { SubSink } from 'subsink';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-join-our-newsletter',
  templateUrl: './join-our-newsletter.component.html',
  styleUrls: ['./join-our-newsletter.component.scss']
})
export class JoinOurNewsletterComponent implements OnInit {
  userEmail = "";
  mainForm: FormGroup;
  private sub: SubSink = new SubSink();
  constructor(
    private formBuilder: FormBuilder,
    private newsLetterSubscribeService:NewsLetterSubscribeService,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private errorHandling: ErrorHandlingService,
  ) { }

  ngOnInit(): void {
    this.createForm();
  }

  private createForm(): void {
    this.mainForm = this.formBuilder.group({
      email: new FormControl(null, [Validators.email, Validators.required, Validators.min(5), Validators.max(100)]),
    });
  }

  disableForm(): void {
    this.mainForm.disable();
  }

  enableForm(): void {
    this.mainForm.enable();
  }
  
  save(): void {
    let command = this.mainForm.getRawValue();
    if (validateForm(this.mainForm)) {
    this.disableForm();
      command = trim(command);
      this.sub.sink = this.newsLetterSubscribeService.subscribeNewsLetter(command).subscribe((result) => {
        if (result && result.ok) {
          this.translateService.get('JON.MESSAGES.SUCCESS').subscribe((t) => {
            this.toastr.success(t);
          });
        }
        this.mainForm.reset();
        setTimeout(() => {
          this.enableForm();
        }, 3000)
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
        this.enableForm();
      });
    } else {
      this.translateService.get('JON.MESSAGES.INVALID_FORM').subscribe((t) => {
        this.toastr.error(t);
      });
    }
  }
}
