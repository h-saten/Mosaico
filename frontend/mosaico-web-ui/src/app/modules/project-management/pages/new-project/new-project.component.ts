import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SubSink } from 'subsink';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { PreValidationQuery, ProjectPreValidationResponse, ProjectService } from 'mosaico-project';
import { FormBase, trim, validateForm } from 'mosaico-base';
import { CreateProjectCommand } from '../../../../../../projects/mosaico-project/src/lib/commands/create-project.command';
import { CompanyList, CompanyService } from 'mosaico-dao';
import { Store } from '@ngrx/store';
import { selectUserInformation } from 'src/app/modules/user-management/store';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-new-project',
  templateUrl: './new-project.component.html',
  styleUrls: ['./new-project.component.scss']
})
export class NewProjectComponent extends FormBase implements OnInit, OnDestroy {
  public latestValidationResult: ProjectPreValidationResponse;
  private currentTimeout: NodeJS.Timeout;
  private sub: SubSink = new SubSink();
  public isLoading = true;
  public companyId?: string;
  companies: CompanyList[];
  isSaving$ = new BehaviorSubject<boolean>(false);
  isValidating = false;
  wasEverValidated = false;

  private prevalidationAwaitingTreshold = 1500;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private projectService: ProjectService,
    private toastrService: ToastrService,
    private translateService: TranslateService,
    private route: ActivatedRoute,
    private companyService: CompanyService,
    private store: Store
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.companyId = this.route.snapshot.queryParams.companyId;
    if (!this.companyId || this.companyId.length === 0) {
      this.sub.sink = this.store.select(selectUserInformation).subscribe((user) => {
        if (user) {
          this.sub.sink = this.companyService.getUserCompanies(0, 50, user.id).subscribe((response) => {
            if(response && response.data) {
              this.companies = response.data?.entities;
            }
            this.isLoading = false;
          });
        }
      });
    }
    else{
      this.isLoading = false;
    }
    this.form = this.formBuilder.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      shortDescription: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(180)]],
      companyId: [this.companyId, [Validators.required]]
    });
    this.subscribeToPrevalidation('title');
    this.sub.sink = this.isSaving$.subscribe((newVal) => {
      if(newVal === true) {
        this.form.disable();
      }
      else {
        this.form.enable();
      }
    })
  }

  subscribeToPrevalidation(controlName: string): void {
    if (controlName && controlName.length > 0) {
      this.sub.sink = this.form.get(controlName)?.valueChanges.subscribe((newValue) => {
        if (this.currentTimeout) {
          clearTimeout(this.currentTimeout);
        }
        if (newValue && newValue.length > 0) {
          this.currentTimeout = setTimeout(this.prevalidate.bind(this), this.prevalidationAwaitingTreshold);
        }
      });
    }
  }

  titleHasErrors(): boolean {
    return this.latestValidationResult && !this.latestValidationResult.isTitleValid;
  }

  prevalidate(): void {
    this.isValidating = true;
    const formValue = this.form.getRawValue();
    const query = {
      title: formValue?.title,
    } as PreValidationQuery;
    this.sub.sink = this.projectService.preValidateProject(query).subscribe((res) => {
      this.latestValidationResult = res.data;
      this.isValidating = false;
    }, (error) => {
      this.isValidating = false;
    });
    this.wasEverValidated = true;
  }

  isPrevalidated(): boolean {
    return this.latestValidationResult &&
      this.latestValidationResult.isTitleValid;
  }

  save(): void {
    if (validateForm(this.form) && this.isPrevalidated()) {
      let command = this.form.getRawValue() as CreateProjectCommand;
      if (this.companyId && this.companyId.length > 0) {
        command.companyId = this.companyId;
      }
      this.isSaving$.next(true);
      command = trim(command);
      this.sub.sink = this.projectService.createProject(command).subscribe((result) => {
        if (result && result.data) {
          this.sub.sink = this.translateService.get('NEW_PROJECT.MESSAGES.SUCCESSFULLY_CREATED').subscribe((res) => {
            this.toastrService.success(res);
          });
          this.router.navigateByUrl(`/project/${result.data}`);
        }
      }, (error) => this.isSaving$.next(false));
    }
    else {
      this.sub.sink = this.translateService.get('NEW_PROJECT.MESSAGES.INVALID_FORM').subscribe((res) => {
        this.toastrService.error(res);
      });
    }
  }
}
