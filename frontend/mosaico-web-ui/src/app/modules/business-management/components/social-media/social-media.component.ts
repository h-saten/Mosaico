import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormBase, trim, validateForm } from 'mosaico-base';
import { CompanyService, UpdateCompanySocialLinks } from 'mosaico-dao';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { selectCompanyPreview } from '../../store';

@Component({
  selector: 'app-social-media',
  templateUrl: './social-media.component.html',
  styleUrls: ['./social-media.component.scss']
})
export class SocialMediaComponent extends FormBase implements OnInit {

  mainForm: FormGroup;
  companyId: string;
  SocialMediaLinks: any;
  loading: boolean = false;
  contentsControl: FormArray = new FormArray([]);
  sub: SubSink = new SubSink();
  constructor(
    private formBuilder: FormBuilder,
    private companyService: CompanyService,
    private store: Store,
    private translateService: TranslateService,
    private toastr: ToastrService,
    private errorHandling: ErrorHandlingService,) {
    super();
  }

  ngOnInit(): void {
    this.createForm();
    this.sub.sink = this.store.select(selectCompanyPreview).subscribe((company) => {
      if (company) {
        this.companyId = company.id;
        this.getSocialLinks();
      }
    });
  }
  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }


  private createForm(): void {
    const reg = /^(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})$/;
    this.mainForm = this.formBuilder.group({
      telegram: ['', [Validators.pattern(reg)]],
      youtube: ['', [Validators.pattern(reg)]],
      linkedIn: ['', [Validators.pattern(reg)]],
      facebook: ['', [Validators.pattern(reg)]],
      twitter: ['', [Validators.pattern(reg)]],
      instagram: ['', [Validators.pattern(reg)]],
      medium: ['', [Validators.pattern(reg)]]
    });
  }
  get m() {
    return this.mainForm.controls;
  }
  private updateFormValue(social: UpdateCompanySocialLinks): void {
    this.mainForm.setValue({
      telegram: social.telegram,
      youtube: social.youtube,
      linkedIn: social.linkedIn,
      facebook: social.facebook,
      twitter: social.twitter,
      instagram: social.instagram,
      medium: social.medium
    });
  }

  getSocialLinks() {
    this.sub.sink = this.companyService.getCompanySocialLinks(this.companyId).subscribe((result) => {
      if (result && result.data) {
        this.SocialMediaLinks = result.data;
        this.updateFormValue(this.SocialMediaLinks);
      }
    }, (error) => {
      setTimeout(() => this.mainForm.enable(), 1500);
    },
    );
  }
  save(): void {
    this.loading = true;
    let command = this.mainForm.getRawValue();
    if (validateForm(this.mainForm)) {
      command = trim(command);
      this.sub.sink = this.companyService.updateCompanySocialLinks(this.companyId, command).subscribe((result) => {
        if (result && result.ok) {
          this.translateService.get('SOCIAL_LINKS.MESSAGE.SUCCESS').subscribe((t) => {
            this.toastr.success(t);
            this.loading = false;
          });
        }
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
        this.loading = false;
      });
    } else {
      this.translateService.get('SOCIAL_LINKS.MESSAGE.FAILED').subscribe((t) => {
        this.toastr.error(t);
        this.loading = false;
      });
    }
  }
}
