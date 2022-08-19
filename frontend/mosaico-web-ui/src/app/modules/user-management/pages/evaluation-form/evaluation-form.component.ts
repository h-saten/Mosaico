import { Component, OnDestroy, OnInit } from '@angular/core';
import { ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';
import { SubSink } from 'subsink';
import { BehaviorSubject } from 'rxjs';
import { EvaluationService, UserService } from '../../services';
import { EvaluationQuestion } from '../../models';
import { FormArray, FormGroup, FormControl, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { StonlyService } from 'src/app/services/stonly.service';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { setEvaluationCompleted } from '../../store';

@Component({
  selector: 'app-evaluation-form',
  templateUrl: './evaluation-form.component.html',
  styleUrls: ['./evaluation-form.component.scss']
})
export class EvaluationFormComponent extends FormBase implements OnInit, OnDestroy {
  sub = new SubSink();
  isInitialLoading$ = new BehaviorSubject<boolean>(true);
  questions: EvaluationQuestion[] = [];
  questionControls = new FormArray([]);
  saving$ = new BehaviorSubject<boolean>(false);

  public checkAllCheckboxes = false;

  constructor(private service: EvaluationService, private translateService: TranslateService, private toastr: ToastrService,
    private stonlyService: StonlyService, private userService: UserService, private router: Router, private store: Store,
    private errorHandler: ErrorHandlingService) { super(); }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
    this.sub.sink = this.service.getQuestions().subscribe((q) => {
      this.questions = q?.data;
      this.fillFormQuestions();
      this.isInitialLoading$.next(false);
    });
    this.sub.sink = this.saving$.subscribe((newVal) => {
      if(newVal === true) {
        this.form.disable({emitEvent: false});
      }
      else{
        this.form.enable({emitEvent: false});
      }
    });
  }

  createForm(): void {
    this.form = new FormGroup({
      questions: this.questionControls,
      notForbiddenCitizenship: new FormControl('', [
        Validators.requiredTrue
      ]),
      newsletterPersonalDataProcessing: new FormControl(false),
      checkAll: new FormControl(false),
      terms: new FormControl('', [
        Validators.requiredTrue
      ])
    });
  }

  fillFormQuestions(): void {
    if(this.questions && this.questions.length > 0) {
      this.questions.forEach((q) => {
        const formGroup = this.getQuestionForm(q.title, q.key);
        this.questionControls.push(formGroup);
      });
    }
  }

  private getQuestionForm(title: string, key: string): FormGroup{
    const form = new FormGroup({
      title: new FormControl(title, [Validators.required]),
      key: new FormControl(key, [Validators.required]),
      response: new FormControl(null)
    });
    this.sub.sink = form.get('response')?.valueChanges.subscribe((res) => {
      if(res === 'false' && !this.saving$.value){
        this.stonlyService.openGuide(key);
      }
    });
    return form;
  }

  getControls(): FormGroup[] {
    return this.questionControls.controls.map((a) => a as FormGroup);
  }

  submit(): void {
    if(validateForm(this.form) && this.validateResponses()){
      this.saving$.next(true);
      const responses = this.questionControls.controls.map((c) => {
        return {
          title: c.get('title')?.value,
          key: c.get('key')?.value,
          response: c.get('response')?.value
        };
      });
      this.sub.sink = this.userService.completeEvaluation(responses).subscribe((res) => {
        this.translateService.get('EVALUATION.MESSAGES.SUCCESS').subscribe((t) => {
          this.toastr.success(t);
        });
        this.store.dispatch(setEvaluationCompleted({completed: true}));
        this.router.navigateByUrl('/')
      }, (error) => { this.saving$.next(false); this.errorHandler.handleErrorWithToastr(error); });
    }
    else {
      this.translateService.get('EVALUATION.MESSAGES.INVALID_FORM').subscribe((res) => {
        this.toastr.error(res);
      });
    }
  }

  validateResponses(): boolean {
    let result = true;
    this.questionControls?.controls?.forEach((c) => {
      result = c.get('response')?.value === 'true' || c.get('response')?.value === 'false';
      if(!result){
        return;
      }
    });
    return result;
  }

  displayKb(key?: string): void {
    if(key && key.length > 0) {
      this.stonlyService.openGuide(key);
    }
  }

  checkAllCheck() {

    this.checkAllCheckboxes = !this.checkAllCheckboxes;
    const state = this.checkAllCheckboxes;

    const controls = this.form.controls;

    controls.terms.setValue(state);
    controls.terms.markAsTouched();

    controls.notForbiddenCitizenship.setValue(state);
    controls.notForbiddenCitizenship.markAllAsTouched();

    controls.newsletterPersonalDataProcessing.setValue(state);
  }

  changeConditions() {
    const controls = this.form.controls;
    if(this.form.value && this.form.controls.notForbiddenCitizenship.value && this.form.controls.newsletterPersonalDataProcessing.value) {
      this.checkAllCheckboxes = true;
      controls.checkAll.setValue(true);
    } else {
      controls.checkAll.setValue(false);
      this.checkAllCheckboxes = false;
    }
  }

  getFormControl(name: string) {
    return this.form.get(name);
  }

  // Return TRUE, if FormControl element is not correct after value has been changed
  hasError(name: string): boolean {
    const e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
  }

}
