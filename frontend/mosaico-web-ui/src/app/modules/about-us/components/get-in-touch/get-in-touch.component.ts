import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {SubSink} from "subsink";
import {ToastrService} from "ngx-toastr";
import {BehaviorSubject} from "rxjs";
import {finalize} from "rxjs/operators";
import {TranslateService} from "@ngx-translate/core";;
import {ContactService} from "mosaico-base";

@Component({
  selector: 'app-get-in-touch',
  templateUrl: './get-in-touch.component.html',
  styleUrls: ['./get-in-touch.component.scss']
})
export class GetInTouchComponent implements OnInit {

  private subs: SubSink = new SubSink();

  form: FormGroup;

  sendingInProgress$ = new BehaviorSubject<boolean>(false);

  formIsModified = false;

  constructor(
    private formBuilder: FormBuilder,
    private contactClient: ContactService,
    private toastr: ToastrService,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    this.form = this.createForm();
    this.form.valueChanges.subscribe(() => this.formIsModified = true);
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      userName: ['', Validators.required],
      emailAddress: ['', Validators.required],
      message: ['', Validators.required],
      terms: [false, Validators.requiredTrue]
    });
  }

  submit(): void {

    const userName = this.form.controls.userName.value;
    const emailAddress = this.form.controls.emailAddress.value;
    const message = this.form.controls.message.value;

    this.sendingInProgress$.next(true);
    this.contactClient.sendMessage({
      emailAddress: emailAddress,
      userName: userName,
      message: message
    })
    .pipe(finalize(() => {
      this.sendingInProgress$.next(false);
    }))
    .subscribe(() => {
      this.formIsModified = false;
      this.subs.sink = this.translateService.get('FORM.SUCCESS').subscribe((t) => this.toastr.success(t));
      this.form.reset();
    }, () => {
      this.subs.sink = this.translateService.get('FORM.ERROR').subscribe((t) => this.toastr.error(t));
    });
  }

}
