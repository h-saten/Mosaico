import { selectProjectFaqs } from './../../../store';
import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, OnChanges, SimpleChanges, HostListener } from '@angular/core';
import { SubSink } from 'subsink';
import { validateForm, ErrorHandlingService, FormBase, FormModeEnum, trim } from 'mosaico-base';
import { Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { TokenPageService, Faq, CreateUpdateFaqCommand } from 'mosaico-project';
import { Store } from '@ngrx/store';
import { ProjectPathEnum } from '../../../constants';

@Component({
  selector: 'app-faq-form',
  templateUrl: './faq-form.component.html',
  styleUrls: ['./faq-form.component.scss']
})

export class ProjectFaqFormComponent extends FormBase implements OnInit, OnDestroy, OnChanges {

  private subs: SubSink = new SubSink();

  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  @Input() currentFormMode: FormModeEnum;

  @Input() faqId?: string;
  @Input() projectId: string;
  @Input() pageId: string;
  @Input() currentLang: string;

  @Output() saveEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() cancelEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  titleCol: number;
  answerCol: number;

  itemList: Faq[] = [];

  projectPathEnum: typeof ProjectPathEnum = ProjectPathEnum;

  // necessary - they are used by the parent - modal, sometime for removal
  isEditingIsInProgress = false;
  dataSavingRequestInProgress = false;

  constructor(
    private store: Store,
    private tokenPageService: TokenPageService,
    private toastr: ToastrService,
    private errorHandling: ErrorHandlingService,
    private formBuilder: FormBuilder
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.currentLang?.firstChange === false) {
      this.getProjectFaqs();
    }
  }

  ngOnInit(): void {

    this.createForm();
    this.onResize();

    if (this.currentFormMode === FormModeEnum.Edit) {
      this.getProjectFaqs();
    }
  }
  
  @HostListener('window:resize')
  onResize() {
    this.titleCol = window.innerWidth <= 567 ? 3 : 2;
    this.answerCol = window.innerWidth <= 567 ? 5 : window.innerWidth <= 992 ? 2 : 3; 
  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      content: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(500)]],
      isHidden: [false],
      order: [1],
    });
  }

  private updateFormValue(faq: Faq): void {
    if (faq) {
      this.form.setValue({
        title: faq.title,
        content: faq.content,
        isHidden: faq.isHidden,
        order: faq.order,
      });
    }
  }

  private getProjectFaqs(): void {
    if (this.faqId) {

      this.subs.sink = this.store.select(selectProjectFaqs).subscribe(data => {
        if (data) {
          this.itemList = data;

          if (this.itemList) {
            const projectFaq = this.itemList.find((m) => m.id === this.faqId);
              if (projectFaq) {
                this.updateFormValue(projectFaq);
              }
          }
        }
      });
    }
  }

  save(): void {
    if (validateForm(this.form)) {
      let command = this.form.getRawValue() as CreateUpdateFaqCommand;
      command.order = 1;
      this.form.disable();
      command = trim(command);
      if (this.pageId) {

        // edit
        if (this.faqId) {
          this.subs.sink = this.tokenPageService.updateFAQ(this.pageId, this.faqId, command).subscribe((result) => {
            if (result) {
              this.toastr.success('Faq details were successfully updated');

              this.saveEvent.emit(true);
            }
          }, (error) => {
            this.errorHandling.handleErrorWithToastr(error);
          }, () => {
          });

        // add
        } else {
          this.subs.sink = this.tokenPageService.createFAQ(this.pageId, command).subscribe((result) => {
              if (result) {
                this.toastr.success('Faq details were successfully added');

                this.saveEvent.emit(true);
              }
            }, (error) => {
              this.errorHandling.handleErrorWithToastr(error);
            }, () => {
          });
        }

      } else {
        this.toastr.error('Form has invalid values. Please fix issue to continue');
      }
    } else {
      this.toastr.error('Form has invalid values. Please fix issue to continue');
    }
  }

  cancel(): void {
    this.form.reset();
    this.cancelEvent.emit(true);
  }
}
